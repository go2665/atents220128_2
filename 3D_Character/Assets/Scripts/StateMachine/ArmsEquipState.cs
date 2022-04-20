using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmsEquipState : StateMachineBehaviour
{
    Player player = null;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
    }

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Debug.Log($"Enter : {stateInfo.shortNameHash}");
        player.ArmsEquip(true);
    }
}
