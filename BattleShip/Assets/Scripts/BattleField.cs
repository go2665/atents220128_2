using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BattleField : MonoBehaviour
{
    // 상수들 --------------------------------------------------------------------------------------
    /// <summary>
    /// 필드 한변의 크기.(필드는 항상 정사각형)
    /// </summary>
    public const int FieldSize = 10;

    /// <summary>
    /// 한 필드에 있을 수 있는 배의 수
    /// </summary>
    const int ShipCount = 5;

    // enum ---------------------------------------------------------------------------------------
    /// <summary>
    /// 필드 한칸에 배가 있는지 포탄이 있는지에 대한 정보를 표시할 enum
    /// </summary>    
    public enum FieldExists : byte
    {
        None = 0,
        Ship = 1,
        CannonBall = 2
    }

    // 구조체 --------------------------------------------------------------------------------------
    /// <summary>
    /// 필드 한칸에 대한 정보를 저장하는 구조체
    /// </summary>
    public struct FieldCell
    {
        /// <summary>
        /// 필드 한칸에 배가 있는지 포탄이 있는지에 대한 정보를 가짐
        /// </summary>
        public FieldExists exists;

        /// <summary>
        /// 필드 한칸에 어떤 배가 있는지에 대한 정보
        /// </summary>
        public Ship ship;
    }

    // 주요 변수 -----------------------------------------------------------------------------------
    /// <summary>
    /// 필드. 특정 좌표에 배치된 오브젝트 정보 기록(확인용)
    /// </summary>
    FieldCell[,] field = new FieldCell[FieldSize, FieldSize];

    /// <summary>
    /// 필드에 배치된 가라앉지 않은 배의 수
    /// </summary>
    int aliveShipCount = 0;

    /// <summary>
    /// 그래픽 출력용 배와 포탄
    /// </summary>
    Ship[] ships = new Ship[ShipCount];
    Ship selectedShip = null;
    Vector2Int oldMouseCoord = -Vector2Int.one;
    List<Vector2Int> cannonballPosition = new List<Vector2Int>(FieldSize* FieldSize);

    /// <summary>
    /// 인풋 시스템 액션 맵
    /// </summary>
    InputActionMaps inputActions = null; 


    // 프로퍼티 ------------------------------------------------------------------------------------
    /// <summary>
    /// 필드의 배가 모두 침몰되었는지 여부. true면 모든 배가 침몰되었다.
    /// </summary>
    public bool IsDepeat
    {
        get => (aliveShipCount <= 0);
    }    
    

    // 함수들 --------------------------------------------------------------------------------------
    /// <summary>
    /// 게임이 시작될때 실행될 초기화 함수
    /// </summary>
    void Initialize()
    {
        aliveShipCount = ShipCount;
        for(int i=0;i<ShipCount;i++)
        {
            ships[i] = GameManager.Inst.MakeShip((ShipType)i);
            ships[i].gameObject.transform.parent = transform;
            ships[i].gameObject.SetActive(false);
        }

        ShipDiploymentMode(false);
    }

    /// <summary>
    /// 해당 위치에 배가 있는지 확인
    /// </summary>
    /// <param name="pos">확인할 위치</param>
    /// <returns>배가 있으면 true, 없으면 false</returns>
    public bool IsShipThere(Vector2Int pos)
    {
        return ((field[pos.x, pos.y].exists & FieldExists.Ship) != 0);
    }

    /// <summary>
    /// 배를 배치 가능한지 확인하는 함수
    /// </summary>
    /// <param name="pos">배치할 위치</param>
    /// <param name="ship">배치할 배</param>
    /// <param name="positions">배치 가능할 경우 배치되는 좌표들을 기록해 놓을 곳.(out)</param>
    /// <returns>true면 배치가능, false면 배치불가</returns>
    bool IsShipDeployment(Vector2Int pos, Ship ship, out Vector2Int[] positions)
    {
        positions = new Vector2Int[ship.size];      // 배 크기에 맞게 할당

        int index = (int)ship.Direction;
        Vector2Int[] temp = { new Vector2Int(-1, 0), new Vector2Int(0, -1), new Vector2Int(1, 0), new Vector2Int(0, 1) };
        for ( int i=0 ; i<ship.size ; i++ )
        {
            positions[i] = pos + temp[index] * i;   // 배가 배치될 좌표들 기록
        }

        bool result = true;
        for (int i = 0; i < ship.size; i++)
        {
            // 밖으로 벗어나는 위치가 있는지 확인
            // 같은 위치에 배가 있는지 확인
            if( !IsValidPosition(positions[i]) || IsShipThere(positions[i]))
            {
                //한 칸이라도 밖으로 벗어나거나 다른배와 겹칠 경우 안됨.
                Debug.Log($"{ship.gameObject.name}을 ({pos.x}, {pos.y})에 배치할 수 없습니다.");
                result = false;
                break;
            }
        }        
        return result;
    }

    /// <summary>
    /// 함선 배치. 배치할 수 없을 경우 실행 안함.
    /// </summary>
    /// <param name="pos">배치할 위치</param>
    /// <param name="ship">배치할 배</param>
    public void ShipDeployment(Vector2Int pos, Ship ship)
    {
        Vector2Int[] positions = null;
        if ( IsShipDeployment(pos, ship, out positions) )
        {
            foreach(Vector2Int position in positions)
            {
                field[position.x, position.y].exists = FieldExists.Ship;
                field[position.x, position.y].ship = ship;
            }
            ship.Position = pos;
            //Debug.Log($"{ship.gameObject.name}을 ({pos.x}, {pos.y})에 배치했습니다.");
        }
    }

    /// <summary>
    /// 위치가 필드 범위 안인지 체크하는 함수
    /// </summary>
    /// <param name="pos">체크할 위치</param>
    /// <returns>true면 필드 안의 위치, false면 필드 밖의 위치</returns>
    public bool IsValidPosition(Vector2Int pos)
    {
        return (-1 < pos.x && pos.x < FieldSize && -1 < pos.y && pos.y < FieldSize);
    }

    /// <summary>
    /// 공격 가능한 위치인지 확인
    /// </summary>
    /// <param name="pos">공격할 위치</param>
    /// <returns>공격가능하면 true, 아니면 false</returns>
    public bool IsAttackable(Vector2Int pos)
    {
        return (field[pos.x, pos.y].exists != FieldExists.CannonBall);
    }

    /// <summary>
    /// 이 필드가 공격을 받음처리
    /// </summary>
    /// <param name="pos">공격받은 위치</param>
    public void Attacked(Vector2Int pos)
    {
        if (IsValidPosition(pos))   // 적절한 위치여야 한다.
        {
            if (IsAttackable(pos))  // 공격 가능한 위치여야 한다.(이미 공격하지 않은 곳)
            {
                if(field[pos.x, pos.y].exists == FieldExists.Ship)  
                {
                    // 배를 공격했으면
                    Debug.Log($"{gameObject.name} : 배({pos.x},{pos.y})가 공격받았습니다.");
                    field[pos.x, pos.y].ship.Hit();             // 배에 데미지를 주고
                    if (!field[pos.x, pos.y].ship.IsSinking)    // 배가 가라앉았는지를 확인
                    {
                        aliveShipCount--;                       // 생존해 있는 배 수 감소
                    }
                }
                else
                {
                    Debug.Log($"{gameObject.name} : 바다({pos.x},{pos.y})가 공격받았습니다.");
                }
                field[pos.x, pos.y].exists = FieldExists.CannonBall;    // 공격 받았다고 표시
            }
        }
    }


    // 유니티 이벤트 함수 --------------------------------------------------------------------------
    private void Awake()
    {
        inputActions = new InputActionMaps();
    }

    private void Start()
    {
        Initialize();
    }

    private void OnEnable()
    {
        inputActions.BattleField.Enable();
        inputActions.BattleField.Click.performed += OnBattleFieldClick;
        inputActions.BattleField.MouseMove.performed += OnBattleFieldMouseMove;
        inputActions.BattleField.MouseWheel.performed += OnBattleFieldMouseWheel;
    }
    
    private void OnDisable()
    {
        inputActions.BattleField.MouseWheel.performed -= OnBattleFieldMouseWheel;
        inputActions.BattleField.MouseMove.performed -= OnBattleFieldMouseMove;
        inputActions.BattleField.Click.performed -= OnBattleFieldClick;
        inputActions.BattleField.Disable();
    }

    private void OnBattleFieldMouseWheel(InputAction.CallbackContext context)
    {
        float whellDelta = context.ReadValue<float>();
        //Debug.Log($"Wheel : {whellDelta}"); 
        if (selectedShip != null)
        {
            selectedShip.Rotate(whellDelta < 0.0f);
            Vector2Int[] temp = new Vector2Int[selectedShip.size];
            bool deployable = IsShipDeployment(oldMouseCoord, selectedShip, out temp);
            selectedShip.SetMaterial(deployable);
        }
    }

    private void OnBattleFieldClick(InputAction.CallbackContext context)
    {
        Vector2 pos = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(pos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100.0f, LayerMask.GetMask("BattleField"))
            && hit.collider.gameObject == this.gameObject )
        {
            //    //Debug.Log($"Hit : {hit.point}");
            //    //Debug.Log($"Local origin : {transform.position}");
            //    //Debug.Log($"Diff :{hit.point - transform.position}");

            //    Vector3 diff = hit.point - transform.position;
            //    Vector2Int coord = new Vector2Int((int)diff.x, (int)-diff.z);
            //    Debug.Log($"2D coord : {coord}");
            ShipDeployment(oldMouseCoord, selectedShip);
            ShipDiploymentMode(false);
        }        
    }

    

    private void OnBattleFieldMouseMove(InputAction.CallbackContext context)
    {
        Vector2 pos = context.ReadValue<Vector2>();
        Ray ray = Camera.main.ScreenPointToRay(pos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100.0f, LayerMask.GetMask("BattleField"))
            && hit.collider.gameObject == this.gameObject)
        {
            Vector3 diff = hit.point - transform.position;
            Vector2Int coord = new Vector2Int((int)diff.x, (int)-diff.z);
            if (coord != oldMouseCoord)
            {
                //Debug.Log($"2D coord : {coord}");
                selectedShip.transform.position = transform.position + new Vector3(coord.x + 0.5f, 0.0f, -coord.y - 0.5f);

                Vector2Int[] temp = new Vector2Int[selectedShip.size];
                bool deployable = IsShipDeployment(coord, selectedShip, out temp);
                //Debug.Log($"result : {success}");
                selectedShip.SetMaterial(deployable);

                oldMouseCoord = coord;
            }
        }
        else
        {
            Vector3 newPos = Camera.main.ScreenToWorldPoint(pos);
            selectedShip.transform.position = new Vector3(newPos.x, 0, newPos.z);
            selectedShip.SetMaterial(false);
        }
    }

    public void ShipDiploymentMode(bool on, ShipType shipType = ShipType.SizeOfShipType)
    {
        if(on)
        {            
            if (shipType != ShipType.SizeOfShipType)
            {
                Ship target = ships[(int)shipType];
                if (!target.IsDeployed)
                {
                    selectedShip = target;
                    selectedShip.gameObject.SetActive(true);

                    inputActions.BattleField.MouseMove.Enable();
                    inputActions.BattleField.MouseWheel.Enable();
                    inputActions.BattleField.Click.Enable();
                }
            }
        }
        else
        {
            inputActions.BattleField.Click.Disable();
            inputActions.BattleField.MouseWheel.Disable();
            inputActions.BattleField.MouseMove.Disable();
            oldMouseCoord = -Vector2Int.one;
            selectedShip = null;
        }
    }
}
