using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator anim;
    private bool canShoot = true;

    protected State currentState;
    public Transform Player;
    public GameObject[] waypoints;
    // public GameObject bullet;
    // public Transform muzzle;
    public float bulletSpeed = 10f;

    IEnumerator Standby(int s)
    {
        yield return new WaitForSeconds(s);
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        currentState = new Idle(this.gameObject, Player, agent, waypoints, anim);
    }

    void Update()
    {
        currentState = currentState.Process();
        if(currentState.CanAttackPlayer())
        {
            // var bulletInstance = Instantiate(bullet, muzzle.position, muzzle.rotation);
            // bulletInstance.GetComponent<Rigidbody>().velocity = muzzle.transform.forward * bulletSpeed;
            canShoot = false;
            Standby(1);
            // Destroy(bulletInstance, 1.0f);
            canShoot = true;
        }
    }
}
