using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public int hp = 0;
    public int maxHP = 3;
    public float moveSpeed = 1.0f;
    
    // 자식으로 생성할 오브젝트 관련 정보
    public MemoryPool childPool = null;
    public uint childCount = 2;
    public float splashAngle = 90.0f;
    

    Rigidbody2D rigid = null;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();        
    }

    private void OnEnable()
    {
        hp = maxHP;
        rigid.velocity = -this.transform.right * moveSpeed;
        rigid.angularVelocity = Random.Range(-360.0f, 360.0f); ;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if( !collision.gameObject.CompareTag("Border")
            && !collision.gameObject.CompareTag("Asteroid")
            && !collision.gameObject.CompareTag("Enemy") )
        {
            hp--;
            if(hp <= 0)
            {
                SpawnChild();
                this.gameObject.SetActive(false);
            }
        }
    }

    void SpawnChild()
    {
        if(childPool != null && childCount > 0)
        {
            if (childCount > 1)
            {
                float startAngle = -splashAngle * 0.5f;             //첫번째 자식의 각도
                float angleGap = splashAngle / (childCount - 1);    //자식들 간의 각도 간격
                for (int i = 0; i < childCount; i++)
                {
                    childPool.GetObject(this.transform.position, Quaternion.Euler(0, 0, startAngle + angleGap * i));
                }
            }
            else
            {
                childPool.GetObject(this.transform.position, Quaternion.identity);
            }
        }
    }
}
