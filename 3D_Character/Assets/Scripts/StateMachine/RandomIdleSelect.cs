using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomIdleSelect : StateMachineBehaviour
{
    public float maxIdleInterval = 5.0f;
    public float minIdleInterval = 2.0f;
    public float waitTime = 5.0f;
    public float normTime = 0.0f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        waitTime = Random.Range(minIdleInterval, maxIdleInterval);
        animator.SetInteger("IdleSelect", 0);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        normTime = stateInfo.normalizedTime;
        if (stateInfo.normalizedTime > waitTime )
        {
            animator.SetInteger("IdleSelect", Random.Range(1, 5));
        }
    }
}
