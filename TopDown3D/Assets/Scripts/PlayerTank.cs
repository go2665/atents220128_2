using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTank : MonoBehaviour, IControllable
{
    public float moveSpeed = 5.0f;
    public float spinSpeed = 20.0f;
    public Transform turret = null;
    public Transform firePos = null;
    public GameObject[] shells = null;

    FireData[] fireDatas = null;

    private Vector3 inputDir = Vector3.zero;
    private Vector2 mousePos = Vector2.zero;

    public Vector3 KeyboardInputDir
    {
        set
        {
            inputDir = value;
            //Debug.Log($"Input : {inputDir}");
        }
    }

    public Vector2 MouseInputPosition
    {
        set
        {
            mousePos = value;
            //Debug.Log($"MouseInput : {mousePos}");
        }
    }

    public IControllable.InputActionDelegate onFireNormal { get; set; }
    public IControllable.InputActionDelegate onFireSpecial { get; set; }
    public IControllable.InputActionDelegate onShortCut01 { get; set; }
    public IControllable.InputActionDelegate onShortCut02 { get; set; }

    private Rigidbody rigid = null;    

    ShellType selectedSpecialShell = ShellType.Cluster;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        onFireNormal = FireNormal;
        onFireSpecial = FireSpecial;
        onShortCut01 = ShortCut1;
        onShortCut02 = ShortCut2;
    }

    void Start()
    {
        fireDatas = new FireData[shells.Length];
        for(int i=0; i<shells.Length; i++)
        {
            fireDatas[i] = new FireData(shells[i], shells[i].GetComponent<Shell>().data);
        }
    }

    void Update()
    {
        foreach(FireData fire in fireDatas)
        {
            fire.DecreaseCoolTime(Time.deltaTime);
        }
    }

    void FixedUpdate()
    {
        Move();
        TurnTurret();
    }

    private void Move()
    {
        rigid.AddForce(inputDir.y * transform.forward * moveSpeed); // 리지드바디에 힘 추가
        rigid.AddTorque(0, inputDir.x * spinSpeed, 0);              // 리지드바디에 회전력 추가
    }

    private void TurnTurret()
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePos);   // 레이 생성
        //if (Physics.RaycastNonAlloc(ray, raycastHits, 30.0f, LayerMask.GetMask("Ground")) > 0)        //레이캐스팅으로 구하는 방법
        //{
        //    Vector3 look = raycastHits[0].point - transform.position;   // 터렛이 바라봐야 할 방향
        //    turret.rotation = Quaternion.Slerp(turret.rotation, Quaternion.LookRotation(look), 0.5f);
        //}

        Vector3 targetPosition = new Vector3(ray.origin.x, transform.position.y, ray.origin.z);
        Vector3 look = targetPosition - transform.position;
        turret.rotation = Quaternion.Slerp(turret.rotation, Quaternion.LookRotation(look), 0.05f);
    }

    /// <summary>
    /// 일반 공격. 마우스를 왼클릭 했을 때 실행
    /// </summary>
    private void FireNormal()
    {
        //Debug.Log("Normal");
        Fire(ShellType.Normal);
    }

    /// <summary>
    /// 특수 공격. 마우스를 오른클릭했을 때 실행
    /// </summary>
    private void FireSpecial()
    {
        //Debug.Log("Special");
        Fire(selectedSpecialShell);
    }

    private void Fire(ShellType type)
    {
        FireData data = fireDatas[(int)type];
        if(data.IsFireReady)
        {
            Debug.Log("Fire");
            GameObject obj = Instantiate(shells[(int)type]);
            obj.transform.position = firePos.position;
            obj.transform.rotation = firePos.rotation;

            data.ResetCoolTime();
        }        
    }

    private void ShortCut1()
    {
        selectedSpecialShell = ShellType.Cluster;
    }

    private void ShortCut2()
    {
        selectedSpecialShell = ShellType.BadEffect;
    }
}
