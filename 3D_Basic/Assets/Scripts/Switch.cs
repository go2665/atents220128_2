using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour, IUseable
{
    public Door targetDoor = null;

    Transform bar = null;
    bool switchOn = false;
    const float angle = 15.0f;

    private void Awake()
    {
        bar = transform.Find("BarPivot");
    }

    // 누군가가 이 오브젝트를 사용할 때 실행되는 함수
    public void OnUse()
    {
        if(switchOn)    
        {
            // 지금 스위치가 켜져 있는데 스위치를 눌렀다 => 스위치를 끈다
            //switchOn = false;
            bar.rotation = Quaternion.Euler(-angle, 0, 0);  // 스위치 바를 off위치로 회전
            switchOn = targetDoor.Close();                  // 연결된 문을 닫았다.
        }
        else
        {
            // 지금 스위치가 꺼져 있는데 스위치를 눌렀다 => 스위치를 켠다
            //switchOn = true;
            bar.rotation = Quaternion.Euler(angle, 0, 0);   // 스위치 바를 on위치로 회전
            switchOn = targetDoor.Open();                   // 연결된 문을 열었다.
        }
    }
}
