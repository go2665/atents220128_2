using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceSet : MonoBehaviour
{
    Dice[] dices;
    public int NumOfDices
    {
        get => dices.Length;
    }

    public System.Action<PlayerType> OnDouble;

    void Awake()
    {
        dices = FindObjectsOfType<Dice>();
    }

    /// <summary>
    /// 여러 주사위를 굴려 주사위 값의 합을 돌려줌
    /// </summary>
    /// <returns>모든 주사위값의 합</returns>
    public int RollAll_GetTotalSum(bool showDiceRotate = false)
    {
        int[] results = RollAll_GetIndividual(showDiceRotate);
        int sum = 0;
        foreach(int r in results)
        {
            sum += r;
        }

        return sum;
    }

    /// <summary>
    /// 여러 주사위를 굴려 각 주사위의 결과값을 돌려줌
    /// </summary>
    /// <returns>모든 주사위의 결과값</returns>
    public int[] RollAll_GetIndividual(bool showDiceRotate = false)
    {   
        int[] results = new int[dices.Length];
        for(int i=0;i<dices.Length;i++)
        {
            results[i] = dices[i].Roll(showDiceRotate);            
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

        if (isDouble)
        {
            OnDouble?.Invoke(GameManager.Inst.TurnManager.CurrentPlayer);
        }

        return results;
    }

    /// <summary>
    /// 주사위 하나 굴리기
    /// </summary>
    /// <param name="index">굴릴 주사위의 인덱스. 디폴트는 0</param>
    /// <returns>주사위를 굴린 결과</returns>
    public int Roll(int index = 0)
    {
        int result = 0;
        if(index < dices.Length && index >= 0)
        {
            result = dices[index].Roll();
        }
        return result;
    }
}
