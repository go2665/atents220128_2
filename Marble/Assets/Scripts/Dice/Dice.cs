using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    // 1 - 0,270,0
    // 2 - 270,0,0
    // 3 - 180,0,0 
    // 4 - 0,0,0
    // 5 - 90,0,0
    // 6 - 90,90,0

    public int diceMax = 6;

    Quaternion[] diceEyeRotation = new Quaternion[6];

    private void Awake()
    {
        diceEyeRotation[0] = Quaternion.Euler(0, 270, 0);
        diceEyeRotation[1] = Quaternion.Euler(270, 0, 0);
        diceEyeRotation[2] = Quaternion.Euler(180, 0, 0);
        diceEyeRotation[3] = Quaternion.Euler(0, 0, 0 );
        diceEyeRotation[4] = Quaternion.Euler(90, 0, 0);
        diceEyeRotation[5] = Quaternion.Euler(90, 90, 0);
    }

    public int Roll()
    {
        int eye = Random.Range(1, diceMax + 1);
        transform.rotation = diceEyeRotation[eye - 1];

        return eye;
    }
}
