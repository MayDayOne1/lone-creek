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
        if(Random.Range(0, 100) < 10)
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
        currentIndex = 0;
        base.Enter();
    }

    public override void Update()
    {
        if(agent.remainingDistance < 1)
        {
            if(currentIndex >= GameEnvironment.Instance.Checkpoints.Count - 1)
            {
                currentIndex = 0;
            } else
            {
                currentIndex++;
            }

            agent.SetDestination(GameEnvironment.Instance.Checkpoints[currentIndex].transform.position);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
