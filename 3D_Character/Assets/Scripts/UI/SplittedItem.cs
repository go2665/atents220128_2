using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

/// <summary>
/// 지금 분리중인 아이템을 표시하기 위한 클래스
/// </summary>
public class SplittedItem : MonoBehaviour
{
    Image itemIcon = null;      // 아이콘 표시용 이미지
    Text itemCountText = null;  // 분리하는 아이템 개수 표시용 텍스트
    ItemSlot itemSlot = null;   // 분리작업을 시작한 슬롯
    public ItemSlot ItemSlot { get => itemSlot; }

    private int itemCount = 0;
    public int ItemCount 
    {
        get => itemCount;
        set
        {
            itemCount = value;
            itemCountText.text = itemCount.ToString();
        }
    }

    private void Awake()
    {
        itemIcon = GetComponentInChildren<Image>(); // 이미지 컴포넌트 찾기
        itemCountText = GetComponentInChildren<Text>(); //텍스트 컴포넌트 찾기
    }

    private void OnEnable()
    {
        if(itemSlot != null)    // start전에도 한번 호출이 일어나기 때문에 
        {
            itemIcon.sprite = itemSlot.SlotItem.itemImage;  // itemSlot에 데이터가 있으면 슬롯에 있는 스프라이트를 사용하여 표시
        }
    }

    private void Start()
    {
        this.gameObject.SetActive(false);   // 시작할 때 닫기
    }

    private void Update()
    {
        this.transform.position = Mouse.current.position.ReadValue();   // 항상 마우스 위치를 따라다니기
    }

    /// <summary>
    /// 이 게임 오브젝트를 활성화 할 때(표시할 때) 사용하는 함수. (반드시 이 함수로만 진행되어야 함)
    /// </summary>
    /// <param name="targetSlot">아이템을 분리하는 작업을 진행하는 슬롯</param>
    /// <param name="count">분리하는 개수</param>
    public void Open(ItemSlot targetSlot, int count)
    {
        this.transform.position = Mouse.current.position.ReadValue();   // 미리 마우스 위치로 옮겨놓기
        itemSlot = targetSlot;  // 대상 슬롯 저장
        ItemCount = count;      // 개수 저장
        this.gameObject.SetActive(true);    // 게임 오브젝트 활성화
    }

    public void Close()
    {
        itemSlot = null;                        // 널로 초기화
        this.gameObject.SetActive(false);       // 게임 오브젝트 비활성화
    }
}
