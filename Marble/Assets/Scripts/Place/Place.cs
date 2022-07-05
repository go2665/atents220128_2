using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Place : MonoBehaviour
{
    GameObject placeObject;
    protected int id;
    protected string placeName;

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
}
