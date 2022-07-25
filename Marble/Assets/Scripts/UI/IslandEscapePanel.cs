using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 주사위 굴림판. 클릭하면 주사위를 굴림
/// </summary>
public class IslandEscapePanel : MonoBehaviour, IPointerClickHandler
{
    /// <summary>
    /// 자식 UI들의 알파값을 조절하기 위한 캔버스 그룹
    /// </summary>
    CanvasGroup canvasGroup;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    /// <summary>
    /// 주사위 굴림판을 보여줄지 결정하는 함수
    /// </summary>
    /// <param name="isShow">true면 보여준다.</param>
    public void Show(bool isShow)
    {
        if( isShow )
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
        }
    }

    /// <summary>
    /// IPointerClickHandler로 연결된 함수. 클릭할 때 실행된다.
    /// </summary>
    /// <param name="eventData">이벤트 데이터</param>
    public void OnPointerClick(PointerEventData eventData)
    {
        Show(false);    // 닫고

        Player player = GameManager.Inst.GetPlayer(PlayerType.Human);
        if( player.TryEscapeIsland() )
        {
            player.OnArriveIsland(0);
        }
        player.OnPanelClose();
    }
}
