using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;
using UnityEngine.UI;
using DG.Tweening;

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
    public Transform Player;
    public PlayerController playerController;
    public Transform targetForEnemy;

    [Header("SHOOTING")]
    public Transform muzzle;
    public AudioSource Gunshot;
    public ParticleSystem MuzzleFlash;
    [SerializeField][Range (0f, 1f)] private float hitChance = .8f;
    [SerializeField] private float rifleDamage = .25f;
    [SerializeField] private Rig aimRig;
    private float aimRigWeight;

    [SerializeField] private EnemySoundManager soundManager;
    protected State currentState;
    private NavMeshAgent agent;
    private Animator anim;

    void Start()
    {
        healthSlider.gameObject.SetActive(false);
        childrenRB = this.GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in childrenRB)
        {
            rb.isKinematic = true;
        }

        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        currentState = new Idle(this.gameObject, targetForEnemy, agent, waypoints, anim);
    }

    void Update()
    {
        currentState = currentState.Process();
        aimRig.weight = Mathf.Lerp(aimRigWeight, aimRigWeight, Time.deltaTime * 20f);
        if (currentState.CanSeePlayer())
        {
            aimRigWeight = 1f;
        }
        else
        {
            aimRigWeight = 0f;
        }
    }
    private void Die()
    {
        soundManager.isAlive = false;
        soundManager.EmitDeathSound();
        DisableEnemy();
        ActivateRagdoll();
        
#if ENABLE_CLOUD_SERVICES_ANALYTICS
        PlayerController.enemiesKilled++;
        AnalyticsService.Instance.CustomData("enemyDie", new Dictionary<string, object>()
            {
                { "playerHealth", PlayerController.health },
                { "playerHealthKitCount", PlayerController.playerHealthKitCount },
                { "playerDeathCount", PlayerController.playerDeathCount },
                { "playerPistolAmmo", PlayerAmmoManager.currentAmmo + PlayerAmmoManager.currentClip },
                { "playerAmmoClipCount", PlayerInteract.playerAmmoClipCount },
                { "playerBottleCount",  PlayerInteract.playerBottleCount },
                { "playerBottleThrowCount", PlayerShootingManager.playerBottleThrowCount },
                { "playerShotsFiredCount", PlayerShootingManager.playerShotsFiredCount },
                { "enemiesKilled", PlayerController.enemiesKilled },
                { "enemyShotsFiredCount", PlayerController.enemyShotsFiredCount },
                { "enemyShotsHit", PlayerController.enemyShotsHit },
                { "playerPistolsPickedUp", PlayerInteract.playerPistolsPickedUp },
                { "playerShotsHit", PlayerShootingManager.playerShotsHit }
            });
#endif
    }

    private void DisableEnemy()
    {
        aimRigWeight = 0f;
        agent.enabled = false;
        this.enabled = false;
        anim.enabled = false;
        healthSlider.gameObject.SetActive(false);
    }
    private void ActivateRagdoll()
    {
        foreach (Rigidbody r in childrenRB)
        {
            r.gameObject.tag = "Untagged";
            r.isKinematic = false;
        }
    }
    private void PursuePlayerWhenShot()
    {
        if(currentState.stateName != State.STATE.ATTACK) 
        {
            currentState.WalkTowardsPlayer();
            agent.isStopped = false;
            agent.speed = 4;
        }       
        
    }
    public void TakeDamage(float damage)
    {
        if(!isInvincible)
        {
            if (!healthSlider.gameObject.activeSelf)
            {
                healthSlider.gameObject.SetActive(true);
            }

            health -= damage;
            healthSlider.DOValue(health, .2f, false);
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
            StartCoroutine(Invincibility());
        }
    }
    private IEnumerator Invincibility()
    {
        isInvincible = true;
        yield return new WaitForSeconds(.25f);
        isInvincible = false;
    }
    public void ShootAtPlayer()
    {
        if(agent.enabled && PlayerController.health > 0f)
        {
            Shoot();
            Vector3 dirTowardsPlayer = targetForEnemy.position - muzzle.position;

            if (Physics.Raycast(muzzle.position, dirTowardsPlayer, out RaycastHit hit, 999f))
            {
                if (hit.transform.gameObject.layer == 6)
                {
                    if(CalculateHitChance() < hitChance)
                    {
                        Player.GetComponent<PlayerController>().PlayerTakeDamage(rifleDamage);
#if ENABLE_CLOUD_SERVICES_ANALYTICS
                        PlayerController.enemyShotsHit++;
#endif
                    }
                }
            }
#if ENABLE_CLOUD_SERVICES_ANALYTICS
            PlayerController.enemyShotsFiredCount++;
#endif
        }
    }
    private void Shoot()
    {
        aimRigWeight = 1f;
        Gunshot.Play();
        MuzzleFlash.Play();
        anim.SetTrigger("Shoot");
    }
    private float CalculateHitChance()
    {
        float chance = Random.Range(0f, 1f);
        float roundChance = Mathf.Round(chance * 100f) / 100f;
        return roundChance;
    }
}