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

    //RaycastHit[] raycastHits = new RaycastHit[1];

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

    public IControllable.MouseClickAction onFireNormal { get; set; }
    public IControllable.MouseClickAction onFireSpecial { get; set; }

    private Rigidbody rigid = null;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        onFireNormal = FireNormal;
        onFireSpecial = FireSpecial;
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
        Debug.Log("Normal");
        GameObject obj = Instantiate(shells[0]);
        obj.transform.position = firePos.position;
        obj.transform.rotation = firePos.rotation;
    }

    /// <summary>
    /// 특수 공격. 마우스를 오른클릭했을 때 실행
    /// </summary>
    private void FireSpecial()
    {
        Debug.Log("Special");
    }
}
