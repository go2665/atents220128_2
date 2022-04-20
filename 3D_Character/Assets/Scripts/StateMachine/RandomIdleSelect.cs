using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomIdleSelect : StateMachineBehaviour
{
    public float maxIdleInterval = 5.0f;
    public float minIdleInterval = 2.0f;
    private float waitTime = 5.0f;
    //public float normTime = 0.0f;
    private bool isSelect = false;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        waitTime = Random.Range(minIdleInterval, maxIdleInterval);
        animator.SetInteger("IdleSelect", 0);
        isSelect = false;
        //Test();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {        
        //normTime = stateInfo.normalizedTime;
        if (stateInfo.normalizedTime > waitTime && !isSelect && !animator.IsInTransition(0))
        {
            animator.SetInteger("IdleSelect", IdleSelect());
        }
    }

    int IdleSelect()
    {
        isSelect = true;

        float ran = Random.Range(0.0f, 1.0f);
        int index = 0;

        if( ran < 0.5f )
        {
            index = 1;
        }
        else if( ran < 0.7f )
        {
            index = 2;
        }
        else if (ran < 0.8f)
        {
            index = 3;
        }
        else
        {
            index = 4;
        }

        return index;
    }

    //void Test()
    //{
    //    int[] result = new int[5];
    //    for( int i=0;i<100000;i++)
    //    {
    //        result[IdleSelect()]++;
    //    }

    //    Debug.Log($"result : {result[0]}, {result[1]}, {result[2]}, {result[3]}, {result[4]}");
    //}
}
