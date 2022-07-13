using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 맵. 게임판
/// </summary>
public class Map : MonoBehaviour
{
    public GameObject placeNormal;  // 일반 맵 한칸 표시용 프리팹
    public GameObject placeCorner;  // 맵 구석 한칸 표시용 프리팹

    Place[] places;                 // 각 장소(40개가 있음)
    const int SideSize = 10;        // 일반칸 9개 + 구석칸 1개
    const int NumOfSize = 4;        // 4면
    const int NumOfPlaces = SideSize * NumOfSize;   // 전체 칸수

    PlayerInputActionMaps actions;

    /// <summary>
    /// 초기화 함수(우선순위 상관없음)
    /// </summary>
    public void Initialize()
    {
        places = new Place[NumOfPlaces];    // 장소 배열 잡기

        MapData[] mapDatas = LoadMapData(); // 데이터 파일에서 맵 정보 불러오기
        GameObject[] mapObjects = MakeMapObject();  // 전체 칸 배치
        for (int i = 0; i < NumOfPlaces; i++)   // 각 칸들을 타입에 맞춰 컴포넌트 추가하고 초기화
        {
            switch (mapDatas[i].type)
            {
                case PlaceType.City:
                    places[i] = mapObjects[i].AddComponent<City>();
                    break;
                case PlaceType.CityBase:
                    places[i] = mapObjects[i].AddComponent<CityBase>();
                    break;
                case PlaceType.Start:
                    places[i] = mapObjects[i].AddComponent<Place_Start>();
                    break;
                case PlaceType.Island:
                    places[i] = mapObjects[i].AddComponent<Place_Island>();
                    break;
                case PlaceType.Fund_Get:
                    places[i] = mapObjects[i].AddComponent<Place_FundGet>();
                    break;
                case PlaceType.Fund_Pay:
                    places[i] = mapObjects[i].AddComponent<Place_FundPay>();
                    break;
                case PlaceType.SpaceShip:
                    places[i] = mapObjects[i].AddComponent<Place_SpaceShip>();
                    break;
                case PlaceType.GoldenKey:
                    places[i] = mapObjects[i].AddComponent<Place_GoldenKey>();
                    break;
                default:
                    break;
            }
            places[i].Initialize(mapObjects[i], ref mapDatas[i]);   // 한칸씩 초기화
        }

        actions = new PlayerInputActionMaps();
        actions.Map.Enable();
        actions.Map.Point.performed += OnPointMove;
    }

    private void OnPointMove(InputAction.CallbackContext context)
    {
        Vector2 screenPos = context.ReadValue<Vector2>();
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        Place place = null;
        if (Physics.Raycast(ray, out RaycastHit hit, 10.0f, LayerMask.GetMask("Place")))
        {
            place = hit.collider.GetComponentInParent<Place>();
        }
        GameManager.Inst.UI_Manager.SetPlaceInfo(place);
    }

    /// <summary>
    /// 장소 가져오는 함수
    /// </summary>
    /// <param name="id">가져올 장소의 id</param>
    /// <returns>찾은 장소</returns>
    public Place GetPlace(MapID id)
    {
        return places[(int)id];
    }

    /// <summary>
    /// 맵에 있는 플레이어를 이동시키는 함수
    /// </summary>
    /// <param name="player">이동시킬 플레이어</param>
    /// <param name="count">이동시킬 칸 수</param>
    public void Move(Player player, int count)
    {
        int dest = (int)player.Position + count;    // 목적지 계산하기
        if(dest >= NumOfPlaces)         // 한바퀴 돌았을 경우
        {
            dest -= NumOfPlaces;
            Place_Start start = GetPlace(MapID.Start) as Place_Start;
            start.GetSalaray(player);   // 월급 추가
        }

        player.Position = (MapID)dest;  // 실제 위치 변경
        Place place = GetPlace(player.Position);
        player.transform.position = place.GetPlayerPosition(player.Type);   // 말 위치 변경
        place.OnArrive(player);         // 장소 도착 함수 실행
    }

