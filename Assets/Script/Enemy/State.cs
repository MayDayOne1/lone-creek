using UnityEngine;
using UnityEngine.AI;

public class State
{
    public enum STATE
    {
        IDLE, PATROL, PURSUIT, ATTACK
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
    public float visAngle = 90.0f;
    public float attackDist = 10.0f;

    PlayerController playerController;

    protected float attackCooldown = 2f;
    protected float attackTimer = 0f;
    protected float patrolSpeed = 2f;
    protected float pursueSpeed = 3f;
    public State(GameObject _npc, Transform _player, NavMeshAgent _agent, GameObject[] waypoints, Animator anim)
    {
        npc = _npc;
        player = _player;
        agent = _agent;
        eventName = EVENT.ENTER;
        this.waypoints = waypoints;
        animator = anim;
        playerController = player.gameObject.GetComponent<PlayerController>();
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
        // if (playerController.IsCrouching) direction.y += .5f;
        // else direction.y += 1f;
        float angle = Vector3.Angle(direction, npc.transform.forward);

        if(direction.magnitude < visDist && angle < visAngle)
        {
            RaycastHit hit;
            if (Physics.Raycast(npc.transform.position, direction, out hit, 999f))
            {
                // Debug.Log(hit.transform.name);
                // Debug.DrawRay(npc.transform.position, direction, Color.red, 2f);
                if(hit.transform.tag.Equals("Player"))
                {
                    // Debug.Log("I see you");
                    return true;
                } else
                {
                    return false;
                }
            }
        }

        return false;
    }
    public bool CanAttackPlayer()
    {
        if(agent.enabled)
        {
            Vector3 direction = player.position - npc.transform.position;
            float angle = Vector3.Angle(direction, npc.transform.forward);
            if (direction.magnitude < attackDist && angle < visAngle)
            {
                return true;
            }
        }
        return false;
    }
    public void WalkTowardsPlayer()
    {
        if(agent.enabled)
        {
            agent.SetDestination(player.position);
            if (agent.hasPath && stateName != STATE.ATTACK)
            {
                if (CanAttackPlayer())
                {
                    nextState = new Attack(npc, player, agent, waypoints, animator);
                    eventName = EVENT.EXIT;
                }
                else if (!CanSeePlayer())
                {
                    nextState = new Patrol(npc, player, agent, waypoints, animator);
                    eventName = EVENT.EXIT;
                }
            }
        }
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
    public void ShootWithCooldown()
    {
        if (stateName == STATE.ATTACK)
        {
            if (attackTimer < attackCooldown)
            {
                attackTimer += Time.deltaTime;
            }
            else
            {
                npc.GetComponent<AI>().ShootAtPlayer();
                attackTimer = 0f;
            }
        }
    }
}
public class Idle : State
{
    public Idle(GameObject _npc, Transform _player, NavMeshAgent _agent, GameObject[] waypoints, Animator anim)
        : base(_npc, _player, _agent, waypoints, anim)
    {
        stateName = STATE.IDLE;
        anim.SetBool("IsPatrolling", false);
        anim.SetBool("IsPursuing", false);
        // Debug.Log("State idle for " + _npc.name);
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        if(CanSeePlayer())
        {
            nextState = new Pursue(npc, player, agent, waypoints, animator);
            eventName = EVENT.EXIT;
        }
        else if(Random.Range(0, 100) < 10)
        {
            nextState = new Patrol(npc, player, agent, waypoints, animator);
            eventName = EVENT.EXIT;
        }
    }
}
public class Patrol : State
{
    int currentIndex = -1;
    public Patrol(GameObject _npc, Transform _player, NavMeshAgent _agent, GameObject[] waypoints, Animator anim)
        : base(_npc, _player, _agent, waypoints, anim)
    {
        stateName = STATE.PATROL;
        agent.speed = patrolSpeed;
        agent.isStopped = false;
        anim.SetBool("IsPatrolling", true);
        anim.SetBool("IsPursuing", false);
        anim.SetLayerWeight(1, 0);
        // Debug.Log("State patrol for " + _npc.name);
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
            nextState = new Pursue(npc, player, agent, waypoints, animator);
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
    public Pursue(GameObject _npc, Transform _player, NavMeshAgent _agent, GameObject[] waypoints, Animator anim)
        : base(_npc, _player, _agent, waypoints, anim)
    {
        stateName = STATE.PURSUIT;
        agent.speed = pursueSpeed;
        agent.isStopped = false;
        anim.SetBool("IsPatrolling", false);
        anim.SetBool("IsPursuing", true);
        anim.SetLayerWeight(1, 0);
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        WalkTowardsPlayer();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
public class Attack : State
{
    float rotationSpeed = 2.0f;

    public Attack(GameObject _npc, Transform _player, NavMeshAgent _agent, GameObject[] waypoints, Animator anim)
        : base(_npc, _player, _agent, waypoints, anim)
    {
        stateName = STATE.ATTACK;
        anim.SetLayerWeight(1, 1);
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

        ShootWithCooldown();

        if (!CanAttackPlayer())
        {
            nextState = new Idle(npc, player, agent, waypoints, animator);
            eventName = EVENT.EXIT;
        }
    }
}