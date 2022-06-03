using System;
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
        actions.Player.Look.performed += OnLookInput;
        actions.Player.FireNormal.performed += OnFireNormalInput;
        actions.Player.FireSpecial.performed += OnFireSpecialInput;
        actions.Player.ShortCut01.performed += OnShortCut01;
        actions.Player.ShortCut02.performed += OnShortCut02;

        SetTarget(GameManager.Inst.MainPlayer as IControllable);
    }    

    private void OnDisable()
    {
        actions.Player.Move.performed -= OnMoveInput;
        actions.Player.Move.canceled -= OnMoveInput;
        actions.Player.Look.performed -= OnLookInput;
        actions.Player.FireNormal.performed -= OnFireNormalInput;
        actions.Player.FireSpecial.performed -= OnFireSpecialInput;
        actions.Player.ShortCut01.performed -= OnShortCut01;
        actions.Player.ShortCut02.performed -= OnShortCut02;
        actions.Player.Disable();
    }

    public void SetTarget(IControllable controllTarget)
    {
        target = controllTarget;
    }

    private void OnMoveInput(InputAction.CallbackContext context)
    {
        //Debug.Log(context.ReadValue<Vector2>());
        target.KeyboardInputDir = context.ReadValue<Vector2>();
    }

    private void OnLookInput(InputAction.CallbackContext context)
    {
        target.MouseInputPosition = context.ReadValue<Vector2>();
    }

    private void OnFireNormalInput(InputAction.CallbackContext context)
    {
        target.onFireNormal?.Invoke();
    }

    private void OnFireSpecialInput(InputAction.CallbackContext context)
    {
        target.onFireSpecial?.Invoke();
    }

    private void OnShortCut01(InputAction.CallbackContext context)
    {
        target.onShortCut01?.Invoke();
    }

    private void OnShortCut02(InputAction.CallbackContext context)
    {
        target.onShortCut02?.Invoke();
    }
}
