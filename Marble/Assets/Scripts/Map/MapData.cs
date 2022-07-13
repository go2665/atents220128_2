using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 데이터 파일에서 데이터를 읽을때 받아야 할 정보들
/// </summary>
public struct MapData
{
    public MapID id;                // MapID
    public PlaceType type;          // 장소의 종류(도시, 황금열쇠, 무인도 등등)
    public string name;             // 장소의 이름
    public int placeBuyPrice;       // 땅 구입비
    public int placeUsePrice;       // 땅 사용료
    public int villaBuyPrice;       // 별장 구입비
    public int buildingBuyPrice;    // 빌딩 구입비
    public int hotelBuyPrice;       // 호텔 구입비
    public int villaUsePrice;       // 별장 사용료
    public int buildingUsePrice;    // 빌딩 사용료
    public int hotelUsePrice;       // 호텔 사용료
}
