using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// 기금을 획득할 수 있는 장소
/// </summary>
public class Place_FundGet : Place
{
    public GameObject fundTextPrefab;   // 누적된 금액을 표시할 추가 오브젝트의 프리팹
    TextMeshPro fundText;   // 누적된 금액을 표시할 UI text;
    int totalFund = 0;      // 누적된 금액

    private void Awake()
    {
        CoverImage_Corner cover = GetComponentInChildren<CoverImage_Corner>();
        cover.SetImage(CoverImage_Corner.Type.Fund);    // 커버 이미지 설정
        cover.transform.Rotate(0, 0, 90);        
    }

    private void Start()
    {
        GameObject textObj = Instantiate(fundTextPrefab, this.transform);
        fundText = textObj.GetComponent<TextMeshPro>();     // 프리팹 추가해서 미리 찾아놓기
    }

    /// <summary>
    /// 기금에 누적될 금액 추가
    /// </summary>
    /// <param name="money">누적되는 금액</param>
    public void AddFund(int money)
    {
        totalFund += money;
        fundText.text = totalFund.ToString();
    }

    /// <summary>
    /// 플레이어가 도착했을 때 실행할 함수. 누적된 기금 지급
    /// </summary>
    /// <param name="player">도착한 플레이어</param>
    public override void OnArrive(Player player)
    {
        //Debug.Log($"{player} : 기금을 {totalFund}원 얻었습니다.");
        player.Money += totalFund;
        totalFund = 0;
        fundText.text = totalFund.ToString();
    }
}
