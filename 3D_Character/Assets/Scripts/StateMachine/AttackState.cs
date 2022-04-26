using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : StateMachineBehaviour
{
    // 해결방법1--------------------------------------------------------------
    //public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    GameManager.Inst.MainPlayer.IsAttack = true;
    //}
    //public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    GameManager.Inst.MainPlayer.IsAttack = false;
    //    animator.ResetTrigger("Attack");
    //}
    //-----------------------------------------------------------------------

    public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        //Debug.Log("Enter");
        GameManager.Inst.MainPlayer.IsAttack = true;
    }

    public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        GameManager.Inst.MainPlayer.IsAttack = false;
        animator.ResetTrigger("Attack");
        //Debug.Log("Eixt");
    }

}
