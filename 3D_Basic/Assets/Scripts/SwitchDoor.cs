using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchDoor : Door
{
    public float closeDelayTime = 3.0f;     // 3초 기다린다는 것을 저장할 변수
    public Switch doorSwitch = null;        // 이 문을 열고 닫는 스위치

    public override bool Open()
    {
        //Debug.Log("Open");
        bool result = base.Open();          // Door의 Open함수 실행
        //timer = 0.0f;
        //opening = true;
        StartCoroutine(CloseDelay());       // 코루틴으로 문을 닫는 동작 실행

        return result;
    }

    IEnumerator CloseDelay()
    {
        yield return new WaitForSeconds(closeDelayTime);    //closeDelayTime만큼 대기
        doorSwitch.OnUse();                 // 스위치를 다시 한번 사용(문이 닫힌다)
        //Debug.Log("Close");
    }

    //float timer = 0.0f;
    //bool opening = false;

    //private void Update()
    //{
    //    timer += Time.deltaTime;
    //    if(opening && timer > closeDelayTime)
    //    {
    //        CloseDoor();
    //    }
    //}

    //void CloseDoor()
    //{
    //    doorSwitch.OnUse();
    //    opening = false;
    //}
}
