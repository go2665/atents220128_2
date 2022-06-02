using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell_Cluster : Shell
{
    public float lifeTime = 1.0f;
    public float upPower = 20.0f;

    Shell_Submunition[] submunitions = null;

    protected override void Awake()
    {
        base.Awake();
        submunitions = GetComponentsInChildren<Shell_Submunition>();
        foreach(Shell_Submunition sub in submunitions)
        {
            sub.gameObject.SetActive(false);
        }
    }

    protected override void Start()
    {
        base.Start();
        //Time.timeScale = 0.2f;

        StartCoroutine(TimeOut());
    }

    // 포탄이 위로 날아간다.
    private void FixedUpdate()
    {
        rigid.AddForce(Vector3.up * upPower);     // 계속 위쪽으로 힘을 가하고
        rigid.MoveRotation(Quaternion.LookRotation(rigid.velocity));    // 이동 방향을 바라보도록 수정
    }

    protected override void Explotion(Vector3 up)
    {
        foreach(Shell_Submunition sub in submunitions)
        {
            sub.transform.parent = null;
            sub.gameObject.SetActive(true);            
            sub.RandomSpread(-up);
        }
        base.Explotion(up);
    }

    IEnumerator TimeOut()
    {
        yield return new WaitForSeconds(lifeTime);
        Explotion(Vector3.up);
    }
}
