using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;
using UnityEngine.UI;
using DG.Tweening;
using MEC;
using Zenject;

#if ENABLE_CLOUD_SERVICES_ANALYTICS
using Unity.Services.Analytics;
#endif

public class AI : MonoBehaviour
{
    [Header("HEALTH")]
    [SerializeField] private Slider healthSlider;
    private float health = 1f;
    private bool isInvincible = false;
    private Rigidbody[] childrenRB;

    [Header("MOVEMENT")]
    public GameObject[] waypoints;

    [Header("PLAYER")]
    [Inject] private PlayerController controller;
    [Inject] private TargetForEnemy targetForEnemy;
    private const int PLAYER_LAYER = 6;


    [Header("SHOOTING")]
    public Transform muzzle;
    public AudioSource Gunshot;
    public ParticleSystem MuzzleFlash;
    public TrailRenderer bulletTrail;
    public IEnumerator<float> aimCoroutine;
    [SerializeField][Range (0f, 1f)] private float hitChance = .8f;
    [SerializeField] private float rifleDamage = .25f;
    [SerializeField] private Rig aimRig;
    [SerializeField] private Transform aimTarget;
    [SerializeField] private float rigBlendTime = .2f;
    [SerializeField] private LayerMask aimColliderLayerMask = new();

    [SerializeField] private EnemySoundManager soundManager;

    protected State currentState;
    private NavMeshAgent agent;
    private Animator anim;

    public float Health
    { 
        get
        {
            return health;
        }
        set
        {
            health = value;
            healthSlider.DOValue(health, .2f, false);
        }
    }
    private Vector3 DirectionTowardsPlayer
    {
        get
        {
            return targetForEnemy.transform.position - muzzle.position;
        }
    }

#if ENABLE_CLOUD_SERVICES_ANALYTICS
    [Inject] AnalyticsManager analyticsManager;
#endif

    void Start()
    {
        healthSlider.gameObject.SetActive(false);
        childrenRB = this.GetComponentsInChildren<Rigidbody>();

        ActivateRagdoll(false);

        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        currentState = new Idle(this.gameObject, targetForEnemy.transform, agent, waypoints, anim);
        if(waypoints.Length == 0)
        {
            throw new System.Exception(gameObject.name + " has no waypoints! Assign them in the Inspector.");
        }

        aimRig.weight = 0f;
    }

    void Update()
    {
        currentState = currentState.Process();
    }
    
    public void TakeDamage(float damage)
    {
        if(!isInvincible)
        {
            if (!healthSlider.gameObject.activeSelf)
            {
                healthSlider.gameObject.SetActive(true);
            }

            Health -= damage;

            if (health > .01f)
            {
                PursuePlayerWhenShot();
                soundManager.EmitDamageSound();
            }
            else
            {
                health = 0f;
                Die();
            }
            Timing.RunCoroutine(Invincibility().CancelWith(gameObject));
        }
    }

    public void ShootAtPlayer()
    {
        if(agent.enabled && PlayerParams.health > 0f)
        {
            Shoot();
            TrailRenderer trail = Instantiate(bulletTrail, muzzle.position, Quaternion.identity);

            if (Physics.Raycast(muzzle.position, DirectionTowardsPlayer, out RaycastHit hit, 999f, aimColliderLayerMask))
            {
                Timing.RunCoroutine(MoveTrail(trail, hit));
                if (HasPlayerLayer(hit.transform))
                {
                    if(CalculateHitChance() < hitChance)
                    {
                        controller.PlayerTakeDamage(rifleDamage);
#if ENABLE_CLOUD_SERVICES_ANALYTICS
                        PlayerParams.enemyShotsHit++;
#endif
                    }
                }
            }
#if ENABLE_CLOUD_SERVICES_ANALYTICS
            PlayerParams.enemyShotsFiredCount++;
#endif
        }
    }

    public void SetAimRigWeight(float newWeight)
    {
        Timing.RunCoroutine(UpdateAimTarget().CancelWith(gameObject));
        if (Time.timeScale > 0f)
        {
            LeanTween.value(gameObject, aimRig.weight, newWeight, rigBlendTime)
                    .setOnUpdate((value) =>
                    {
                        aimRig.weight = value;
                    });

        }
    }

    private bool HasPlayerLayer(Transform t) => t.gameObject.layer == PLAYER_LAYER;

    private IEnumerator<float> Invincibility()
    {
        isInvincible = true;
        yield return Timing.WaitForSeconds(.25f);
        isInvincible = false;
    }

    private void Shoot()
    {
        SetAimRigWeight(1f);
        Gunshot.Play();
        MuzzleFlash.Play();
        anim.SetTrigger("Shoot");
    }

    private IEnumerator<float> MoveTrail(TrailRenderer trail, RaycastHit hit)
    {
        float time = 0;
        while(time < 1)
        {
            trail.transform.position = Vector3.Lerp(muzzle.transform.position, hit.point, time);
            if(trail.gameObject.activeSelf)
            {
                time += Time.deltaTime / trail.time;
                yield return Timing.WaitForOneFrame;
            }
            
        }
        if (trail.gameObject.activeSelf)
        {
            trail.transform.position = hit.point;
            Destroy(trail, trail.time);
        }
    }

    private float CalculateHitChance()
    {
        float chance = Random.Range(0f, 1f);
        float roundChance = Mathf.Round(chance * 100f) / 100f;
        return roundChance;
    }

    private IEnumerator<float> UpdateAimTarget()
    {
        while (aimRig.weight >= 0f)
        {
            if (Physics.Raycast(muzzle.position,
                DirectionTowardsPlayer,
                out RaycastHit hit,
                999f,
                aimColliderLayerMask))
            {
                aimTarget.position = hit.point;
            }
            yield return Timing.WaitForSeconds(.1f);
        }
    }

    private void Die()
    {
        soundManager.isAlive = false;
        soundManager.EmitDeathSound();
        DisableEnemy();
        ActivateRagdoll(true);

#if ENABLE_CLOUD_SERVICES_ANALYTICS
        PlayerParams.enemiesKilled++;
        analyticsManager.SendEnemyDie();
#endif
    }

    private void DisableEnemy()
    {
        agent.enabled = false;
        this.enabled = false;
        anim.enabled = false;
        healthSlider.gameObject.SetActive(false);
    }

    private void ActivateRagdoll(bool activate)
    {
        if (activate)
        {
            foreach (Rigidbody r in childrenRB)
            {
                r.gameObject.tag = "Untagged";
                r.isKinematic = false;
            }
        }
        else
        {
            foreach (Rigidbody rb in childrenRB)
            {
                rb.isKinematic = true;
            }
        }

    }

    private void PursuePlayerWhenShot()
    {
        if (currentState.stateName != State.STATE.ATTACK)
        {
            currentState.WalkTowardsPlayer();
            agent.isStopped = false;
            agent.speed = 4;
        }

    }
}