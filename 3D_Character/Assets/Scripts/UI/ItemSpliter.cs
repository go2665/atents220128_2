using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSpliter : MonoBehaviour
{
    /// <summary>
    /// 아이템을 몇개로 나눌지 결정하는 변수
    /// </summary>
    public int itemSplitCount = 1;
    public int ItemSplitCount
    {
        get => itemSplitCount;
        set
        {
            itemSplitCount = value;
            itemSplitCount = Mathf.Max(1, itemSplitCount);  // 최소값은 1이고 
            if (slot != null)
            {
                itemSplitCount = Mathf.Min(itemSplitCount, slot.ItemCount - 1); // 최대값은 슬롯에 들어있는 아이템 수 - 1
            }
            inputField.text = itemSplitCount.ToString();    // 인풋 필드의 글자 변경
        }
    }
    private ItemSlot slot = null;
    private InputField inputField = null;
    private SplittedItem splittedItem = null;

    private void Awake()
    {
        // 각종 UI들을 찾아서 이벤트 연결
        inputField = transform.Find("InputField").GetComponent<InputField>();
        inputField.onValueChanged.AddListener(OnInputChage);
        Button increase = transform.Find("Button_Increase").GetComponent<Button>();
        increase.onClick.AddListener(IncreaseCount);
        Button decrease = transform.Find("Button_Decrease").GetComponent<Button>();
        decrease.onClick.AddListener(DecreaseCount);
        Button ok = transform.Find("Button_OK").GetComponent<Button>();
        ok.onClick.AddListener(SelectOK);
        Button cancel = transform.Find("Button_Cancel").GetComponent<Button>();
        cancel.onClick.AddListener(SelectCancel);
        splittedItem = transform.parent.GetComponentInChildren<SplittedItem>();
    }

    private void Start()
    {
        this.gameObject.SetActive(false);   // 시작할 때 자동으로 꺼지기
    }

    private void OnEnable()
    {
        ItemSplitCount = 1; // 활성화 할 때 무조건 1로 시작
    }

    void OnInputChage(string change)
    {
        ItemSplitCount = int.Parse(change); // 입력받은 문자를 숫자로 변경해서 저장
    }

    /// <summary>
    /// 오른쪽 화살표 버튼 클릭시 실행
    /// </summary>
    void IncreaseCount()
    {
        ItemSplitCount++;   
    }

    /// <summary>
    /// 왼쪽 화살표 버튼 클릭시 실행
    /// </summary>
    void DecreaseCount()
    {
        ItemSplitCount--;   
    }

    /// <summary>
    /// OK 버튼을 눌렀을 때 실행될 함수
    /// </summary>
    void SelectOK()
    {
        splittedItem.Open(slot, itemSplitCount);    // splittedItem에 아이템을 분리하는 슬롯과 분리하는 갯수 넘겨주며 열기
        this.gameObject.SetActive(false);           // 열었으면 자신은 닫는다.
    }

    /// <summary>
    /// Cancel 버튼을 눌렀을 때 실행될 함수
    /// </summary>
    void SelectCancel()
    {
        slot = null;                        // 들어있던 값을 비우고 
        this.gameObject.SetActive(false);   // 자신을 닫는다.
    }

    /// <summary>
    ///  슬롯을 쉬프트 클릭해서 ItemSpliter를 열때 실행되는 함수
    /// </summary>
    /// <param name="newSlot">쉬프트 클릭한 슬롯</param>
    public void Open(ItemSlot newSlot)
    {
        if (newSlot.ItemCount > 1)  // 아이템이 쪼갤 수 있는 갯수가 있을 때만 진행
        {
            slot = newSlot;         // slot에 대상 슬롯 설정하고 
            this.gameObject.SetActive(true);    // 창 열기
        }
    }

}
