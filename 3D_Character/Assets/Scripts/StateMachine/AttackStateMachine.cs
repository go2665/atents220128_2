using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackStateMachine : StateMachineBehaviour
{
    public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        GameManager.Inst.MainPlayer.IsAttack = true;
        Debug.Log("Enter");
    }

    public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        GameManager.Inst.MainPlayer.IsAttack = false;
        animator.ResetTrigger("Attack");
        Debug.Log("Eixt");
    }

}
