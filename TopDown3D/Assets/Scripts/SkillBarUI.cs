using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBarUI : MonoBehaviour
{
    CoolTimeUI[] slots = null;
    public CoolTimeUI this[int index]
    {
        get => slots[index];
    }

    private void Awake()
    {
        slots = GetComponentsInChildren<CoolTimeUI>();
    }

    public void SetSelected(int index)
    {
        for(int i=0; i<slots.Length; i++)
        {
            if( i!=index)
            {
                slots[i].SetSelected(false);
            }
            else
            {
                slots[i].SetSelected(true);
            }
        }
    }
}
