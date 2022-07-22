using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class GoldenKeyDrawPanel : MonoBehaviour, IPointerClickHandler
{
    CanvasGroup mainCanvasGroup;
    CanvasGroup subCanvasGroup; //GoldenKeyResult의 캔버스 그룹
    TextMeshProUGUI cardName;
    TextMeshProUGUI cardDescription;

    GoldenKeyManager goldenKeyManager;
    Player goldenKeyPicker;
    GoldenKeyType drawCard;
    int clickCount = 0;

    System.Action OnClose;

    private void Awake()
    {
        mainCanvasGroup = GetComponent<CanvasGroup>();
        Transform temp = transform.Find("GoldenKeyResult");
        subCanvasGroup = temp.GetComponent<CanvasGroup>();
        cardName = temp.GetChild(0).GetComponent<TextMeshProUGUI>();
        cardDescription = temp.GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        goldenKeyManager = GameManager.Inst.GoldenKeyManager;
    }

    public void Show(bool isShow, Player player, System.Action onClose)
    {
        if (isShow)
        {
            clickCount = 0;
            mainCanvasGroup.alpha = 1;
            mainCanvasGroup.interactable = true;
            mainCanvasGroup.blocksRaycasts = true;
            subCanvasGroup.alpha = 0;
            goldenKeyPicker = player;
            OnClose = onClose;
        }
        else
        {
            mainCanvasGroup.alpha = 0;
            mainCanvasGroup.interactable = false;
            mainCanvasGroup.blocksRaycasts = false;
            subCanvasGroup.alpha = 0;
            cardName.text = "";
            cardDescription.text = "";
            goldenKeyPicker = null;
            OnClose?.Invoke();
            OnClose = null;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {        
        if( clickCount == 0 )
        {
            // 카드를 뽑아 이름과 설명을 출력

            drawCard = goldenKeyManager.DrawCard();
            Debug.Log($"{goldenKeyManager.GetCardName(drawCard)} 카드를 뽑았다.");
            cardName.text = goldenKeyManager.GetCardName(drawCard);
            cardDescription.text = goldenKeyManager.GetCardDescription(drawCard);

            subCanvasGroup.alpha = 1;
        }
        else
        {
            // 뽑은 카드 적용
            Debug.Log($"{goldenKeyManager.GetCardName(drawCard)} 카드를 적용한다.");
            goldenKeyManager.RunGoldenCard(drawCard, goldenKeyPicker);
            Show(false, null, null);
        }        

        clickCount++;
    }
}
