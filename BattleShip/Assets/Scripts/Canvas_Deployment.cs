using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Canvas_Deployment : MonoBehaviour
{
    public Button[] buttons = null;

    private void Start()
    {
        //for(int i=0;i<buttons.Length; i++)
        //{
        //    //buttons[i].onClick.AddListener(() => SelectShip((ShipType)i));
        //}

        buttons[0].onClick.AddListener(() => SelectShip(ShipType.Ship5));
        buttons[1].onClick.AddListener(() => SelectShip(ShipType.Ship4));
        buttons[2].onClick.AddListener(() => SelectShip(ShipType.Ship3_1));
        buttons[3].onClick.AddListener(() => SelectShip(ShipType.Ship3_2));
        buttons[4].onClick.AddListener(() => SelectShip(ShipType.Ship2));
        buttons[5].onClick.AddListener(Cancel);     // 취소
        buttons[6].onClick.AddListener(Random);     // 랜덤
        //buttons[7].onClick.AddListener();   // 결정

    }

    void SelectShip(ShipType type)
    {
        GameManager.Inst.FieldLeft.ShipDiploymentMode(true, type);
        buttons[(int)type].interactable = false;
    }

    void Cancel()
    {
        GameManager.Inst.FieldLeft.ResetDeployment();
        int size = (int)ShipType.SizeOfShipType;
        for (int i=0;i<size;i++)
        {
            buttons[i].interactable = true;
        }
    }

    void Random()
    {
        GameManager.Inst.FieldLeft.RandomDeployment();
        int size = (int)ShipType.SizeOfShipType;
        for (int i = 0; i < size; i++)
        {
            buttons[i].interactable = false;
        }
    }
}
