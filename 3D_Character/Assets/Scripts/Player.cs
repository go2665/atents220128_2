using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IControllable
{
    public float moveSpeed = 3.0f;

    private Animator anim = null;
    private CharacterController controller = null;
    private Vector2 inputDir = Vector2.zero;

    public void ControllerConnect()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    public void MoveInput(Vector2 dir)
    {
        inputDir = dir;
    }

    public void MoveUpdate()
    {
        anim.SetFloat("Speed",inputDir.y);
        //CollisionFlags flags = controller.Move(transform.forward * inputDir.y * Time.deltaTime * moveSpeed);
        controller.SimpleMove(transform.forward * inputDir.y * moveSpeed);
        //Debug.Log(isGround);
    }
}
