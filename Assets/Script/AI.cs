using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    private NavMeshAgent agent;
    protected State currentState;
    public Transform Player;

    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        currentState = new Idle(this.gameObject, Player, agent);
    }

    void Update()
    {
        currentState = currentState.Process();
    }
}
