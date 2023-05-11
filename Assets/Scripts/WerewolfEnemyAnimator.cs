using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WerewolfEnemyAnimator : MonoBehaviour
{
    private const string IS_WALKING = "IsWalking";
    private const string IS_SWORD_ATTACKING = "IsSwordAttacking";

    [SerializeField] private WerewolfEnemy _werewolfEnemy;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _animator.SetBool(IS_WALKING, _werewolfEnemy.IsWalking());
        _animator.SetBool(IS_SWORD_ATTACKING, _werewolfEnemy.IsSwordAttacking());
    }
}
