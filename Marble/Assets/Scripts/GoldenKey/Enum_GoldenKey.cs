using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GoldenKeyType
{
    ForcedSale = 0,         // 반액대매출
    IncomeTex,              // 종합소득세
    RepairCost,             // 건물수리비
    CrimePreventionCost,    // 방법비
    IslandEscapeTicket,     // 무인도탈출권
    FreePassTicket,         // 우대권
    MoveBack,               // 뒤로 이동
    Trip_Busan,             // 부산 여행
    Trip_Jeju,              // 제주 여행
    Trip_Seoul,             // 서울 여행
    ToIsland,               // 무인도에 조난
    GetFund,                // 사회복지기금으로 이동해서 획득
    RoundWorld,             // 세계일주(월급+기금 획득)
    MoveSpaceShip,          // 우주왕복선으로 이동
    CruiseTrip,              // 베이징으로 이동(퀸 엘리자베스 사용료도 추가 지급)
    AirplaneTrip,           // 콩코드 여객기를 타고 타이페이로(콩코드 비용도 추가 지급)
    Highway,                // 출발지로 이동
    NobelPrize,             // 노벨상
    LotteryWin,             // 복권당첨
    RaceWin,                // 자동차 경주 우승
    SchilarShip,            // 장학금
    Pension,                // 연금
    StudyAbroad,            // 해외 유학
    Hospital,               // 병원비
    Fine,                   // 과속 벌금
    Birthday                // 생일 축하
}

[System.Flags]
public enum GoldenKeyFeatureType : byte
{
    RemoveCity = 1, // 땅 잃어버리기
    MoneyGet = 2,   // 돈 얻기
    MoneyPay = 4,   // 돈 주기(기본 은행, 옵션으로 다른 사람)
    Move = 8,       // 이동
    EscapeIsland = 16,  // 무인도 탈출
    FreePass = 32,      // 우대권
    FundGet = 64        // 사회복지기금 얻기
}