using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using System;

public class SpaceShipPanel : MonoBehaviour
{
    PlayerInputActionMaps actions;

    TextMeshProUGUI availiableText;
    Button yesButton;
    Button noButton;

    string useAvailableStr = "사용료는 200 만원입니다.";
    string useNotAvailableStr = "사용료가 부족합니다.";

    Player passenger;

    /// <summary>
    /// 자식 UI들의 알파값을 조절하기 위한 캔버스 그룹
    /// </summary>
    CanvasGroup canvasGroup;

    CanvasGroup targetSelectCanvasGroup;

    void Awake()
    {
        actions = new PlayerInputActionMaps();
        actions.SpaceShip.Click.performed += OnDestinationClick;

        canvasGroup = GetComponent<CanvasGroup>();
        yesButton = transform.Find("YesButton").GetComponent<Button>();
        noButton = transform.Find("NoButton").GetComponent<Button>();
        yesButton.onClick.AddListener(OnClickYes);
        noButton.onClick.AddListener(OnClickNo);
        availiableText = transform.Find("AvailiableText").GetComponent<TextMeshProUGUI>();
        targetSelectCanvasGroup = transform.Find("TargetSelectPanel").GetComponent<CanvasGroup>();
    }    

    private void OnDisable()
    {
        actions.SpaceShip.Disable();
    }

    /// <summary>
    /// 우주왕복선 사용판을 보여줄지 결정하는 함수
    /// </summary>
    /// <param name="isShow">true면 보여준다.</param>
    public void Show(bool isShow, Player arrived)
    {        
        if (isShow)
        {
            canvasGroup.alpha = 1;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;

            if(Place_SpaceShip.shipUsePrice > arrived.Money)
            {
                // 사용 불가
                yesButton.interactable = false;
                availiableText.text = useNotAvailableStr;
            }
            else
            {
                // 사용 가능
                yesButton.interactable = true;
                availiableText.text = useAvailableStr;
            }
            passenger = arrived;
        }
        else
        {
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }        
    }

    void OnClickYes()
    {
        Debug.Log("사용합니다.");        

        // 탑승할 경우
        CityBase coumbia = GameManager.Inst.GameMap.GetPlace(MapID.Columbia) as CityBase;
        Player ownerPlayer = GameManager.Inst.GetPlayer(coumbia.Owner);

        passenger.Money -= Place_SpaceShip.shipUsePrice;
        ownerPlayer.Money += Place_SpaceShip.shipUsePrice;

        actions.SpaceShip.Enable();

        targetSelectCanvasGroup.alpha = 1;
        targetSelectCanvasGroup.blocksRaycasts = true;

        //PanelEnd();
    }

    private void OnDestinationClick(InputAction.CallbackContext context)
    {
        Vector2 screenPos = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        if (Physics.Raycast(ray, out RaycastHit hit,  10.0f, LayerMask.GetMask("Place")))
        {
            Place place = hit.collider.GetComponentInParent<Place>();
            passenger.Move(place.ID);
            
            Debug.Log($"{place.placeName}로 이동합니다.");
            PanelEnd();
        }
    }

    void OnClickNo()
    {
        Debug.Log("사용하지 않습니다.");
        passenger.PlayerTurnEnd();
        PanelEnd();
    }

    void PanelEnd()
    {
        targetSelectCanvasGroup.alpha = 0;
        targetSelectCanvasGroup.blocksRaycasts = false;

        actions.SpaceShip.Disable();
        passenger = null;
        Show(false, null);
    }
}
