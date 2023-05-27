using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EntityAI : MonoBehaviour
{
    public enum State
    {
         Idle,
         Patrolling,
         Fleeing,
         Attacking,
    }

    private State _state;

    private NavMeshAgent _navMeshAgent;
    private Vector3 _position;
    private Entity _entity;

    private float _waitTime;
    private float _currentWaitTime;
    private float _fleeDistanceToPlayer = 5f;
    private float _attackVisabilityDistanceToPlayer = 10f;


    private void Awake()
    {
        _state = State.Idle;

        _entity = GetComponent<Entity>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _position = transform.position + new Vector3(Random.Range(0f, 5f), transform.position.y , Random.Range(0f, 5f));

        _currentWaitTime = Time.time;
        _waitTime = Time.time + Random.Range(0f, 600f);
    }

    private void Update()
    {
        switch (_state)
        {
            case State.Idle:
                Idle();
                break;
            case State.Patrolling:
                Patrol();
                break;
            case State.Fleeing:
                Fleeing();
                break;
            case State.Attacking:
                Attacking();
                break;
            default:
                Debug.LogError("state " + _state + " was unexpected");
                break;
        }
    }

    private void Idle()
    {
        _entity.IsWalking = false;
        if ((!_entity.IsHostile()) && ShouldFlee())
        {
            return;
        }

        if (_entity.IsHostile() && ShouldAttack())
        {
            return;
        }

        if (_currentWaitTime >= _waitTime)
        {
            _state = State.Patrolling;
            _navMeshAgent.speed = _entity.GetWalkSpeed();
            _position = transform.position + new Vector3(Random.Range(-10f, 10f), transform.position.y, Random.Range(-10f, 10f));
        }
        else
        {
            _currentWaitTime++;
        }
    }

    private void Patrol()
    {
        if ((!_entity.IsHostile()) && ShouldFlee())
        {
            return;
        }

        if (_entity.IsHostile() && ShouldAttack())
        {
            return;
        }

        // TODO: check if position is reachable or not

        MoveNavmeshAgent();
    }

    private void Fleeing()
    {
        if(Vector3.Distance(Player.Instance.transform.position, transform.position) > _fleeDistanceToPlayer)
        {
            _state = State.Idle;
            return;
        }
        else
        {
            MoveNavmeshAgent();
        } 
    }

    private void Attacking()
    {
        if (!ShouldAttack())
        {
            _state = State.Idle;
            return;
        }
        else
        {
            if ((Vector3.Distance(Player.Instance.transform.position, transform.position) < _entity.GetAttackDistanceToPlayer()))
            {
                
                if(_currentWaitTime >= (Time.time + _entity.GetAttackSpeed()))
                {
                    //Debug.Log(_entity.name + " is attacking the player");
                    _entity.IsWalking = false;
                    _entity.IsSwordAttacking = true;
                    //_entity.DealDamage(Player.Instance.gameObject.GetComponent<Entity>());
                    _currentWaitTime = Time.time;
                }
                else
                {
                    _entity.IsSwordAttacking = false;
                    _currentWaitTime++;
                }
            }
            else
            {
                _entity.IsSwordAttacking = false;
                _entity.IsWalking = true;
                _navMeshAgent.destination = Player.Instance.transform.position;
            }
        }
    }

    private bool ShouldFlee()
    {
        if ((Vector3.Distance(Player.Instance.transform.position, transform.position) < _fleeDistanceToPlayer))
        {
            _state = State.Fleeing;
            _position = transform.position + new Vector3(Random.Range(-20f, 20f), transform.position.y, Random.Range(-20f, 20f));
            _navMeshAgent.speed = _entity.GetRunSpeed();
            return true;
        }
        return false;
    }

    private bool ShouldAttack()
    {
        if ((Vector3.Distance(Player.Instance.transform.position, transform.position) < _attackVisabilityDistanceToPlayer))
        {
            _state = State.Attacking;
            _position = Player.Instance.transform.position;
            _currentWaitTime = (Time.time + _entity.GetAttackSpeed());
            _navMeshAgent.speed = _entity.GetRunSpeed();
            return true;
        }
        return false;
    }

    private void MoveNavmeshAgent()
    {
        
        if (transform.position.x <= (_position.x + _navMeshAgent.stoppingDistance) && transform.position.z <= (_position.z + _navMeshAgent.stoppingDistance))
        {
            _state = State.Idle;
            _currentWaitTime = Time.time;
            _waitTime = Time.time + Random.Range(0f, 600f);
        }
        else
        {
            _entity.IsWalking = true;
            _navMeshAgent.destination = _position;
        }
    }

    public State GetState()
    {
        return _state;
    }
}
