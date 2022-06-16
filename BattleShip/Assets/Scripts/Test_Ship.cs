using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Ship : MonoBehaviour
{
    public Ship ship = null;

    private void Start()
    {
        //Test_Rotate();
        //Test_Hit();
    }

    private void Test_Hit()
    {
        for (int i = 0; i < 5; i++)
        {
            ship.Hit();
            Debug.Log($"{ship.gameObject.name}이 침몰했나요? {ship.IsSinking}");
        }        
    }

    private void Test_Rotate()
    {
        ship.Rotate();
        ship.Rotate();
        ship.Rotate();
        ship.Rotate();
        ship.Rotate(false);
        ship.Rotate(false);
        ship.Rotate(false);
        ship.Rotate(false);
        ship.Rotate();
        ship.Rotate(false);
        ship.Rotate();
        ship.Rotate();
        ship.Rotate(false);
        ship.Rotate();
    }
}
