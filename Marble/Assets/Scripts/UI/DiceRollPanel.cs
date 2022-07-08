using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DiceRollPanel : MonoBehaviour, IPointerClickHandler
{
    CanvasGroup canvasGroup;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Show(bool isShow)
    {
        if( isShow )
        {
            canvasGroup.alpha = 1;
        }
        else
        {
            canvasGroup.alpha = 0;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //Debug.Log("DiceRollPanel");
        GameManager.Inst.TurnManager.PlayerTurnProcess();
    }
}
