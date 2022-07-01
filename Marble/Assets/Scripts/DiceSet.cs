using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceSet
{
    Dice[] dices;
    public int NumOfDices
    {
        get => dices.Length;
    }

    public DiceSet()
    {
        dices = MonoBehaviour.FindObjectsOfType<Dice>();
    }

    public int RollAll()
    {
        int result = 0;
        foreach(Dice dice in dices)
        {
            result += dice.Roll();
        }
        return result;
    }

    public bool RollAll(out int[] results)
    {   
        results = new int[dices.Length];
        for(int i=0;i<dices.Length;i++)
        {
            results[i] = dices[i].Roll();            
        }

        bool isDouble = true;
        int oldRoll = results[0];
        for(int i=1;i<dices.Length;i++)
        {
            if(oldRoll != results[i])
            {
                isDouble = false;
                break;
            }
        }

        return isDouble;
    }

    public int Roll(int index)
    {
        int result = 0;
        if(index < dices.Length && index >= 0)
        {
            result = dices[index].Roll();
        }
        return result;
    }
}
