using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Place : MonoBehaviour
{
    GameObject placeObject;    
    protected int id;    
    public string placeName;

    public Transform[] playerPostions;

    public int ID
    {
        get => id;
    }

    public virtual void OnArrive(Player player)
    {
    }

    public virtual void OnTurnStart(Player player)
    {
    }

    /// <summary>
    /// 장소 한칸의 초기화 함수
    /// </summary>
    /// <param name="obj">장소 한칸을 보여줄 오브젝트</param>
    /// <param name="mapData">이 장소에 대한 데이터</param>
    public virtual void Initialize(GameObject obj, ref MapData mapData)
    {
        placeObject = obj;
        id = mapData.id;
        placeName = mapData.name;

        int numOfPlayer = GameManager.Inst.NumOfPlayer;
        playerPostions = new Transform[numOfPlayer - 1];
        for(int i=1;i< numOfPlayer;i++)
        {
            playerPostions[i - 1] = transform.Find($"Player{i}");
        }
    }

    /// <summary>
    /// 이 장소에 말이 배치될 위치를 리턴하는 함수
    /// </summary>
    /// <param name="type">배치될 플레이어</param>
    /// <returns>배치될 위치</returns>
    public Vector3 GetPlayerPosition(PlayerType type)
    {
        return playerPostions[(int)type - 1].position;
    }
}
