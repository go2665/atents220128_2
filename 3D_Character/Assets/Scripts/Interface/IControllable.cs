using UnityEngine;

interface IControllable
{
    void ControllerConnect();       // 컨트롤러가 연결될 때 초기화 시키는 함수
    void MoveInput(Vector2 dir);    // 받은 방향입력을 저장 및 처리하는 함수
    void MoveUpdate();              // Update 함수에서 실제로 움직임을 처리하는 함수
    void MoveModeChange();          // 걷기 달리기 전환 입력 처리
    void AttackInput();             // 공격 입력 처리
    void LockOnInput();             // 락온 입력 처리
    void PickupInput();             // 아이템 줍는 입력 처리
    void InventoryOnOffInput();     // 인벤토리 열고 닫기
}