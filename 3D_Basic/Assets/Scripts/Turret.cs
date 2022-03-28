using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public GameObject bullet = null;
    public Transform shotTransform = null;

    public float interval = 1.0f;
    public int shots = 5;
    public float rateOfFire = 0.1f;

    IEnumerator shotSave;

    private void Start()
    {
        shotSave = Shot();
        StartCoroutine(shotSave);    
    }

    IEnumerator Shot()
    {
        while (true)
        {
            yield return new WaitForSeconds(interval - shots * rateOfFire); //1초-0.1초*5 대기
            for (int i = 0; i < shots; i++)
            {
                Instantiate(bullet, shotTransform);
                yield return new WaitForSeconds(rateOfFire);    //0.1초 대기
            }
        }
    }
}
