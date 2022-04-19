using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IControllable
{
    public float moveSpeed = 3.0f;
    public float turnSpeed = 0.3f;

    private Animator anim = null;
    private CharacterController controller = null;
    private Vector3 inputDir = Vector2.zero;
    private Quaternion targetRotation = Quaternion.identity;

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
        //Debug.Log(inputDir.sqrMagnitude);
        anim.SetFloat("Speed",inputDir.sqrMagnitude);
        controller.SimpleMove(inputDir * moveSpeed);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }

}
