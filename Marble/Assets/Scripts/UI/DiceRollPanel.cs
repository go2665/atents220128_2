using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 주사위 굴림판. 클릭하면 주사위를 굴림
/// </summary>
public class DiceRollPanel : MonoBehaviour, IPointerClickHandler
{
    /// <summary>
    /// 자식 UI들의 알파값을 조절하기 위한 캔버스 그룹
    /// </summary>
    CanvasGroup canvasGroup;
    Player target;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    /// <summary>
    /// 주사위 굴림판을 보여줄지 결정하는 함수
    /// </summary>
    /// <param name="isShow">true면 보여준다.</param>
    public void Show(bool isShow, Player targetPlayer)
    {        
        if ( isShow )
        {
            canvasGroup.alpha = 1;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
        else
        {            
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            targetPlayer.StateChange(PlayerState.RollResult);
        }
        target = targetPlayer;
    }

    /// <summary>
    /// IPointerClickHandler로 연결된 함수. 클릭할 때 실행된다.
    /// </summary>
    /// <param name="eventData">이벤트 데이터</param>
    public void OnPointerClick(PointerEventData eventData)
    {
        //Debug.Log("DiceRollPanel");
        target.DiceRoll();
        Show(false, target);    // 닫고

        //Player player = GameManager.Inst.GetPlayer(PlayerType.Human);
        //player.MoveDiceRoll();
        //player.OnPanelClose();
    }
}
