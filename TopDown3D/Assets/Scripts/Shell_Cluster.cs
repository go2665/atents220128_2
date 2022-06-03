using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell_Cluster : Shell
{
    Shell_Submunition[] submunitions = null;
    ShellData_Cluster clusterData = null;

    protected override void Awake()
    {
        base.Awake();
        submunitions = GetComponentsInChildren<Shell_Submunition>();
        foreach(Shell_Submunition sub in submunitions)
        {
            sub.gameObject.SetActive(false);
        }
        clusterData = data as ShellData_Cluster;
        onExplosion = (objTransform, up) => clusterData.Explosion(objTransform, up, submunitions);
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
        rigid.AddForce(Vector3.up * clusterData.upPower);     // 계속 위쪽으로 힘을 가하고
        rigid.MoveRotation(Quaternion.LookRotation(rigid.velocity));    // 이동 방향을 바라보도록 수정
    }    

    IEnumerator TimeOut()
    {
        yield return new WaitForSeconds(clusterData.lifeTime);
        onExplosion?.Invoke(transform, Vector3.up);
        Destroy(this.gameObject);
    }
}
