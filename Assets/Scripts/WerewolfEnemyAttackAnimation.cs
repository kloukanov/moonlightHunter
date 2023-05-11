using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WerewolfEnemyAttackAnimation : EntityBaseState
{
    int currentAttackFrame;
    int desiredAttackFrame = 35;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Debug.Log("sword attack start"); 
        currentAttackFrame = 0;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        currentAttackFrame++;
        if (currentAttackFrame == desiredAttackFrame)
        {
            Debug.Log("attack!!");
            Entity entity = GetEntity(animator);

            if ((Vector3.Distance(Player.Instance.transform.position, entity.transform.position) < entity.GetAttackDistanceToPlayer()))
            {
                entity.DealDamage(Player.Instance.gameObject.GetComponent<Entity>());
            }    
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Entity entity = GetEntity(animator);
        //entity.SetSwordAttack(false);
    }
}
