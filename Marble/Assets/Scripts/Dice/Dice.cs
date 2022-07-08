using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    // 1 - 0,0,270
    // 2 - 0,0,0
    // 3 - 270,0,0 
    // 4 - 90,0,0
    // 5 - 180,0,0
    // 6 - 0,0,90

    public int diceMax = 6;

    Quaternion[] diceEyeRotation = new Quaternion[6];

    private void Awake()
    {
        diceEyeRotation[0] = Quaternion.Euler(0, 0, 270);
        diceEyeRotation[1] = Quaternion.Euler(0, 0, 0);
        diceEyeRotation[2] = Quaternion.Euler(270, 0, 0);
        diceEyeRotation[3] = Quaternion.Euler(90, 0, 0);
        diceEyeRotation[4] = Quaternion.Euler(180, 0, 0);
        diceEyeRotation[5] = Quaternion.Euler(0, 0, 90);
    }

    public int Roll(bool showDiceRotate = false)
    {
        int eye = Random.Range(1, diceMax + 1);
        if (showDiceRotate)
        {
            transform.rotation = diceEyeRotation[eye - 1];
        }
        //Debug.Log($"{gameObject.name} : {eye}");

        return eye;
    }
}
