using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlaceType
{
    City = 0,   // 일반 도시
    CityBase,   // 살 수 있지만 건물 못짓는 도시
    Start,      // 시작지점
    Island,     // 무인도
    Fund_Get,   // 기금 획득
    Fund_Pay,   // 기금 기부
    SpaceShip,  // 우주왕복선
    GoldenKey   // 황금열쇠
}
