using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboFinish : StateMachineBehaviour
{
    private ParticleSystem ps = null;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (ps == null)
        {
            ps = GameManager.Inst.MainPlayer.weapone.GetComponentInChildren<ParticleSystem>();
        }
        ps.Play();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(stateInfo.normalizedTime > 0.5f)
        {
            ps.Clear();
            ps.Stop();
        }
    }

    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    ps.Clear();
    //    ps.Stop();
    //}
}
