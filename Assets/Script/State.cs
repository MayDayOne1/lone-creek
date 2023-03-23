using System.Collections;
using System.Collections.Generic;
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

    public STATE stateName;
    protected EVENT eventName;
    protected GameObject npc;
    protected Transform player;
    protected State nextState;
    protected NavMeshAgent agent;

    public float visDist = 10.0f;
    public float visAngle = 30.0f;
    public float attackDist = 1.0f;

    public State(GameObject _npc, Transform _player, NavMeshAgent _agent)
    {
        npc = _npc;
        player = _player;
        agent = _agent;
        eventName = EVENT.ENTER;
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
        if(direction.magnitude < attackDist)
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
    public Idle(GameObject _npc, Transform _player, NavMeshAgent _agent)
        : base(_npc, _player, _agent)
    {
        stateName = STATE.IDLE;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        if(CanSeePlayer())
        {
            nextState = new Pursue(npc, player, agent);
            eventName = EVENT.EXIT;
        }
        else if(Random.Range(0, 100) < 10)
        {
            nextState = new Patrol(npc, player, agent);
            eventName = EVENT.EXIT;
        }
    }
}

public class Patrol : State
{
    int currentIndex = -1;
    public Patrol(GameObject _npc, Transform _player, NavMeshAgent _agent)
        : base(_npc, _player, _agent)
    {
        stateName = STATE.PATROL;
        agent.speed = 2;
        agent.isStopped = false;
    }

    public override void Enter()
    {
        float lastDist = Mathf.Infinity;
        for(int i = 0; i < GameEnvironment.Instance.Checkpoints.Count; i++)
        {
            GameObject thisWaypoint = GameEnvironment.Instance.Checkpoints[i];
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
            if(currentIndex >= GameEnvironment.Instance.Checkpoints.Count - 1)
            {
                currentIndex = 0;
            } else
            {
                currentIndex++;
            }
            // Debug.Log("currentIndex: " + currentIndex);
            agent.SetDestination(GameEnvironment.Instance.Checkpoints[currentIndex].transform.position);
        }

        if (CanSeePlayer())
        {
            nextState = new Pursue(npc, player, agent);
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
    public Pursue(GameObject _npc, Transform _player, NavMeshAgent _agent)
        : base(_npc, _player, _agent)
    {
        stateName = STATE.PURSUIT;
        agent.speed = 5;
        agent.isStopped = false;
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
                nextState = new Attack(npc, player, agent);
                eventName = EVENT.EXIT;
            }
            else if(!CanSeePlayer())
            {
                nextState = new Patrol(npc, player, agent);
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

    public Attack(GameObject _npc, Transform _player, NavMeshAgent _agent)
        : base(_npc, _player, _agent)
    {
        stateName = STATE.ATTACK;
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
            nextState = new Idle(npc, player, agent);
            eventName = EVENT.EXIT;
        }
    }
}


