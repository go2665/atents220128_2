using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public GameObject target = null;
    public float moveSpeed = 3.0f;

    private Animator anim = null;
    private CharacterController controller = null;

    private Vector3 inputDir = Vector3.zero;

    private void Start()
    {
        SetTarget(target);
    }

    public void SetTarget(GameObject _newTarget)
    {
        if (_newTarget != null)
        {
            target = _newTarget;
            anim = target.GetComponent<Animator>();
            controller = target.GetComponent<CharacterController>();
        }
        else
        {
            target = null;
            anim = null;
            controller = null;
        }
    }

    private void OnValidate()
    {
        SetTarget(target);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        inputDir = context.ReadValue<Vector2>();        
    }

    private void Update()
    {
        anim.SetFloat("Speed",inputDir.y);
        CollisionFlags flags = controller.Move(target.transform.forward * inputDir.y * Time.deltaTime * moveSpeed);
        //bool isGround = controller.SimpleMove(target.transform.forward * inputDir.y * moveSpeed);
        //Debug.Log(isGround);
    }

}
