using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float speed = 10.0f;
    Vector2 inputDirection = Vector2.zero;
    
    //애니메이터 관련
    Animator anim = null;
    readonly int hashState = Animator.StringToHash("State");

    private void Awake()
    {
        anim = GetComponent<Animator>();        
    }

    private void Update()
    {
        transform.Translate(inputDirection * speed * Time.deltaTime);
    }

    public void MoveInput(InputAction.CallbackContext context)
    {
        inputDirection = context.ReadValue<Vector2>();
        //anim.SetFloat("InputY", inputDirection.y);
        if (inputDirection.y > 0.0f)
        {
            anim.SetInteger(hashState, 1);
        }
        else if (inputDirection.y < 0.0f)
        {
            anim.SetInteger(hashState, 2);
        }
        else
        {
            anim.SetInteger(hashState, 0);
        }
    }
}
