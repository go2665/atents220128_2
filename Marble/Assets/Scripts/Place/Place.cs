using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Place : MonoBehaviour
{
    GameObject placeObject;    
    protected int id;    
    public string placeName;

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
    }
}
