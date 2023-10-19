using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;
using UnityEngine.UI;

public class AI : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator anim;
    private float health = 1f;
    private float rifleDamage = .25f;
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
        if(health > .01f)
        {
            PursuePlayerWhenShot();
        }

        if(!HealthSlider.gameObject.activeSelf)
        {
            HealthSlider.gameObject.SetActive(true);
        }

        health -= damage;
        HealthSlider.value = health;
        if (health < .01f)
        {
            health = 0f;
            Die();
        }
    }
    public void ShootAtPlayer()
    {
        if(agent.enabled)
        {
            // Debug.Log("Shooting");
            EnableAim();
            Gunshot.Play();
            MuzzleFlash.Play();
            anim.SetTrigger("Shoot");
            // Debug.Log("Shot by " + this.name);
            Vector3 dirTowardsPlayer = targetForEnemy.position - this.transform.position;
            Transform bullet = Instantiate(DummyBullet, dirTowardsPlayer, Quaternion.identity);
            bullet.GetComponent<Rigidbody>().AddForce(dirTowardsPlayer, ForceMode.Acceleration);
            Destroy(bullet.gameObject, .5f);

            RaycastHit hit;
            if (Physics.Raycast(this.transform.position, dirTowardsPlayer, out hit, 999f))
            {
                if (hit.transform.gameObject.tag.Equals("Player"))
                {
                    float chance = Random.Range(0f, 1f);
                    // Debug.Log("chance: " + chance);
                    if(chance < hitChance)
                        Player.GetComponent<PlayerController>().PlayerTakeDamage(rifleDamage);
                }
            }
        }

    }
}