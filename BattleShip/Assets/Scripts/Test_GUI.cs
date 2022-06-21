using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_GUI : MonoBehaviour
{
    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(Screen.width - 310, 10, 300, 300));
        if(GUILayout.Button("함선배치 5"))
        {
            GameManager.Inst.FieldLeft.ShipDiploymentMode(true, ShipType.Ship5);
        }
        if (GUILayout.Button("함선배치 4"))
        {
            GameManager.Inst.FieldLeft.ShipDiploymentMode(true, ShipType.Ship4);
        }
        if (GUILayout.Button("함선배치 3-1"))
        {
            GameManager.Inst.FieldLeft.ShipDiploymentMode(true, ShipType.Ship3_1);
        }
        if (GUILayout.Button("함선배치 3-2"))
        {
            GameManager.Inst.FieldLeft.ShipDiploymentMode(true, ShipType.Ship3_2);
        }
        if (GUILayout.Button("함선배치 2"))
        {
            GameManager.Inst.FieldLeft.ShipDiploymentMode(true, ShipType.Ship2);
        }
        GUILayout.EndArea();
    }
}
