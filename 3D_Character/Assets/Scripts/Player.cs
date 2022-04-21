using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveMode : byte
{
    WALK = 0,
    RUN
}

public class Player : MonoBehaviour, IControllable
{
    public float walkSpeed = 3.0f;
    public float runSpeed = 6.0f;
    public float turnSpeed = 0.3f;
    public GameObject weapone = null;
    public GameObject shield = null;

    private Animator anim = null;
    private CharacterController controller = null;
    private Vector3 inputDir = Vector2.zero;
    private Quaternion targetRotation = Quaternion.identity;
    private MoveMode moveMode = MoveMode.RUN;

    //public float waitTime = 5.0f;

    //void Update()
    //{
    //    waitTime -= Time.deltaTime;
    //    if(waitTime < 0)
    //    {
    //        anim.SetInteger("IdleSelect", Random.Range(1, 5));
    //        waitTime = 5.0f;
    //        AnimationClip[] clip = anim.runtimeAnimatorController.animationClips;
    //        float a = clip[0].length;
    //    }
    //}

    public void ControllerConnect()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    public void MoveInput(Vector2 dir)
    {
        // dir.x : a(-1) d(+1)
        // dir.y : s(-1) w(+1)
        inputDir.x = dir.x;
        inputDir.y = 0;
        inputDir.z = dir.y;
        inputDir.Normalize();

        if (inputDir.sqrMagnitude > 0.0f)
        {
            inputDir = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0) * inputDir;            
            targetRotation = Quaternion.LookRotation(inputDir); 
        }

    }

    public void MoveUpdate()
    {
        if (inputDir.magnitude > 0.0f)
        {
            float speed = 1.0f;
            if (moveMode == MoveMode.WALK)
            {
                anim.SetFloat("Speed", 0.5f);
                speed = walkSpeed;
            }
            else if (moveMode == MoveMode.RUN)
            {
                anim.SetFloat("Speed", 1.0f);
                speed = runSpeed;
            }
            else
            {
                anim.SetFloat("Speed", 0.0f);
            }
            
            controller.SimpleMove(inputDir * speed);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }
        else
        {
            anim.SetFloat("Speed", 0.0f);
        }        
    }

    public void MoveModeChange()
    {
        if( moveMode == MoveMode.WALK )
        {
            moveMode = MoveMode.RUN;
        }
        else
        {
            moveMode = MoveMode.WALK;
        }
    }

    public void ArmsEquip(bool equip)
    {
        // equip이 true면 무기와 방패가 보인다. false 보이지 않는다.
        weapone.SetActive(equip);
        shield.SetActive(equip);
    }

}
