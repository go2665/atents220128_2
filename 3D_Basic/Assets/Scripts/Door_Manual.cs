using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_Manual : Door, IUseable
{
    //public override void Open()
    //{
    //    //Debug.Log("Manual Open");
    //    base.Open();
    //    isOpen = true;
    //}

    //public override void Close()
    //{
    //    //Debug.Log("Manual Close");
    //    base.Close();
    //    isOpen = false;
    //}

    public void OnUse()
    {
        ToggleOpenClose();
    }
}
