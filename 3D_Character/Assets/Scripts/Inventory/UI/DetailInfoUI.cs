using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetailInfoUI : MonoBehaviour
{
    // 주요 데이터----------------------------------------------------------------------------------
    /// <summary>
    /// 표시할 아이템 데이터. Open 할 때만 설정 가능
    /// </summary>
    ItemData itemData = null;
    /// <summary>
    /// 디테일창 일시정지. 드래그 할 때 열리지 않도록하는 스위치
    /// </summary>
    bool pause = false;


    // 캐싱용 컴포넌트들----------------------------------------------------------------------------
    /// <summary>
    /// 아이템 이름용 텍스트
    /// </summary>
    Text itemName = null;
    /// <summary>
    /// 아이템 아이콘용 이미지
    /// </summary>
    Image itemIcon = null;
    /// <summary>
    /// 아이템 설명용 텍스트
    /// </summary>
    Text itemDesc = null;
    /// <summary>
    /// 아이템 가격용 텍스트
    /// </summary>
    Text itemPrice = null;
    

    // 함수들--------------------------------------------------------------------------------------
    /// <summary>
    /// 디테일 창을 여는 함수. pause 상태가 아닐 때만 동작한다.
    /// </summary>
    /// <param name="data">디테일 창이 표시할 아이템 정보</param>
    public void Open(ItemData data)
    {
        if (!pause)
        {
            itemData = data;
            Refresh();
            this.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// 디테일 창을 닫는 함수. pause 상태가 아닐 때만 동작한다.
    /// </summary>
    public void Close()
    {
        if (!pause)
        {
            this.gameObject.SetActive(false);
            itemData = null;
        }
    }

    /// <summary>
    /// UI 갱신용. itemData에 있는 값을 기준으로 갱신
    /// </summary>
    void Refresh()
    {
        if(itemData != null)
        {
            itemName.text = itemData.itemName;
            itemIcon.sprite = itemData.itemImage;
            itemDesc.text = itemData.itemDesc;
            itemPrice.text = $"가격 : {itemData.price} 골드";
        }
    }

    /// <summary>
    /// Pause 상태로 만든다. 게임 오브젝트를 비활성화 시키고 다른 public 함수들의 동작을 막는다.
    /// </summary>
    void Pause()
    {
        pause = true;
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// Pause 상태를 해제한다. 게임오브젝트를 활성화 시키고 다른 public 함수들이 동작 가능하게 한다. UI도 갱신한다.
    /// </summary>
    void Restart()
    {
        pause = false;
        if (itemData != null)
        {
            Refresh();      // pause 상태일 때의 itemData를 기준으로 갱신한다.
            this.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// 디테일 창 비우기 용도.
    /// </summary>
    void Clear()
    {
        itemData = null;
    }


    // 유니티 이벤트 함수들---------------------------------------------------------------------------------
    private void Awake()
    {
        // 하부 게임 오브젝트의 컴포넌트 찾기
        itemName = transform.Find("Name").GetComponent<Text>();
        itemIcon = transform.Find("Icon").GetComponent<Image>();
        itemDesc = transform.Find("Desc").GetComponent<Text>();
        itemPrice = transform.Find("Price").GetComponent<Text>();
    }

    private void Start()
    {
        GameManager.Inst.InventoryUI.onInventoryClose += Close;         // 디테일 창이 떠있을 때 인벤토리가 닫히면 같이 닫히게 수정
        GameManager.Inst.InventoryUI.onInventoryDragStart += Pause;     // 드래그가 시작되면 일시 정지
        GameManager.Inst.InventoryUI.onInventoryDragEnd += Restart;     // 드래그가 끝나면 다시 동작
        GameManager.Inst.InventoryUI.onInventoryDragOutEnd += Clear;    // 드래그가 인벤토리 밖에서 끝나면 itemData비워서 안보이게 하기
        GameManager.Inst.InventoryUI.onInventorySplittingStart += Pause;// 아이템을 나누기 시작하면 일시 정지
        GameManager.Inst.InventoryUI.onInventorySplittingEnd += Restart;// 아이템을 나누는게 끝나면 다시 동작
        this.gameObject.SetActive(false);
    }
}
