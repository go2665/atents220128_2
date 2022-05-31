using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTank : MonoBehaviour, IControllable
{
    private Vector3 inputDir = Vector3.zero;
    public Vector3 KeyboardInputDir
    {
        set
        {
            inputDir = value;
            Debug.Log($"Input : {inputDir}");
        }
    }
}
