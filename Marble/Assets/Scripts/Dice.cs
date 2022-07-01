using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    public GameObject dicePrefab;
    public int diceMax = 6;

    public int RollDice()
    {
        return Random.Range(1, diceMax + 1);
    }
}
