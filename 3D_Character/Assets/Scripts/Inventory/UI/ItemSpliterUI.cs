using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSpliterUI : MonoBehaviour
{
    // 변수들 -------------------------------------------------------------------------------------
    /// <summary>
    /// 아이템을 몇개로 나눌지 결정하는 변수
    /// </summary>
    public uint itemSplitCount = 1;

    /// <summary>
    /// 아이템을 나눌 대상 슬롯
    /// </summary>
    private ItemSlotUI slotUI = null;

    // 캐싱용 컴포넌트들 ---------------------------------------------------------------------------
    private InputField inputField = null;


    // 프로퍼티들 ----------------------------------------------------------------------------------
    /// <summary>
    /// 아이템을 몇개로 나눌지 결정하는 프로퍼티. set은 1~(가지고 있는 개수-1)로 자동변경된다.
    /// </summary>
    public uint ItemSplitCount
    {
        private get => itemSplitCount;
        set
        {
            itemSplitCount = value;
            itemSplitCount = (uint)Mathf.Max(1, itemSplitCount);    // 최소값은 1이고 
            if (slotUI != null)
            {
                itemSplitCount = (uint)Mathf.Min(itemSplitCount, slotUI.ItemCount - 1); // 최대값은 슬롯에 들어있는 아이템 수 - 1
            }
            inputField.text = itemSplitCount.ToString();            // 인풋 필드의 글자 변경
        }
    }


    // 델리게이트들 --------------------------------------------------------------------------------
    /// <summary>
    /// OK 버튼을 눌렀을 때 실행될 델리게이트
    /// </summary>
    public InventoryDelegate onSpliterOK = null;


    // 함수들 -------------------------------------------------------------------------------------
    /// <summary>
    ///  슬롯을 쉬프트 클릭해서 ItemSpliterUI 열때 실행되는 함수
    /// </summary>
    /// <param name="newSlotUI">쉬프트 클릭한 슬롯UI</param>
    public void Open(ItemSlotUI newSlotUI)
    {
        if( newSlotUI.ItemCount > 1 )
        {
            slotUI = newSlotUI;
            this.gameObject.SetActive(true);    // 창 열기
        }
    }

    // 유니티 이벤트 함수들 ------------------------------------------------------------------------
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
    }

    private void Start()
    {
        GameManager.Inst.InventoryUI.onInventoryClose += SelectCancel;  // UI 닫힐때 자동으로 캔슬
        this.gameObject.SetActive(false);                       // 시작할 때 자동으로 꺼지기
    }

    private void OnEnable()
    {
        ItemSplitCount = 1; // 활성화 할 때 무조건 1로 시작
    }


    // UI 이벤트 함수들 ----------------------------------------------------------------------------
    /// <summary>
    /// InputField의 글자가 바뀔 때 실행
    /// </summary>
    /// <param name="change"></param>
    void OnInputChage(string change)
    {
        ItemSplitCount = uint.Parse(change); // 입력받은 문자를 숫자로 변경해서 저장
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
        GameManager.Inst.InventoryUI.SpliteItem_Start((uint)slotUI.ID, ItemSplitCount); // 아이템 나누기 시작 함수 실행
        onSpliterOK?.Invoke();                      // MovingSlot 열기
        this.gameObject.SetActive(false);           // 열었으면 자신은 닫는다.
    }

    /// <summary>
    /// Cancel 버튼을 눌렀을 때 실행될 함수
    /// </summary>
    void SelectCancel()
    {
        slotUI = null;                      // 들어있던 값을 비우고 
        this.gameObject.SetActive(false);   // 자신을 닫는다.
    }
}
