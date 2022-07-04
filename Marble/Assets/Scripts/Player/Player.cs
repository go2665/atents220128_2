using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    int money;
    public int Money 
    { 
        get => money; 
        set
        {
            money = value;
        }
    }
}
