using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour
{
    private PlayerTankActions actions = null;

    private void Awake()
    {
        actions = new PlayerTankActions();
    }

    private void OnEnable()
    {
        actions.Player.Enable();
        actions.Player.Move.started += OnMoveInput;
        actions.Player.Move.canceled += OnMoveInput;
    }

    private void OnDisable()
    {
        actions.Player.Move.started -= OnMoveInput;
        actions.Player.Move.canceled -= OnMoveInput;
        actions.Player.Disable();
    }

    void OnMoveInput(InputAction.CallbackContext context)
    {
        Debug.Log(context.ReadValue<Vector2>());
    }
}
