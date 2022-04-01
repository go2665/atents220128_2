using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret_Base : MonoBehaviour
{
    public int additionalGunCount = 0;              // 추가될 총의 개수
    private Gun[] guns = null;                      // 터렛의 총들
    private Transform gunBase = null;               // 총들이 부모 transform
    private GameObject gunObject = null;            // 기본 총 오브젝트(복사될 원본)
    private Queue<GameObject> additionalGuns = new Queue<GameObject>(0);    // 추가로 생성된 총들의 목록

    protected void Initiallize()
    {
        gunBase = transform.Find("GunBase");            // 총이 자식으로 붙을 gunBase 찾기
        gunObject = gunBase.GetChild(0).gameObject;     // 복제될 원본 총 오브젝트 찾기
        AddGuns();
        InitializeGun(1.0f, 3, 0.1f);                   // 총의 특성 설정(발사 인터벌과 연사 등)
    }

    // additionalGunCount만큼 총을 추가해주는 함수
    private void AddGuns()
    {
        additionalGuns = new Queue<GameObject>(additionalGunCount); // 추가한 총을 저장할 콜렉션 생성

        for (int i = 0; i < additionalGunCount; i++)                // additionalGunCount만큼 반복
        {
            GameObject cloneGun = Instantiate(gunObject, gunBase);  // 총 오브젝트 클로닝하고 gunBase의 자식으로 추가
            cloneGun.transform.Translate(cloneGun.transform.up * (i + 1) * 0.2f); // 새로 추가한 총을 위로 붙임
            additionalGuns.Enqueue(cloneGun);                       // 큐 콜렉션에 총 추가
        }

        guns = gunBase.GetComponentsInChildren<Gun>();              // gunBase의 자식으로 있는 모든 총 찾기
    }

    // additionalGuns에 있는 모든 총을 삭제
    private void RemoveGuns()
    {
        while (additionalGuns.Count > 0)    // additionalGuns에 들어있는 것이 있으면 계속 실행
        {
            GameObject clonGun = additionalGuns.Dequeue();  // additionalGuns에 들어있는 것을 하나 꺼내고
            Destroy(clonGun);               // 삭제
        }
    }

    // 총의 발사 인터벌, 연사수, 연사간격 설정
    private void InitializeGun(float interval, int shots, float rateOfFire)
    {
        foreach (Gun gun in guns)   // guns에 들어있는 모든 총들을 초기화
        {
            gun.Initialize(interval, shots, rateOfFire);    // 총 특성 초기화
        }
    }

    // 터렛에 달린 총 발사
    public virtual void StartFire()
    {
        foreach (Gun gun in guns)   // guns에 있는 모든 총을 발사
        {
            gun.StartFire();
        }
    }

    // 터렛에 달린 총 발사 중지
    public virtual void StopFire()
    {
        foreach (Gun gun in guns)   // guns에 있는 모든 총 발사 중지
        {
            gun.StopFire();
        }
    }
}
