using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour, IUseable
{
    public Door targetDoor = null;

    Transform bar = null;
    bool switchOn = false;
    const float angle = 15.0f;

    private void Awake()
    {
        bar = transform.Find("BarPivot");
    }

    public void OnUse()
    {
        if(switchOn)
        {
            //switchOn = false;
            bar.rotation = Quaternion.Euler(-angle, 0, 0);
            switchOn = targetDoor.Close();
        }
        else
        {
            //switchOn = true;
            bar.rotation = Quaternion.Euler(angle, 0, 0);
            switchOn = targetDoor.Open();
        }
    }
}
