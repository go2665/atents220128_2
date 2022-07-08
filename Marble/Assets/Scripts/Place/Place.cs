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

    public Vector3 GetPlayerPosition(PlayerType type)
    {
        return playerPostions[(int)type - 1].position;
    }
}
