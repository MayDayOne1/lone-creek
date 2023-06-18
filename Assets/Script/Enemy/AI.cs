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
    private float aimRigWeight;
    private Rigidbody[] childrenRB;

    protected State currentState;
    public Transform Player;
    public GameObject[] waypoints;
    // public GameObject bullet;
    // public Transform muzzle;
    public float bulletSpeed = 10f;
    public Slider HealthSlider;
    public AudioSource Gunshot;
    public ParticleSystem MuzzleFlash;
    public PlayerController playerController;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        currentState = new Idle(this.gameObject, Player, agent, waypoints, anim);
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
        } else
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
    public bool GetCanAttackPlayer() => currentState.CanAttackPlayer();
    private void PursuePlayerWhenShot()
    {
        currentState.WalkTowardsPlayer();
        anim.SetBool("IsPursuing", true);
        agent.speed = 4;
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
        EnableAim();
        Gunshot.Play();
        MuzzleFlash.Play();
        // Debug.Log("Start shooting");
        Vector3 dirTowardsPlayer = Player.transform.position - this.transform.position;
        if (playerController.IsCrouching) dirTowardsPlayer.y += .5f;
        else dirTowardsPlayer.y += 1f;
        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, dirTowardsPlayer, out hit, 999f))
        {
            if(hit.transform.gameObject.tag.Equals("Player"))
            {
                Player.GetComponent<PlayerController>().PlayerTakeDamage(rifleDamage);
            }
        }
    }
    
}