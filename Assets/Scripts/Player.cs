using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    [SerializeField] private float _movementSpeed = 7f;
    [SerializeField] private Transform _attackPointTransform;
    [SerializeField] private GameObject _humanPrefab;
    [SerializeField] private GameObject _werewolfPrefab;

    public event EventHandler OnPlayerDead;

    private float _turnSmoothTime = 0.1f;
    private float _turnSmoothVelocity;
    private float _lootPlayerCastRadius = 1f;
    private float _attackPlayerCastRadius = 2f;
    private int _nonPlayerLayerMask = 7;

    private bool _isWalking;
    private bool _isSwordAttacking = false;
    private Vector3 _lootCollisionSphereOffset = new(0, 1, 0);

    private Entity _entity;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GameInput.Instance.OnSwordAttackAction += GameInput_OnSwordAttackAction;
        GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;
        _entity = GetComponent<Entity>();
        GameManager.Instance.OnTimerFinished += GameManager_OnTimerFinished;
    }

    private void GameManager_OnTimerFinished(object sender, EventArgs e)
    {
        if (_humanPrefab.activeInHierarchy)
        {
            TurnIntoWerewolf();
        }
        else
        {
            TurnIntoHuman();
        }
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        //empty for now
    }

    private void GameInput_OnSwordAttackAction(object sender, System.EventArgs e)
    {
        _isSwordAttacking = true;
        _isWalking = false;
        //PerformAttack();
        // show visual indicator of performed attack to the entity?
    }

    void Update()
    {
        if(!_isSwordAttacking)
            HandleMovement();

        HandleCollision(_lootCollisionSphereOffset, _lootPlayerCastRadius); // stop it once we hit at least 1 object?
    }

    private void HandleMovement()
    {
        Vector2 inputVector = GameInput.Instance.GetMovementVectorNormalized();

        if (inputVector.magnitude == 0)
        {
            _isWalking = false;
            return; 
        }

        Vector3 direction = new Vector3(inputVector.x, 0, inputVector.y).normalized;
        LookAt(direction);
        transform.Translate(Vector3.forward * _movementSpeed * Time.deltaTime);
        _isWalking = true;
        
    }

    public void PerformAttack()
    {
        //Debug.DrawRay(new Vector3(transform.position.x, 3f, transform.position.z), transform.forward, Color.green, 1);

        Collider[] hitColliders = GetCollidingObjectsWithTransform(_attackPointTransform, Vector3.zero, _attackPlayerCastRadius);
        if (hitColliders != null)
        {
            foreach (Collider hitCollider in hitColliders)
            {
                if (hitCollider.TryGetComponent(out Entity entity))
                {
                    Debug.Log("we hit an entity with name =" + entity.GetName());
                    _entity.DealDamage(entity);
                }
            }
        }
    }

    private void LookAt(Vector3 dir)
    {
        float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, _turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }

    private void HandleCollision(Vector3 sphereOffset, float radius)
    {
        Collider[] hitColliders = GetCollidingObjectsWithTransform(this.transform, sphereOffset, radius);
        if (hitColliders != null)
        {
            foreach (Collider hitCollider in hitColliders)
            {
                if (hitCollider.TryGetComponent(out Entity entity))
                {
                    //Debug.Log("we hit an entity with name =" + entity.GetName());

                    // show entity health bar when in proximity
                }
                else if (hitCollider.TryGetComponent(out CollectableObject collectableObject))
                {
                    //Debug.Log("we hit a collectableObject with name =" + collectableObject.GetCollectableObjectSO().objectName);
                    if(collectableObject.GetCollectableObjectSO().objectName == "Meat")
                    {
                        GameManager.Instance.AddFood();
                        collectableObject.DestorySelf();
                    }else if (collectableObject.GetCollectableObjectSO().objectName == "Bone")
                    {
                        GameManager.Instance.AddBones();
                        collectableObject.DestorySelf();
                    }
                }
            }
        }
    }
    private Collider[] GetCollidingObjectsWithTransform(Transform transformObj, Vector3 sphereOffset, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(transformObj.position + sphereOffset, radius, _nonPlayerLayerMask);

        return hitColliders;
    }


    private void TurnIntoWerewolf()
    {
        _humanPrefab.SetActive(false);
        _werewolfPrefab.SetActive(true);   
    }

    private void TurnIntoHuman()
    {
        _werewolfPrefab.SetActive(false);
        _humanPrefab.SetActive(true);
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        //Gizmos.DrawSphere(transform.position + new Vector3(0, 1, 0), _playerCastRadius);
        Gizmos.DrawSphere(_attackPointTransform.position, _attackPlayerCastRadius);
    }


    public bool IsWalking()
    {
        return _isWalking;
    }
    public bool IsSwordAttacking()
    {
        return _isSwordAttacking;
    }

    public void SetSwordAttack(bool value)
    {
        _isSwordAttacking = value;
    }

    public void SetPlayerToDead()
    {
        OnPlayerDead?.Invoke(this, EventArgs.Empty);
    }
}
