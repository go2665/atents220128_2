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
    /// 필드에 배치될 배
    /// </summary>
    Ship[] ships = new Ship[ShipCount];

    /// <summary>
    /// 현재 배치중인 배
    /// </summary>
    Ship selectedShip = null;

    /// <summary>
    /// 이전 마우스 커서 위치(필드 그리드 기준)
    /// </summary>
    Vector2Int oldMouseCoord = -Vector2Int.one;

    /// <summary>
    /// 그래픽 출력용 포탄
    /// </summary>     
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
        aliveShipCount = ShipCount;     // 시작 배 대수 설정
        for(int i=0;i<ShipCount;i++)
        {
            ships[i] = GameManager.Inst.MakeShip((ShipType)i);  // 배 생성
            ships[i].gameObject.transform.parent = transform;   // 필드에 배 추가
            ships[i].gameObject.SetActive(false);               // 우선 배는 안보이게 해놓기
        }

        ShipDiploymentMode(false);      // 배 배치모드 아님
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
        positions = ShipPositions(ship, pos);   // 배가 존재하게 될 위치들 구함

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
    /// 배를 배치 가능한지 확인하는 함수(래핑함수)
    /// </summary>
    /// <param name="pos">배치할 위치</param>
    /// <param name="ship">배치할 배</param>
    /// <returns>true면 배치가능, false면 배치불가</returns>
    bool IsShipDeployment(Vector2Int pos, Ship ship)
    {
        Vector2Int[] positions;
        return IsShipDeployment(pos, ship, out positions);
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
            ship.transform.position = GridToWorld(pos.x, pos.y);    // 배를 해당 그리드의 위치로 이동
            ship.gameObject.SetActive(true);                        // 배가 보이게 설정
            ship.IsDeployed = true;
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

    /// <summary>
    /// 함선배치 모드인지 아닌지 설정
    /// </summary>
    /// <param name="on">true면 배치모드, false면 배치모드 아님</param>
    /// <param name="shipType">배치할 배의 종류 (배치모드일 때만 사용)</param>
    public void ShipDiploymentMode(bool on, ShipType shipType = ShipType.SizeOfShipType)
    {
        if (on)
        {
            // 배치모드로 설정하면
            if (shipType != ShipType.SizeOfShipType)
            {
                // 배의 종류가 적절한지 확인
                Ship target = ships[(int)shipType];
                if (!target.IsDeployed)
                {
                    // 해당 배가 이미 배치되지 않았을 때만 실행
                    selectedShip = target;
                    selectedShip.gameObject.SetActive(true);        // 대상을 배치중인 배로 설정하고 enable하기

                    inputActions.BattleField.MouseMove.Enable();    // 마우스 입력 전부 활성화
                    inputActions.BattleField.MouseWheel.Enable();
                    inputActions.BattleField.Click.Enable();
                }
            }
        }
        else
        {
            // 배치모드를 끌 경우
            inputActions.BattleField.Click.Disable();       // 마우스 입력 전부 비할성화
            inputActions.BattleField.MouseWheel.Disable();
            inputActions.BattleField.MouseMove.Disable();
            oldMouseCoord = -Vector2Int.one;                // oldMouseCoord를 불가능한 값으로 설정
            selectedShip = null;                            // 배치중인 배도 null로 설정
        }
    }

    /// <summary>
    /// 랜덤으로 남은 함선 배치
    /// </summary>
    public void RandomDeployment()
    {
        foreach(Ship ship in ships)
        {
            if (ship.IsDeployed)    // 이미 배치된 배는 스킵
                continue;

            int rotateCount = Random.Range(0, 4);   // 4방향 중 랜덤으로 하나 선택
            for (int i = 0; i < rotateCount; i++)
            {
                ship.Rotate();
            }

            bool result;
            Vector2Int pos;
            do
            {
                pos = new Vector2Int(Random.Range(0, FieldSize), Random.Range(0, FieldSize));
                result = IsShipDeployment(pos, ship);
            } while (!result);          // 배치 가능한 위치가 잡힐 때까지 랜덤 수행
            ShipDeployment(pos, ship);  // 실제로 배 배치
        }
    }

    Vector2Int[] ShipPositions(Ship ship, Vector2Int pos)
    {
        Vector2Int[] positions = new Vector2Int[ship.size];
        int index = (int)ship.Direction;
        Vector2Int[] temp = { new Vector2Int(-1, 0), new Vector2Int(0, -1), new Vector2Int(1, 0), new Vector2Int(0, 1) };
        for (int i = 0; i < ship.size; i++)
        {
            positions[i] = pos + temp[index] * i;   // 배가 배치될 좌표들 기록
        }

        return positions;
    }

    void Redeployment(Ship ship)
    {
        Vector2Int[] positions = ShipPositions(ship, ship.Position);
        foreach(var pos in positions)
        {
            field[pos.x, pos.y].exists = FieldExists.None;
            field[pos.x, pos.y].ship = null;
        }
        ship.Position = Vector2Int.zero;
        ship.IsDeployed = false;
        ship.transform.position = Vector3.zero;
        ship.gameObject.SetActive(false);
    }

    public void ResetDeployment()
    {
        foreach(var ship in ships)
        {
            Redeployment(ship);
        }
    }

    // 그리드 좌표를 월드좌표로 변경해주는 함수
    Vector3 GridToWorld(int x, int y)
    {
        return transform.position + new Vector3(x + 0.5f, 0.0f, -y - 0.5f);
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

    /// <summary>
    /// 마우스 휠을 움직였을 때 실행될 함수
    /// </summary>
    /// <param name="context"></param>
    private void OnBattleFieldMouseWheel(InputAction.CallbackContext context)
    {
        // 휠 움직임 받아오기
        if (selectedShip != null)
        {
            float whellDelta = context.ReadValue<float>();
            //Debug.Log($"Wheel : {whellDelta}"); 
            selectedShip.Rotate(whellDelta < 0.0f); // 받아온 값을 기준으로 회전(휠을 올리면 시계방향, 휠을 내리면 반시계밯향)            
            bool deployable = IsShipDeployment(oldMouseCoord, selectedShip);    // 배가 배치 가능한지 확인
            selectedShip.SetMaterial(deployable);   // 배치 가능 여부에 따라 머티리얼 변경
        }
    }

    /// <summary>
    /// 마우스를 클릭했을 때 실행될 함수
    /// </summary>
    /// <param name="context"></param>
    private void OnBattleFieldClick(InputAction.CallbackContext context)
    {
        Vector2 pos = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(pos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100.0f, LayerMask.GetMask("BattleField"))
            && hit.collider.gameObject == this.gameObject )
        {
            // 내 필드를 클릭했을 때만 동작
            ShipDeployment(oldMouseCoord, selectedShip);    // 배치중인 배를 마우스가 마지막에 있었던 그리드에 배치
            ShipDiploymentMode(false);                      // 배치모드 종료
        }        
    }
    
    /// <summary>
    /// 마우스가 움직일 때 실행될 함수
    /// </summary>
    /// <param name="context"></param>
    private void OnBattleFieldMouseMove(InputAction.CallbackContext context)
    {
        Vector2 pos = context.ReadValue<Vector2>();
        Ray ray = Camera.main.ScreenPointToRay(pos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100.0f, LayerMask.GetMask("BattleField"))
            && hit.collider.gameObject == this.gameObject)
        {
            // 내 필드위에서 마우스를 움직였을 때만 동작
            Vector3 diff = hit.point - transform.position;
            Vector2Int coord = new Vector2Int((int)diff.x, (int)-diff.z);   // 스크린좌표를 그리드 좌표로 변경
            if (coord != oldMouseCoord)
            {
                // 좌표 값이 변경되었을 때만 실행
                //Debug.Log($"2D coord : {coord}");
                //selectedShip.transform.position = transform.position + new Vector3(coord.x + 0.5f, 0.0f, -coord.y - 0.5f);  
                selectedShip.transform.position = GridToWorld(coord.x, coord.y);    // 배치 중인 배의 위치를 그리드 좌표에 맞춰 이동
                Vector2Int[] temp = new Vector2Int[selectedShip.size];
                bool deployable = IsShipDeployment(coord, selectedShip, out temp);  // 백가 배치 가능한지 확인
                //Debug.Log($"result : {success}");
                selectedShip.SetMaterial(deployable);       // 배치 가능 여부에 따라 머티리얼 변경

                oldMouseCoord = coord;                      // 그리드 좌표 기록
            }
        }
        else
        {
            // 내 필드 밖에서 마우스를 움직였을 때
            Vector3 newPos = Camera.main.ScreenToWorldPoint(pos);   // 스크린 좌표를 월드 좌표로 변경하고
            selectedShip.transform.position = new Vector3(newPos.x, 0, newPos.z);   // 배의 위치를 이동
            selectedShip.SetMaterial(false);    // 머티리얼은 무조건 error로 설정
        }
    }    
}
