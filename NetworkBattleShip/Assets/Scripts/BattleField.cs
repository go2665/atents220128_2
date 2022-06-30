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
    readonly FieldCell[,] field = new FieldCell[FieldSize, FieldSize];

    /// <summary>
    /// 필드에 배치될 배
    /// </summary>
    readonly Ship[] ships = new Ship[ShipCount];

    /// <summary>
    /// 필드에 배치된 가라앉지 않은 배의 수
    /// </summary>
    int aliveShipCount = 0;   

    /// <summary>
    /// 현재 배치중인 배
    /// </summary>
    Ship selectedShip = null;

    /// <summary>
    /// 이전 마우스 커서 위치(필드 그리드 기준)
    /// </summary>
    Vector2Int oldMouseCoord = -Vector2Int.one;

    /// <summary>
    /// 플레이어의 필드인지 아닌지. true면 플레이어의 필드
    /// </summary>
    bool isPlayerField = false;


    // 프로퍼티 ------------------------------------------------------------------------------------
    /// <summary>
    /// 필드의 배가 모두 침몰되었는지 여부. true면 모든 배가 침몰되었다.
    /// </summary>
    public bool IsDepeat
    {
        get => (aliveShipCount <= 0);
    }    

    /// <summary>
    /// 플레이어의 필드인지 아닌지 여부. true면 플레이어의 필드
    /// </summary>
    public bool IsPlayerField
    {
        get => isPlayerField;
    }

    /// <summary>
    /// 남아 있는 배의 댓수
    /// (특정값과 액션을 묶어주기 위한 private 프로퍼티)
    /// </summary>
    private int AliveShipCount
    {
        get => aliveShipCount;
        set
        {
            aliveShipCount = value;
            OnAliveShipCountChange?.Invoke(aliveShipCount); // 값이 변하면 등록된 델리게이트 실행
        }
    }

    // 델리게이트 ----------------------------------------------------------------------------------
    /// <summary>
    /// 남아 있는 배의 수가 변경될 때 실행될 델리게이트
    /// </summary>
    public System.Action<int> OnAliveShipCountChange;

    // 함수들 --------------------------------------------------------------------------------------
    /// <summary>
    /// 게임이 시작될때 실행될 초기화 함수
    /// </summary>
    /// <param name="isPlayer">플레이어의 필드인지 아닌지. true면 플레이어의 필드</param>
    public void Initialize(bool isPlayer = false)
    {
        AliveShipCount = ShipCount;     // 시작 배 대수 설정
        for(int i=0;i<ShipCount;i++)
        {
            ships[i] = GameManager.Inst.MakeShip((ShipType)i);  // 배 생성
            ships[i].gameObject.transform.parent = transform;   // 필드에 배 추가
            ships[i].gameObject.SetActive(false);               // 우선 배는 안보이게 해놓기
        }
        isPlayerField = isPlayer;
        //transform.Find("Cover").gameObject.SetActive(!isPlayerField); // 일단은 가리지 않도록 설정

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
                //Debug.Log($"{ship.gameObject.name}을 ({pos.x}, {pos.y})에 배치할 수 없습니다.");
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
        return IsShipDeployment(pos, ship, out _);
    }

    /// <summary>
    /// 함선 배치. 배치할 수 없을 경우 실행 안함.
    /// </summary>
    /// <param name="pos">배치할 위치</param>
    /// <param name="ship">배치할 배</param>
    public void ShipDeployment(Vector2Int pos, Ship ship)
    {
        if (IsShipDeployment(pos, ship, out Vector2Int[] positions))
        {
            foreach (Vector2Int position in positions)
            {
                field[position.x, position.y].exists = FieldExists.Ship;
                field[position.x, position.y].ship = ship;
            }
            ship.Position = pos;
            ship.transform.position = GridToWorld(pos.x, pos.y);    // 배를 해당 그리드의 위치로 이동
            ship.gameObject.SetActive(true);                        // 배가 보이게 설정
            ship.IsDeployed = true;
            //Debug.Log($"{ship.gameObject.name}을 ({pos.x}, {pos.y})에 배치했습니다.");

            ShipDiploymentMode(false);                              // 배치모드 종료
        }
    }

    /// <summary>
    /// 위치가 필드 범위 안인지 체크하는 함수
    /// </summary>
    /// <param name="pos">체크할 위치</param>
    /// <returns>true면 필드 안의 위치, false면 필드 밖의 위치</returns>
    private bool IsValidPosition(Vector2Int pos)
    {
        return (-1 < pos.x && pos.x < FieldSize && -1 < pos.y && pos.y < FieldSize);
    }

    /// <summary>
    /// 공격 가능한 위치인지 확인
    /// </summary>
    /// <param name="pos">공격할 위치</param>
    /// <returns>공격가능하면 true, 아니면 false</returns>
    private bool IsAttackable(Vector2Int pos)
    {
        return (field[pos.x, pos.y].exists != FieldExists.CannonBall);
    }

    /// <summary>
    /// 위치가 필드 범위 안이고 공격이 가능한 지역인지 확인
    /// </summary>
    /// <param name="pos">확인할 위치</param>
    /// <returns>적절한 공격 가능 위치면 true, 아니면 false</returns>
    public bool IsValidAndAttackable(Vector2Int pos)
    {
        return (IsValidPosition(pos) && IsAttackable(pos));
    }

    /// <summary>
    /// 위치가 공격 실패한 지점인지 확인
    /// </summary>
    /// <param name="pos">확인할 위치</param>
    /// <returns>true면 공격이 실패했던 지점. 필드 안이 아니거나 공격했던 지점이 아니거나 공격이 성공했으면 false</returns>
    public bool IsAttckFailPosition(Vector2Int pos)
    {
        return (IsValidPosition(pos) && field[pos.x, pos.y].exists == FieldExists.CannonBall && field[pos.x, pos.y].ship == null);
    }

    /// <summary>
    /// 이 필드가 공격을 받음처리
    /// </summary>
    /// <param name="pos">공격받은 위치</param>
    public Ship Attacked(Vector2Int pos)
    {
        Debug.Log($"{pos}가 공격 받았다.");
        Ship hitShip = null;
        if (IsValidPosition(pos))   // 적절한 위치여야 한다.
        {
            if (IsAttackable(pos))  // 공격 가능한 위치여야 한다.(이미 공격하지 않은 곳)
            {
                bool isShipHit = field[pos.x, pos.y].exists == FieldExists.Ship;
                if (isShipHit)
                {
                    hitShip = field[pos.x, pos.y].ship;
                    // 배를 공격했으면
                    //Debug.Log($"{gameObject.name} : 배({field[pos.x, pos.y].ship.name})이 ({pos.x},{pos.y})를 공격받았습니다.");
                    
                    hitShip.Hit();             // 배에 데미지를 주고
                    if (hitShip.IsSinking)     // 배가 가라앉았는지를 확인
                    {
                        AliveShipCount--;                       // 생존해 있는 배 수 감소
                                                                //Debug.Log($"남아있는 함선의 수 : {aliveShipCount}척");
                                                                //if(IsDepeat)
                                                                //{
                                                                //    // 게임 오버
                                                                //    //Debug.Log($"{this.name}이 패배");
                                                                //    GameManager.Inst.StateChange(GameState.GameOver);
                                                                //}
                    }
                }
                else
                {
                    //Debug.Log($"{gameObject.name} : 바다({pos.x},{pos.y})가 공격받았습니다.");
                }
                SetBombMark(pos, isShipHit);
            }
        }
        return hitShip;
    }

    public void SetBombMark(Vector2Int pos, bool isShipHit)
    {
        GameObject bombMark = GameManager.Inst.MakeBombMark(isShipHit);
        bombMark.transform.position = GridToWorld(pos.x, pos.y);
        bombMark.transform.Translate(Vector3.up, Space.World);
        bombMark.transform.parent = this.transform;
        field[pos.x, pos.y].exists = FieldExists.CannonBall;    // 공격 받았다고 표시
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
                    selectedShip.gameObject.transform.position = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                    selectedShip.gameObject.SetActive(true);        // 대상을 배치중인 배로 설정하고 enable하기
                }
            }
        }
        else
        {
            // 배치모드를 끌 경우
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

    /// <summary>
    /// 테스트 용도. 원하는대로 함선을 배치하기 위한 함수
    /// </summary>
    public void TestDeployment()
    {
        ships[0].Rotate(false);
        ShipDeployment(new(0, 0), ships[0]);
        ShipDeployment(new(1, 1), ships[1]);
        ShipDeployment(new(2, 1), ships[2]);
        ShipDeployment(new(3, 1), ships[3]);
        ShipDeployment(new(4, 1), ships[4]);
    }

    /// <summary>
    /// 배가 배치되었을 때 차지할 그리드의 좌표들을 리턴하는 함수
    /// </summary>
    /// <param name="ship">위치를 찾을 배</param>
    /// <param name="pos">배가 배치될 위치</param>
    /// <returns>배가 pos 위치에 배치되면 차지하게 될 그리드 좌표들</returns>
    Vector2Int[] ShipPositions(Ship ship, Vector2Int pos)
    {
        Vector2Int[] positions = new Vector2Int[ship.size];
        int index = (int)ship.Direction;    // 배 방향에 맞춰 어느 그리드를 가질지 결정
        Vector2Int[] temp = { new Vector2Int(-1, 0), new Vector2Int(0, -1), new Vector2Int(1, 0), new Vector2Int(0, 1) };
        for (int i = 0; i < ship.size; i++)
        {
            positions[i] = pos + temp[index] * i;   // 배가 배치될 좌표들 기록
        }

        return positions;
    }

    /// <summary>
    /// 배 한척을 배치 취소 하는 함수
    /// </summary>
    /// <param name="ship">배치를 취소할 함선</param>
    void CancelDeployment(Ship ship)
    {
        Vector2Int[] positions = ShipPositions(ship, ship.Position);
        foreach(var pos in positions)
        {
            field[pos.x, pos.y].exists = FieldExists.None;  // 필드에 설정되어 있던 값들 제거
            field[pos.x, pos.y].ship = null;
        }
        ship.Position = Vector2Int.zero;        // 배에 기록된 정보들도 초기화
        ship.IsDeployed = false;
        ship.transform.position = Vector3.zero;
        ship.gameObject.SetActive(false);
    }

    /// <summary>
    /// 모든 배의 배치를 취소하는 함수
    /// </summary>
    public void ResetDeployment()
    {
        foreach(var ship in ships)
        {
            CancelDeployment(ship);
        }
    }

    /// <summary>
    /// 그리드 좌표를 월드좌표로 변경해주는 함수 
    /// </summary>
    /// <param name="x">그리드의 x</param>
    /// <param name="y">그리드의 y</param>
    /// <returns>대상 그리드의 월드 좌표</returns>
    Vector3 GridToWorld(int x, int y)
    {
        return transform.position + new Vector3(x + 0.5f, 0.0f, -y - 0.5f);
    }

    /// <summary>
    /// 월드좌표를 그리드 좌표로 변경해주는 함수
    /// </summary>
    /// <param name="worldPos">변경할 월드 좌표</param>
    /// <returns>변환 완료된 그리드 좌표</returns>
    public Vector2Int WorldToGrid(Vector3 worldPos)
    {
        Vector3 diff = worldPos - transform.position;
        return new Vector2Int((int)diff.x, (int)-diff.z);
    }



    // 입력 처리 함수 ------------------------------------------------------------------------------
    /// <summary>
    /// 마우스 휠을 움직였을 때 실행될 함수
    /// </summary>
    /// <param name="whellDelta">마우스 휠이 움직인 정도. 올리면 120, 내리면 -120</param>
    public void OnMouseWheel(float whellDelta)
    {
        // 상태별로 다르게 동작하도록 변경
        switch (GameManager.Inst.State)
        {
            case GameState.Ready:
                break;
            case GameState.ShipDeployment:
                OnMouseWheel_ShipdeployState(whellDelta);
                break;
            case GameState.Battle:
                break;
            case GameState.GameOver:
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 마우스를 클릭했을 때 실행될 함수
    /// </summary>
    /// <param name="position">마우스 클릭한 좌표(스크린좌표)</param>
    public void OnClick(Vector2 position)
    {
        // 상태별로 다르게 동작하도록 변경
        switch (GameManager.Inst.State)
        {
            case GameState.Ready:
                break;
            case GameState.ShipDeployment:
                OnClick_ShipdeployState(position);
                break;
            case GameState.Battle:
                OnClick_Battle(position);
                break;
            case GameState.GameOver:
                break;
            default:
                break;
        }

    }

    /// <summary>
    /// 마우스가 움직일 때 실행될 함수
    /// </summary>
    /// <param name="position">마우스 위치 좌표(스크린좌표)</param>
    public void OnMouseMove(Vector2 position)
    {
        // 상태별로 다르게 동작하도록 변경
        switch (GameManager.Inst.State)
        {
            case GameState.Ready:
                break;
            case GameState.ShipDeployment:
                OnMouseMove_ShipdeployState(position);
                break;
            case GameState.Battle:
                break;
            case GameState.GameOver:
                break;
            default:
                break;
        }
    }

    private void OnMouseWheel_ShipdeployState(float whellDelta)
    {
        // 휠 움직임 받아오기
        if (selectedShip != null)
        {
            //Debug.Log($"Wheel : {whellDelta}"); 
            selectedShip.Rotate(whellDelta < 0.0f); // 받아온 값을 기준으로 회전(휠을 올리면 시계방향, 휠을 내리면 반시계밯향)            
            bool deployable = IsShipDeployment(oldMouseCoord, selectedShip);    // 배가 배치 가능한지 확인
            selectedShip.SetMaterial(deployable);   // 배치 가능 여부에 따라 머티리얼 변경
        }
    }       

    private void OnClick_ShipdeployState(Vector2 position)
    {
        if (selectedShip != null)
        {
            // 함선 배치용 작업
            Ray ray = Camera.main.ScreenPointToRay(position);
            if (Physics.Raycast(ray, out RaycastHit hit, 100.0f, LayerMask.GetMask("BattleField"))
                && hit.collider.gameObject == this.gameObject)  // 입력은 왼쪽 필드만 받는다.(내 필드를 클릭했다.)
            {

                // 내 필드를 클릭했을 때만 동작
                ShipDeployment(oldMouseCoord, selectedShip);    // 배치중인 배를 마우스가 마지막에 있었던 그리드에 배치                
            }
        }
        else
        {
            // 선택된 배가 없을 때 배치된 배를 클릭하면 재 배치에 들어간다.
            Ray ray = Camera.main.ScreenPointToRay(position);
            if (Physics.Raycast(ray, out RaycastHit hit, 100.0f, LayerMask.GetMask("Ship")))
            {
                Ship ship = hit.collider.GetComponentInParent<Ship>();
                CancelDeployment(ship);
                ShipDiploymentMode(true, ship.shipType);
                Vector3 newPos = Camera.main.ScreenToWorldPoint(position);
                selectedShip.transform.position = new Vector3(newPos.x, 0, newPos.z);
            }
        }
    }
    private void OnClick_Battle(Vector2 position)
    {
        Ray ray = Camera.main.ScreenPointToRay(position);
        if (Physics.Raycast(ray, out RaycastHit hit, 100.0f, LayerMask.GetMask("BattleField")))
        {
            BattleField field = hit.collider.gameObject.GetComponent<BattleField>();
            if (!field.isPlayerField)
            {
                NetPlayer player = GameManager.Inst.PlayerLeft;
                //if (!player.IsTurnActionFinish)   // 임시조치
                {
                    Vector2Int gridPos = field.WorldToGrid(hit.point);
                    //Debug.Log($"적 필드 : {gridPos}");
                    player.Attack(gridPos);
                }
            }
        }
    }

    private void OnMouseMove_ShipdeployState(Vector2 position)
    {
        if (selectedShip != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(position);
            if (Physics.Raycast(ray, out RaycastHit hit, 100.0f, LayerMask.GetMask("BattleField"))
                && hit.collider.gameObject == this.gameObject)
            {
                // 내 필드위에서 마우스를 움직였을 때만 동작
                Vector3 diff = hit.point - transform.position;
                Vector2Int coord = new((int)diff.x, (int)-diff.z);   // 스크린좌표를 그리드 좌표로 변경
                if (coord != oldMouseCoord)
                {
                    // 좌표 값이 변경되었을 때만 실행
                    //Debug.Log($"2D coord : {coord}");
                    selectedShip.transform.position = GridToWorld(coord.x, coord.y);    // 배치 중인 배의 위치를 그리드 좌표에 맞춰 이동                    
                    bool deployable = IsShipDeployment(coord, selectedShip);            // 배가 배치 가능한지 확인                                                                                       
                    selectedShip.SetMaterial(deployable);       // 배치 가능 여부에 따라 머티리얼 변경
                    oldMouseCoord = coord;                      // 그리드 좌표 기록
                }
            }
            else
            {
                // 내 필드 밖에서 마우스를 움직였을 때
                Vector3 newPos = Camera.main.ScreenToWorldPoint(position);   // 스크린 좌표를 월드 좌표로 변경하고
                selectedShip.transform.position = new Vector3(newPos.x, 0, newPos.z);   // 배의 위치를 이동
                selectedShip.SetMaterial(false);    // 머티리얼은 무조건 error로 설정
            }
        }
    }

    // 유니티 이벤트 함수 --------------------------------------------------------------------------
       
}
