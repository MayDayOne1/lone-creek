using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;
using UnityEngine.UI;
using Zenject.SpaceFighter;

public class AI : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator anim;
    private float health = 1f;
    private float rifleDamage = .25f;
    [SerializeField] private Rig aimRig;
    private float aimRigWeight;

    protected State currentState;
    public Transform Player;
    public GameObject[] waypoints;
    // public GameObject bullet;
    // public Transform muzzle;
    public float bulletSpeed = 10f;
    public Slider HealthSlider;
    public AudioSource Gunshot;
    public ParticleSystem MuzzleFlash;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        currentState = new Idle(this.gameObject, Player, agent, waypoints, anim);
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

    public bool GetCanAttackPlayer()
    {
        return currentState.CanAttackPlayer();
    }
    public void TakeDamage(float damage)
    {
        health -= damage;
        HealthSlider.value = health;
        if (health <= 0)
        {
            health = 0;
            // Debug.Log("enemy dead");
        }
    }
    public void ShootAtPlayer()
    {
        Gunshot.Play();
        MuzzleFlash.Play();
        // Debug.Log("Start shooting");
        Vector3 dirTowardsPlayer = Player.transform.position - this.transform.position;
        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, dirTowardsPlayer, out hit, 999f))
        {
            Player.GetComponent<PlayerController>().PlayerTakeDamage(rifleDamage);
        }
    }
    
}