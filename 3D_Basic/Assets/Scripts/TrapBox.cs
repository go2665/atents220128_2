using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapBox : MonoBehaviour, IUseable
{
    public ParticleSystem effect = null;        // Unity에서 특수효과를 사용하기 위한 컴포넌트
    public Light lightEffect = null;                  // 조명 컴포넌트

    IDead target = null;                        // 함정으로 죽일 대상

    private void OnEnable()
    {
        lightEffect.enabled = false;  // 함정이 발동되었을 때를 대비해 조명을 끄는 것
    }

    public void OnUse()         // IUseable 상속받았기 때문에 무조건 구현해야 함
    {
        if( effect != null && lightEffect != null )   // 특수효과와 조명이 설정되어있을 때 실행
        {
            effect.Play();              // 특수효과 재생
            lightEffect.enabled = true;       // 조명 켜기
        }
        if( target != null )    // 죽일 대상이 있다면
        {
            target.OnDead();    // 죽임
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        target = other.gameObject.GetComponent<IDead>();    // 함정 영역에 죽을 수 있는 대상이 들어오면 저장
    }

    private void OnTriggerExit(Collider other)
    {
        // 함정 영역에 죽을 수 있는 대상이 나가면 target을 비운다.
        if ( target == other.gameObject.GetComponent<IDead>() )
        {
            target = null;
        }
    }
}
