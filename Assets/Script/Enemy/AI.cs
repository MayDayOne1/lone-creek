using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Zenject.SpaceFighter;

public class AI : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator anim;
    private float health = 1f;
    private float rifleDamage = .25f;
    private bool alreadyAttacked;

    protected State currentState;
    public Transform Player;
    public GameObject[] waypoints;
    // public GameObject bullet;
    // public Transform muzzle;
    public float bulletSpeed = 10f;
    public Slider HealthSlider;
    public float Cooldown = 3f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        currentState = new Idle(this.gameObject, Player, agent, waypoints, anim);
    }

    void Update()
    {
        currentState = currentState.Process();
        if (GetCanAttackPlayer())
        {
            ShootAtPlayer();
        }
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
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
    private void Fire()
    {
        Debug.Log("Start shooting");
        Vector3 dirTowardsPlayer = Player.transform.position - this.transform.position;
        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, dirTowardsPlayer, out hit, 999f))
        {
            Player.GetComponent<PlayerController>().PlayerTakeDamage(rifleDamage);
        }
    }
    private void ShootAtPlayer()
    {
        if (!alreadyAttacked)
        {
            Fire();
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), Cooldown);
        }
    }
}