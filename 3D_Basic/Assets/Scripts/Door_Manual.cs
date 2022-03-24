using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_Manual : Door, IUseable
{
    private bool isOpen = false;

    public void OnUse()
    {        
        if( isOpen )
        {
            Close();
            isOpen = false;
        }
        else
        {
            Open();
            isOpen = true;
        }
    }
}
