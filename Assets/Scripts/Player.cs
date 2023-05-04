using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    [SerializeField] private GameInput _gameInput;
    [SerializeField] private float _speed = 7f;

    private float _turnSmoothTime = 0.1f;
    private float _turnSmoothVelocity;
    private float _playerCastRadius = 1f;
    private int _nonPlayerLayerMask = 7;

    private bool _isWalking;
    private bool _isSwordAttacking = false;
    private Vector3 _lastInteractDir;

    private Entity _entity;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _gameInput.OnSwordAttackAction += GameInput_OnSwordAttackAction;
        _gameInput.OnInteractAction += GameInput_OnInteractAction;
        _entity = GetComponent<Entity>();
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

        HandleCollision(); // stop it once we hit at least 1 object
    }

    private void HandleMovement()
    {
        Vector2 inputVector = _gameInput.GetMovementVectorNormalized();

        if (inputVector.magnitude == 0)
        {
            _isWalking = false;
            return; 
        }

        Vector3 direction = new Vector3(inputVector.x, 0, inputVector.y).normalized;
        LookAt(direction);
        transform.Translate(Vector3.forward * _speed * Time.deltaTime);
        _isWalking = true;
        
    }

    public void PerformAttack()
    {
        Vector2 inputVector = _gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if (moveDir != Vector3.zero)
        {
            _lastInteractDir = moveDir;
        }

        float interactDistance = 2f;

        Debug.DrawRay(new Vector3(transform.position.x, 3f, transform.position.z), transform.forward, Color.green, 1);

        if (Physics.Raycast(new Vector3(transform.position.x, 3f, transform.position.z), transform.forward, out RaycastHit raycastHit, interactDistance))
        {
            if(raycastHit.transform.TryGetComponent(out Entity entity))
            {
                Debug.Log("we hit an entity with name=" + entity.GetName());
                _entity.DealDamage(entity);
            }
            else
            {
                Debug.Log("not an entity");
            }
        }
        else
        {
            Debug.Log("didnt hit nun");
        }
    }

    private void LookAt(Vector3 dir)
    {
        float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, _turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }

    private void HandleCollision()
    {
        Collider[] hitColliders = GetCollidingObjectsWithPlayer();
        if (hitColliders != null)
        {
            foreach (Collider hitCollider in hitColliders)
            {
                if (hitCollider.TryGetComponent(out Entity entity))
                {
                    //Debug.Log("we hit an entity with name =" + entity.GetName());
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
    private Collider[] GetCollidingObjectsWithPlayer()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position + new Vector3(0, 1, 0), _playerCastRadius, _nonPlayerLayerMask);

        return hitColliders;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position + new Vector3(0, 1, 0), _playerCastRadius);
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
}
