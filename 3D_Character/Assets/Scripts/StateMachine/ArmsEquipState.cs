using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmsEquipState : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Debug.Log($"Enter : {stateInfo.shortNameHash}");
        GameManager.Inst.MainPlayer.ArmsEquip(true);
    }
}