    /// <summary>
    /// 맵에 있는 플레이어를 이동시키는 함수(특정 칸으로 즉시 이동할 때 사용할 함수)
    /// </summary>
    /// <param name="player">이동시킬 플레이어</param>
    /// <param name="mapID">이동할 장소</param>
    public void Move(Player player, MapID mapID)
    {
        int move = mapID - player.Position;
        if( move < 0 )
        {
            move = NumOfPlaces + move;
        }
        Move(player, move);
    }

    /// <summary>
    /// CSV 데이터 파일 읽기 함수
    /// </summary>
    /// <returns>읽어드린 데이터</returns>
    MapData[] LoadMapData()
    {
        string fullPath = $"{Application.dataPath}/Data/MapData.csv";   // 파일 경로 만들기

        string[] allLineText = File.ReadAllLines(fullPath);             // 데이터 파일의 모든 줄 읽기
        MapData[] mapDatas = new MapData[allLineText.Length - 1];       // 읽은 데이터를 저장할 배열만들기
        for (int i = 1; i < allLineText.Length; i++)    // 첫째줄은 필드 이름이니까 스킵하고 나머지 줄 처리
        {
            string[] data = allLineText[i].Split(',');  // ,를 기준으로 텍스트 분리
            MapData mapData = new();
            mapData.id = (MapID)int.Parse(data[0]);                // 분리된 각 항목들을 순서에 맞게 처리
            mapData.type = (PlaceType)int.Parse(data[1]);
            mapData.name = data[2];
            switch (mapData.type)
            {
                case PlaceType.City:
                    mapData.placeBuyPrice = int.Parse(data[3]);
                    mapData.placeUsePrice = int.Parse(data[4]);
                    mapData.villaBuyPrice = int.Parse(data[5]);
                    mapData.buildingBuyPrice = int.Parse(data[6]);
                    mapData.hotelBuyPrice = int.Parse(data[7]);
                    mapData.villaUsePrice = int.Parse(data[8]);
                    mapData.buildingUsePrice = int.Parse(data[9]);
                    mapData.hotelUsePrice = int.Parse(data[10]);
                    break;
                case PlaceType.CityBase:
                    mapData.placeBuyPrice = int.Parse(data[3]);
                    mapData.placeUsePrice = int.Parse(data[4]);
                    break;
                case PlaceType.Start:
                case PlaceType.Island:
                case PlaceType.Fund_Get:
                case PlaceType.Fund_Pay:
                case PlaceType.SpaceShip:
                case PlaceType.GoldenKey:
                default:
                    break;
            }

            mapDatas[i - 1] = mapData;  // mapData를 배열에 하나씩 넣기
        }

        return mapDatas;
    }

    /// <summary>
    /// 맵의 모든 칸 오브젝트 생성
    /// </summary>
    /// <returns></returns>
    GameObject[] MakeMapObject()
    {
        // 보이는 부분 배치하기
        GameObject[] allPaceObject = new GameObject[NumOfPlaces];

        GameObject prefab = null;
        Vector3 makeDir = Vector3.left;
        float makeAngle = 0.0f;
        Vector3 startPos = transform.position;  // 한 줄의 시작 위치
        for (int i = 0; i < NumOfSize; i++)
        {
            GameObject place = null;
            for (int j = 0; j < SideSize; j++)
            {
                // 일반칸 9개 만들고 구석칸 1개 생성
                if (j == 9)
                {
                    prefab = placeCorner;
                }
                else
                {
                    prefab = placeNormal;
                }
                place = Instantiate(prefab, this.transform);
                int id = 1 + i * SideSize + j;
                id %= 40;   // 마지막칸의 id를 0으로 만들기(시작칸이니까)
                place.name = $"ID_{id}";

                // 이 줄의 시작위치와 생성 방향을 기준으로 오브젝트 생성
                makeAngle = 90.0f * i;
                place.transform.Rotate(0, makeAngle, 0);    
                place.transform.Translate(j * makeDir + startPos, Space.World);

                if (id == 0)
                {
                    place.transform.SetAsFirstSibling();    // 시작칸이 마지막에 있으니 첫번째로 돌리기

                }
                allPaceObject[id] = place;  // 한칸씩 저장하고
            }
            startPos = place.transform.position;    // 새 한 줄의 시작위치 설정
            makeDir = Quaternion.Euler(0, 90, 0) * makeDir; // 생성 방향 변경
        }

        return allPaceObject;
    }
}
