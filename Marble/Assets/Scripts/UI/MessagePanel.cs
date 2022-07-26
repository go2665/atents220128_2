using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

/// <summary>
/// 메세지 패널
/// </summary>
public class MessagePanel : MonoBehaviour, IPointerClickHandler
{
    /// <summary>
    /// 자식 UI들의 알파값을 조절하기 위한 캔버스 그룹
    /// </summary>
    CanvasGroup canvasGroup;
    Player target;
    TextMeshProUGUI messageText;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        messageText = GetComponentInChildren<TextMeshProUGUI>();
    }

    /// <summary>
    /// 메세지 패널을 보여줄지 결정하는 함수
    /// </summary>
    /// <param name="isShow">true면 보여준다.</param>
    public void Show(bool isShow, Player targetPlayer, string message = "")
    {        
        if ( isShow )
        {
            targetPlayer.OnPanelOpen();
            messageText.text = message;
            canvasGroup.alpha = 1;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
        else
        {            
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            targetPlayer.OnPanelClose();
        }
        target = targetPlayer;
    }

    /// <summary>
    /// IPointerClickHandler로 연결된 함수. 클릭할 때 실행된다.
    /// </summary>
    /// <param name="eventData">이벤트 데이터</param>
    public void OnPointerClick(PointerEventData eventData)
    {
        Show(false, target);    // 닫고
    }
}
