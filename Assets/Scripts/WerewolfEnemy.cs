using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WerewolfEnemy : MonoBehaviour
{
    private Entity _entity;

    private void Awake()
    {
        _entity = GetComponent<Entity>();
    }

    public bool IsWalking()
    {
        return _entity.IsWalking;
    }

    public bool IsSwordAttacking()
    {
        return _entity.IsSwordAttacking;
    }

}
