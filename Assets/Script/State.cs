using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;

public class State
{
    public enum STATE
    {
        IDLE, PATROL, PURSUIT, ATTACK, DEAD
    };

    public enum EVENT
    {
        ENTER, UPDATE, EXIT
    };
    public GameObject npc;
    public STATE stateName;
    protected EVENT eventName;
    protected Transform player;
    protected State nextState;
    protected NavMeshAgent agent;
    protected GameObject[] waypoints;
    protected Animator animator;
    public float visDist = 12.0f;
    public float visAngle = 30.0f;
    public float attackDist = 10.0f;

    public State(GameObject _npc, Transform _player, NavMeshAgent _agent, GameObject[] waypoints)
    {
        npc = _npc;
        player = _player;
        agent = _agent;
        eventName = EVENT.ENTER;
        this.waypoints = waypoints;
    }

    public virtual void Enter()
    {
        eventName = EVENT.UPDATE;
    }

    public virtual void Update()
    {
        eventName = EVENT.UPDATE;
    }

    public virtual void Exit()
    {
        eventName = EVENT.EXIT;
    }

    public bool CanSeePlayer()
    {
        Vector3 direction = player.position - npc.transform.position;
        float angle = Vector3.Angle(direction, npc.transform.forward);

        if(direction.magnitude < visDist && angle < visAngle)
        {
            return true;
        }

        return false;
    }

    public bool CanAttackPlayer()
    {
        Vector3 direction = player.position - npc.transform.position;
        float angle = Vector3.Angle(direction, npc.transform.forward);
        if (direction.magnitude < attackDist && angle < visAngle)
        {
            return true;
        }

        return false;
    }

    public State Process()
    {
        if (eventName == EVENT.ENTER) Enter();
        if (eventName == EVENT.UPDATE) Update();
        if (eventName == EVENT.EXIT)
        {
            Exit();
            return nextState;
        }

        return this;
    }
}

public class Idle : State
{
    public Idle(GameObject _npc, Transform _player, NavMeshAgent _agent, GameObject[] waypoints)
        : base(_npc, _player, _agent, waypoints)
    {
        stateName = STATE.IDLE;
        Debug.Log("Idle");
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        if(CanSeePlayer())
        {
            nextState = new Pursue(npc, player, agent, waypoints);
            eventName = EVENT.EXIT;
        }
        else if(Random.Range(0, 100) < 10)
        {
            nextState = new Patrol(npc, player, agent, waypoints);
            eventName = EVENT.EXIT;
        }
    }
}

public class Patrol : State
{
    int currentIndex = -1;
    public Patrol(GameObject _npc, Transform _player, NavMeshAgent _agent, GameObject[] waypoints)
        : base(_npc, _player, _agent, waypoints)
    {
        stateName = STATE.PATROL;
        agent.speed = 5;
        agent.isStopped = false;
        Debug.Log("Patrol");
    }

    public override void Enter()
    {
        float lastDist = Mathf.Infinity;
        for(int i = 0; i < waypoints.Length; i++)
        {
            GameObject thisWaypoint = waypoints[i];
            float distance = Vector3.Distance(npc.transform.position, thisWaypoint.transform.position);
            if(distance < lastDist)
            {
                currentIndex = i - 1;
                lastDist = distance;
            }
        }
        base.Enter();
    }

    public override void Update()
    {
        if(agent.remainingDistance < 1)
        {
            // int c = GameEnvironment.Instance.Checkpoints.Count - 1;
            // Debug.Log("checkpoints count - 1: " + c);
            if(currentIndex >= waypoints.Length - 1)
            {
                currentIndex = 0;
            } else
            {
                currentIndex++;
            }
            // Debug.Log("currentIndex: " + currentIndex);
            agent.SetDestination(waypoints[currentIndex].transform.position);
        }

        if (CanSeePlayer())
        {
            nextState = new Pursue(npc, player, agent, waypoints);
            eventName = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}

public class Pursue : State
{
    public Pursue(GameObject _npc, Transform _player, NavMeshAgent _agent, GameObject[] waypoints)
        : base(_npc, _player, _agent, waypoints)
    {
        stateName = STATE.PURSUIT;
        agent.speed = 7;
        agent.isStopped = false;
        Debug.Log("Pursue");
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        agent.SetDestination(player.position);
        if(agent.hasPath)
        {
            if(CanAttackPlayer())
            {
                nextState = new Attack(npc, player, agent, waypoints);
                eventName = EVENT.EXIT;
            }
            else if(!CanSeePlayer())
            {
                nextState = new Patrol(npc, player, agent, waypoints);
                eventName = EVENT.EXIT;
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}

public class Attack : State
{
    float rotationSpeed = 2.0f;

    public Attack(GameObject _npc, Transform _player, NavMeshAgent _agent, GameObject[] waypoints)
        : base(_npc, _player, _agent, waypoints)
    {
        stateName = STATE.ATTACK;
        Debug.Log("Attack");
    }

    public override void Enter()
    {
        agent.isStopped = true;
        base.Enter();
    }

    public override void Update()
    {
        Vector3 direction = player.position - npc.transform.position;
        float angle = Vector3.Angle(direction, npc.transform.forward);
        direction.y = 0;

        npc.transform.rotation = Quaternion.Slerp(npc.transform.rotation,
                                    Quaternion.LookRotation(direction),
                                    rotationSpeed * Time.deltaTime);

        if(!CanAttackPlayer())
        {
            nextState = new Idle(npc, player, agent, waypoints);
            eventName = EVENT.EXIT;
        }
    }
}


