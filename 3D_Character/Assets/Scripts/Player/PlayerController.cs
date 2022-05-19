using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public GameObject targetObject = null;
    IControllable targetControl = null;

    private void Start()
    {
        SetTarget(targetObject);
    }

    public void SetTarget(GameObject _newTarget)
    {
        if (_newTarget != null)
        {
            targetObject = _newTarget;
            targetControl = targetObject.GetComponent<IControllable>();
            if (targetControl != null)
            {
                targetControl.ControllerConnect();
            }
            else
            {
                targetObject = null;
            }
        }
        else
        {
            targetObject = null;
            targetControl = null;
        }
    }

    private void OnValidate()
    {
        SetTarget(targetObject);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        targetControl?.MoveInput(context.ReadValue<Vector2>());
    }

    private void Update()
    {        
        targetControl?.MoveUpdate();        
    }

    public void OnMoveModeChange(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //Debug.Log("Shift");
            targetControl?.MoveModeChange();
        }
    }

    public void OnAttackInput(InputAction.CallbackContext context)
    {
        if( context.started )
        {
            targetControl?.AttackInput();
        }
    }

    public void OnLockOn(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            targetControl?.LockOnInput();
        }
    }

    public void OnPickup(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            targetControl?.PickupInput();
        }
    }

    public void OnInventoryOnOff(InputAction.CallbackContext context)
    {
        if( context.started )
        {
            targetControl?.InventoryOnOffInput();
        }
    }
}
