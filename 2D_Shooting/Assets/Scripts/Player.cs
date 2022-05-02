using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float speed = 10.0f;
    Vector2 inputDirection = Vector2.zero;

    private void Update()
    {
        transform.Translate(inputDirection * speed * Time.deltaTime);
    }

    public void MoveInput(InputAction.CallbackContext context)
    {
        inputDirection = context.ReadValue<Vector2>();
    }
}
