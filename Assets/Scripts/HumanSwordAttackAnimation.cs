using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanSwordAttackAnimation : PlayerStateBase
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
        //Debug.Log((int)(stateInfo.normalizedTime * 100)); possibly use stateInfo.normalizedTime for cleaner solution?

        currentAttackFrame++;
        if (currentAttackFrame == desiredAttackFrame)
        {
            Debug.Log("attack!!");
            Player player = GetPlayer(animator);
            player.PerformAttack();
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Player player = GetPlayer(animator);
        //player.PerformAttack();
        player.SetSwordAttack(false);
        //Debug.Log("sword attack end");
    }
}
