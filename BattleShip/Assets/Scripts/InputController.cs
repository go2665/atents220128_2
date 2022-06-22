using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    /// <summary>
    /// 인풋 시스템 액션 맵
    /// </summary>
    InputActionMaps inputActions = null;
    public BattleField targetField = null;

    private void Awake()
    {
        inputActions = new InputActionMaps();
    }

    private void OnEnable()
    {
        inputActions.BattleField.Enable();
        inputActions.BattleField.Click.performed += OnClick;
        inputActions.BattleField.MouseMove.performed += OnMouseMove;
        inputActions.BattleField.MouseWheel.performed += OnMouseWheel;
    }

    private void OnDisable()
    {
        inputActions.BattleField.MouseWheel.performed -= OnMouseWheel;
        inputActions.BattleField.MouseMove.performed -= OnMouseMove;
        inputActions.BattleField.Click.performed -= OnClick;
        inputActions.BattleField.Disable();
    }

    private void OnMouseWheel(InputAction.CallbackContext context)
    {
        targetField.OnMouseWheel(context.ReadValue<float>());
    }

    private void OnClick(InputAction.CallbackContext context)
    {
        targetField.OnClick(Mouse.current.position.ReadValue());
    }

    private void OnMouseMove(InputAction.CallbackContext context)
    {
        targetField.OnMouseMove(context.ReadValue<Vector2>());
    }
}
