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

    void Awake()
    {
        dices = FindObjectsOfType<Dice>();
    }

    /// <summary>
    /// 여러 주사위를 굴려 주사위의 합과 모두 일치하는지 여부를 돌려줌
    /// </summary>
    /// <param name="sum">출력용 파라메터. 모든 주사위값의 합이 기록됨</param>
    /// <returns>모든 주사위 값의 일치여부 true면 모든 주사위의 값이 같다.</returns>
    public bool RollAll_GetSum(out int sum)
    {
        bool isDouble = RollAll_GetIndividual(out int[] results);
        sum = 0;
        foreach(int r in results)
        {
            sum += r;
        }
        return isDouble;
    }

    /// <summary>
    /// 여러 주사위를 굴려 각 주사위의 결과값과 주사위 결과값이 모두 일치하는지 여부를 돌려줌
    /// </summary>
    /// <param name="results">출력용 파라메터. 모든 주사위값의 합이 기록됨</param>
    /// <returns>모든 주사위 값의 일치여부 true면 모든 주사위의 값이 같다.</returns>
    public bool RollAll_GetIndividual(out int[] results)
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
