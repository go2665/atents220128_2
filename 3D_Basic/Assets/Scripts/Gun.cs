using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject bullet = null;        // 발사할 총알 오브젝트
    public Transform shotTransform = null;  // 총알이 발사될 위치

    public float interval = 1.0f;           // 총알을 발사하는 간격(방아쇠를 당기는 간격)
    public int shots = 5;                   // 한번 발사할때 몇연사를 할 것인가
    public float rateOfFire = 0.1f;         // 연사를 할 때 발사되는 간격

    private IEnumerator shot = null;
    private bool isShoot = false;

    private void Awake()
    {
        shot = Shot();
    }

    public void Initialize(float _interval, int _shots, float _rateOfFire)
    {
        this.interval = _interval;
        this.shots = _shots;
        this.rateOfFire = _rateOfFire;
    }

    public void StartFire()
    {
        if (isShoot == false)
        {
            StartCoroutine(shot);
            isShoot = true;
        }
    }

    public void StopFire()
    {
        StopCoroutine(shot);
        isShoot = false;
    }

    IEnumerator Shot()
    {
        while (true)
        {
            yield return new WaitForSeconds(interval - shots * rateOfFire); // 1초-0.1초*5 대기
            // 총알 연사 시작
            for (int i = 0; i < shots; i++)
            {
                Instantiate(bullet, shotTransform.position, shotTransform.rotation);    // 총알 생성

                yield return new WaitForSeconds(rateOfFire);    // 0.1초 대기
            }
        }
    }
}
