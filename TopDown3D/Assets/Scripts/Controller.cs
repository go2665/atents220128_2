using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour
{
    private IControllable target = null;

    private PlayerTankActions actions = null;

    private void Awake()
    {
        actions = new PlayerTankActions();
    }

    private void OnEnable()
    {
        actions.Player.Enable();
        actions.Player.Move.performed += OnMoveInput;
        actions.Player.Move.canceled += OnMoveInput;
        SetTarget(GameManager.Inst.MainPlayer as IControllable);
    }

    private void OnDisable()
    {
        actions.Player.Move.performed -= OnMoveInput;
        actions.Player.Move.canceled -= OnMoveInput;
        actions.Player.Disable();
    }

    public void SetTarget(IControllable controllTarget)
    {
        target = controllTarget;
    }

    void OnMoveInput(InputAction.CallbackContext context)
    {
        //Debug.Log(context.ReadValue<Vector2>());
        target.KeyboardInputDir = context.ReadValue<Vector2>();
    }

}
