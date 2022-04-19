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

}
