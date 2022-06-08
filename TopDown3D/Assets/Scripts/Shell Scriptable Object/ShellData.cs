using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Shell Data", menuName = "Scriptable Object/Shell Data", order = 0)]
public class ShellData : ScriptableObject
{
    /// <summary>
    /// 초기 속도
    /// </summary>
    public float initialSpeed = 20.0f;

    /// <summary>
    /// 발사 쿨타임
    /// </summary>
    public float coolTime = 1.0f;

    /// <summary>
    /// 터질 때 이펙트
    /// </summary>
    public GameObject explosionEffect = null;

    public float damage = 10.0f;

    /// <summary>
    /// 폭팔용 함수
    /// </summary>
    /// <param name="objTransform">이 스크립터블 오브젝트를 가지고 있는 오브젝트의 트랜스폼</param>
    /// <param name="up">터질 지점의 노멀벡터</param>
    public virtual void Explosion(Transform objTransform, Vector3 up)
    {
        GameObject effect = Instantiate(explosionEffect);       // 이팩트 생성
        effect.transform.position = objTransform.position;      // 이팩트 위치 옮기기

        // Quaternion.AngleAxis(-90.0f, transform.right) : 이펙트 자체가 회전되어 있어서 추가로 각도 맞춤
        // Quaternion.LookRotation(forward, collision.contacts[0].normal) : 충돌한 면의 노멀백터가 up백터가 되는 회전 만들기
        Vector3 forward = Quaternion.AngleAxis(90.0f, objTransform.right) * up;
        effect.transform.rotation =
            Quaternion.AngleAxis(-90.0f, objTransform.right) * Quaternion.LookRotation(forward, up);    // 이펙트의 기본 회전에 맞춰 추가 회전
    }
}
