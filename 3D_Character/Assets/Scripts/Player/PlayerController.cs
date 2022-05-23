using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// targetControl로 설정된 오브젝트를 움직이기 위해 만들어진 클래스
/// </summary>
public class PlayerController : MonoBehaviour
{
    public GameObject targetPlayer = null;
    public GameObject targetCar = null;
    private IControllable targetControl = null;
    private PlayerInput pi = null;              // PlayerInput 컴포넌트 캐싱용
    //private PlayerInputActions pia = null;      // Input Action Asset 파일을 클래스로 자동 생성한 것

    private void Awake()
    {
        pi = GetComponent<PlayerInput>();       // 찾고
    }

    private void OnEnable()
    {
        GameManager.Inst.InventoryUI.onInventoryOpen += ActionMapChange_ToUI;       // 엑션맵 변경하는 델리게이트 추가
        GameManager.Inst.InventoryUI.onInventoryClose += ActionMapChange_ToPlayer;
    }

    private void OnDisable()
    {
        GameManager.Inst.InventoryUI.onInventoryOpen -= ActionMapChange_ToUI;       // 델리게이트에서 추가했던 함수 삭제
        GameManager.Inst.InventoryUI.onInventoryClose -= ActionMapChange_ToPlayer;
    }

    void ActionMapChange_ToPlayer()
    {
        pi.SwitchCurrentActionMap("Player");        // PlayerInput 컴포넌트를 이용해 액션맵 스위치
    }

    void ActionMapChange_ToUI()
    {
        pi.SwitchCurrentActionMap("UI");            // PlayerInput 컴포넌트를 이용해 액션맵 스위치
    }

    private void Start()
    {
        SetTarget(targetPlayer);
    }

    public void SetTarget(GameObject _newTarget)
    {
        if (_newTarget != null)
        {
            targetPlayer = _newTarget;
            targetControl = _newTarget.GetComponent<IControllable>();
            if (targetControl != null)
            {
                targetControl.ControllerConnect();
            }
            else
            {
                targetPlayer = null;
            }
        }
        else
        {
            targetPlayer = null;
            targetControl = null;
        }
    }

    private void OnValidate()
    {
        SetTarget(targetPlayer);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        targetControl?.MoveInput(context.ReadValue<Vector2>());
    }

    private void Update()
    {
        if (targetControl?.UseRigidbody == false)
        {
            targetControl?.MoveUpdate();
        }
    }

    private void FixedUpdate()
    {
        if (targetControl?.UseRigidbody == true)
        {
            targetControl?.MoveUpdate();
        }
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

    public void OnGetIn(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            pi.SwitchCurrentActionMap("Car");
            SetTarget(targetCar);
        }
    }

    public void OnGetOff(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            pi.SwitchCurrentActionMap("Player");
            SetTarget(targetPlayer);
        }
    }
}
