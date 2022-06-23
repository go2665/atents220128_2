using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Canvas_Battle : MonoBehaviour
{
    // UI 컴포넌트들 캐싱용
    TextMeshProUGUI playerPoint = null;
    TextMeshProUGUI enemyPoint = null;
    TextMeshProUGUI turnPoint = null;
    Slider turnTimeSlider = null;

    private void Awake()
    {
        // UI 컴포넌트 찾기
        playerPoint = transform.Find("PlayerPoint").GetComponent<TextMeshProUGUI>();
        enemyPoint = transform.Find("EnemyPoint").GetComponent<TextMeshProUGUI>();
        turnPoint = transform.Find("TurnPoint").GetComponent<TextMeshProUGUI>();
        turnTimeSlider = GetComponentInChildren<Slider>();
    }

    private void Start()
    {
        // 각종 델리게이트 연결하기
        GameManager.Inst.FieldLeft.OnAliveShipCountChange += RefreshPlayerPoint;
        GameManager.Inst.FieldRight.OnAliveShipCountChange += RefreshEnemyPoint;
        GameManager.Inst.TurnManager.OnTurnChange += RefreshTurnPoint;
        GameManager.Inst.TurnManager.OnCountDownChange += RefreshTurnTimeSlide;
    }

    /// <summary>
    /// 턴의 남은 시간을 슬라이더로 표현
    /// </summary>
    /// <param name="ratio">현재 턴에서 남은 시간 비율</param>
    private void RefreshTurnTimeSlide(float ratio)
    {
        turnTimeSlider.value = ratio;
    }

    /// <summary>
    /// 현재 턴 번호 변경
    /// </summary>
    /// <param name="point">현재 턴 수</param>
    private void RefreshTurnPoint(int point)
    {
        turnPoint.text = point.ToString();
    }

    /// <summary>
    /// 플레이어에게 남은 배 수 출력
    /// </summary>
    /// <param name="point">남은 배 수</param>
    void RefreshPlayerPoint(int point)
    {
        playerPoint.text = point.ToString();
    }

    /// <summary>
    /// 적에게 남은 배 수 출력
    /// </summary>
    /// <param name="point">남은 배 수</param>
    void RefreshEnemyPoint(int point)
    {
        enemyPoint.text = point.ToString();
    }
}
