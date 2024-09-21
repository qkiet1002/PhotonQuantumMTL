using Quantum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuffState : StateMachineBehaviour
{
    public float threashold = 0.3f;
    private bool TestHP = false;
    private PlayerView playerView;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (playerView == null)
        {
            playerView = animator.GetComponentInParent<PlayerView>();
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.normalizedTime < threashold)
        {
            TestHP = false;
        }
        else if (stateInfo.normalizedTime > threashold && TestHP == false)
        {
             Debug.Log("Hồi Máu");
            TestHP = true;
            playerView.TestHP = true;

        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
