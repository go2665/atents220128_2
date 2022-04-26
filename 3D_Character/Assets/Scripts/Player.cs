using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveMode : byte
{
    WALK = 0,
    RUN
}

public class Player : MonoBehaviour, IControllable, IBattle
{
    // 이동용 데이터
    public float walkSpeed = 3.0f;
    public float runSpeed = 6.0f;
    public float turnSpeed = 0.3f;

    // 장비 데이터
    public GameObject weapone = null;
    public GameObject shield = null;
    Weapon myWeapon = null;

    // 공격용 데이터
    private float attackPower = 15.0f;
    private float critical = 0.2f;
    public bool IsAttack { get; set; }

    // 방어용 데이터
    private float defencePower = 10.0f;
    private float hp = 100.0f;
    private float maxHP = 100.0f;

    // 기타 데이터
    private Animator anim = null;
    private CharacterController controller = null;
    private Vector3 inputDir = Vector2.zero;
    private Quaternion targetRotation = Quaternion.identity;
    private MoveMode moveMode = MoveMode.RUN;

    void Awake()
    {
        myWeapon = weapone.GetComponentInChildren<Weapon>();
    }

    void Update()
    {
        // 무기에 닿았던 적들 전부 데미지 주기
        while(myWeapon.HitTargetCount() > 0)
        {
            IBattle target = myWeapon.GetHitTarget();
            Attack(target);
        }
    }
    
    public void ControllerConnect()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    public void MoveInput(Vector2 dir)
    {
        // dir.x : a(-1) d(+1)
        // dir.y : s(-1) w(+1)
        inputDir.x = dir.x;
        inputDir.y = 0;
        inputDir.z = dir.y;
        inputDir.Normalize();

        if (inputDir.sqrMagnitude > 0.0f)
        {
            inputDir = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0) * inputDir;            
            targetRotation = Quaternion.LookRotation(inputDir); 
        }

    }

    public void MoveUpdate()
    {
        if (inputDir.magnitude > 0.0f)
        {
            float speed = 1.0f;
            if (moveMode == MoveMode.WALK)
            {
                anim.SetFloat("Speed", 0.5f);
                speed = walkSpeed;
            }
            else if (moveMode == MoveMode.RUN)
            {
                anim.SetFloat("Speed", 1.0f);
                speed = runSpeed;
            }
            else
            {
                anim.SetFloat("Speed", 0.0f);
            }
            
            controller.SimpleMove(inputDir * speed);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }
        else
        {
            anim.SetFloat("Speed", 0.0f);
        }        
    }

    public void MoveModeChange()
    {
        if( moveMode == MoveMode.WALK )
        {
            moveMode = MoveMode.RUN;
        }
        else
        {
            moveMode = MoveMode.WALK;
        }
    }

    public void ArmsEquip(bool equip)
    {
        // equip이 true면 무기와 방패가 보인다. false 보이지 않는다.
        weapone.SetActive(equip);
        shield.SetActive(equip);
    }

    public void AttackInput()
    {
        //Debug.Log("Attack");
        anim.SetFloat("ComboState",
            Mathf.Repeat(anim.GetCurrentAnimatorStateInfo(0).normalizedTime, 1.0f));
        anim.ResetTrigger("Attack");
        anim.SetTrigger("Attack");        
    }

    public void Attack(IBattle target)
    {
        if (target != null)
        {
            float damage = attackPower;
            if (Random.Range(0.0f, 1.0f) < critical)
            {
                damage *= 2.0f;
            }
            target.TakeDamage(damage);
        }
    }

    public void TakeDamage(float damage)
    {
        //Debug.Log($"{gameObject.name} : {damage} 데미지 입음");
        float finalDamage = damage - defencePower;
        if (finalDamage < 1.0f)
        {
            finalDamage = 1.0f;
        }
        hp -= finalDamage;
        if (hp <= 0.0f)
        {
            //Die();
        }
        hp = Mathf.Clamp(hp, 0.0f, maxHP);
    }
}
