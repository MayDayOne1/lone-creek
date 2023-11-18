using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;

public class AI : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator anim;
    private float health = 1f;
    [SerializeField] private float rifleDamage = .25f;
    private bool isInvincible = false;
    [SerializeField] private Rig aimRig;
    [SerializeField][Range (0f, 1f)] private float hitChance = .7f;
    private float aimRigWeight;
    private Rigidbody[] childrenRB;
    public Transform DummyBullet;
    public Transform muzzle;

    protected State currentState;
    public Transform Player;
    public Transform targetForEnemy;
    public GameObject[] waypoints;
    // public GameObject bullet;
    // public Transform muzzle;
    public Slider HealthSlider;
    public AudioSource Gunshot;
    public ParticleSystem MuzzleFlash;
    public PlayerController playerController;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        currentState = new Idle(this.gameObject, targetForEnemy, agent, waypoints, anim);
        HealthSlider.gameObject.SetActive(false);

        childrenRB = this.GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in childrenRB)
        {
            rb.isKinematic = true;
        }
    }

    void Update()
    {
        currentState = currentState.Process();
        aimRig.weight = Mathf.Lerp(aimRigWeight, aimRigWeight, Time.deltaTime * 20f);
        if (currentState.CanSeePlayer())
        {
            EnableAim();
        }
        else
        {
            DisableAim();
        }
    }
    private void EnableAim()
    {
        aimRigWeight = 1f;
    }
    private void DisableAim()
    {
        aimRigWeight = 0f;
    }
    private void Die()
    {
        DisableAim();
        agent.enabled = false;
        this.enabled = false;
        anim.enabled = false;
        HealthSlider.gameObject.SetActive(false);
        foreach (Rigidbody r in childrenRB)
        {
            r.isKinematic = false;
        }
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
    private void PursuePlayerWhenShot()
    {
        if(currentState.stateName != State.STATE.ATTACK) 
        {
            currentState.WalkTowardsPlayer();
            agent.isStopped = false;
            //anim.SetBool("IsPatrolling", false);
            //anim.SetBool("IsPursuing", true);
            agent.speed = 4;
        }       
        
    }
    public void TakeDamage(float damage)
    {
        if(!isInvincible)
        {
            if (health > .01f)
            {
                PursuePlayerWhenShot();
            }

            if (!HealthSlider.gameObject.activeSelf)
            {
                HealthSlider.gameObject.SetActive(true);
            }

            health -= damage;
            HealthSlider.DOValue(health, .2f, false);
            if (health < .01f)
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
        // Debug.Log("not invincible");
    }
    public void ShootAtPlayer()
    {
        if(agent.enabled && playerController.GetHealth() > 0f)
        {
            // Debug.Log("Shooting");
            EnableAim();
            Gunshot.Play();
            MuzzleFlash.Play();
            anim.SetTrigger("Shoot");
            // Debug.Log("Shot by " + this.name);
            Vector3 dirTowardsPlayer = targetForEnemy.position - muzzle.position;
            // Transform bullet = Instantiate(DummyBullet, dirTowardsPlayer, Quaternion.identity);
            // bullet.GetComponent<Rigidbody>().AddForce(dirTowardsPlayer, ForceMode.Acceleration);
            // Destroy(bullet.gameObject, .5f);

            RaycastHit hit;
            if (Physics.Raycast(muzzle.position, dirTowardsPlayer, out hit, 999f))
            {
                // Debug.DrawRay(muzzle.position, dirTowardsPlayer * 999f, Color.red, 2f);
                if (hit.transform.gameObject.CompareTag("Player"))
                {
                    float chance = Random.Range(0f, 1f);
                    // Debug.Log("chance: " + chance);
                    if(chance < hitChance)
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
}