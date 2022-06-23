using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    // 주요 데이터 ---------------------------------------------------------------------------------
    /// <summary>
    /// 1턴이 타임아웃되는데 걸리는 시간
    /// </summary>
    public float CountDownTime = 5.0f;

    /// <summary>
    /// 왼쪽 플레이어(실제 플레이어)
    /// </summary>
    Player playerLeft = null;

    /// <summary>
    /// 오른쪽 플레이어(적)
    /// </summary>
    Player playerRight = null;

    /// <summary>
    /// 현재 진행중인 턴
    /// </summary>
    int currentTurn = 1;

    /// <summary>
    /// 현재 턴에서 남아있는 행동 시간
    /// </summary>
    float countDown = 0.0f;

    /// <summary>
    /// 현재 진행중인 턴을 표시하는 프로퍼티
    /// </summary>
    private int CurrentTurn
    {
        get => currentTurn;
        set
        {
            currentTurn = value;
            OnTurnChange?.Invoke(currentTurn);  // 값이 변경될 경우 델리게이트 실행
        }
    }

    /// <summary>
    /// 현재 턴에서 남아있는 행동 시간을 표시하는 프로퍼티
    /// </summary>
    private float CountDown
    {
        get => countDown;
        set
        {
            countDown = value;
            OnCountDownChange?.Invoke(Mathf.Clamp(countDown / CountDownTime, 0.0f, 1.0f));  // 값이 변경될 경우 델리게이트 실행
        }
    }

    /// <summary>
    /// 값이 변경될 때 실행될 델리게이트들
    /// </summary>
    public System.Action<int> OnTurnChange;
    public System.Action<float> OnCountDownChange;

    // 함수들 --------------------------------------------------------------------------------------
    /// <summary>
    /// 턴이 시작할 때 할 초기화 작업들을 실행
    /// </summary>
    void StartTurn()
    {
        Debug.Log($"{CurrentTurn} 턴 시작");
        CountDown = CountDownTime;          // 카운트 다운 시간 초기화
        playerLeft.TurnStartReset();        // 각 플레이어들이 다시 공격 가능하도록 초기화
        playerRight.TurnStartReset();
    }

    /// <summary>
    /// 카운트 다운 실행. 카운트 다운이 다 될 경우 TimeOut 함수 실행
    /// </summary>
    void CountDownProcess()
    {
        CountDown -= Time.deltaTime;    // 지속적으로 시간 감소
        if (CountDown < 0.0f)
        {
            TimeOut();                  // 시간이 다되면 TimeOut 실행
        }
    }

    /// <summary>
    /// countDown 시간이 다되었을 때 실행될 함수. 플레이어들 강제 공격시키고 턴 종료
    /// </summary>
    void TimeOut()
    {        
        playerLeft.ForcedAttack();      // 플레이어들 강제 공격
        playerRight.ForcedAttack();
        EndTurn();                      // 턴 종료
    }

    /// <summary>
    /// 턴이 종료될 때 일어나야 할 일들을 실행
    /// </summary>
    void EndTurn()
    {        
        Debug.Log($"{CurrentTurn} 턴 종료");
        CurrentTurn++;      // 턴 수 증가

        if(playerLeft.IsDepeat || playerRight.IsDepeat)
        {
            // 게임 종료
            GameManager.Inst.StateChange(GameState.GameOver);
            return;
        }

        StartTurn();        // 다음 턴 시작
    }

    // 유니티 이벤트 함수 --------------------------------------------------------------------------   

    private void OnEnable()
    {
        playerLeft = GameManager.Inst.PlayerLeft;   // GameManager에서 플레이어 가져와서 설정
        playerRight = GameManager.Inst.PlayerRight;

        CurrentTurn = 1;
        StartTurn();    // 첫번째 턴 시작
    }

    private void Update()
    {
        CountDownProcess(); // 카운트다운 진행

        if (playerLeft.IsTurnActionFinish && playerRight.IsTurnActionFinish)    // 두 플레이어가 모두 공격하면 다음 턴 실행
        {
            EndTurn();
        }
    }
}
