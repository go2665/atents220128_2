using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void PickupDelegate();
public delegate void InvenOnOffDelegate();

public enum MoveMode : byte
{
    WALK = 0,
    RUN
}

public class Player : MonoBehaviour, IControllable, IBattle, IHealth, IMana, IEquippableCharacter
{
    // 이동용 데이터
    public float walkSpeed = 3.0f;
    public float runSpeed = 6.0f;
    public float turnSpeed = 5.0f;    

    // 장비 데이터
    public GameObject weaponAttachTarget = null;
    public GameObject shield = null;
    Weapon myWeapon = null;
    ItemData_Weapon equipItem = null;
    public ItemData_Weapon EquipItem { get => equipItem; }

    // 공격용 데이터
    public float attackPower = 15.0f;
    public float critical = 0.2f;
    public bool IsAttack { get; set; }

    // 방어용 데이터
    private float defencePower = 10.0f;
    private float hp = 70.0f;
    private float maxHP = 100.0f;
    public float HP
    {
        get => hp;
        set
        {
            hp = Mathf.Clamp(value, 0, MaxHP);
            onHealthChange?.Invoke();
        }
    }
    public float MaxHP { get => maxHP; }
    public HealthDelegate onHealthChange { get; set; }

    // 락온용 데이터
    public GameObject lockOnEffect = null;
    private Transform lockOnTarget = null;
    private float lockOnRange = 5.0f;

    // 아이템 사용
    public PickupDelegate onPickupAction = null;

    // 인벤토리
    private Inventory inven = null;
    public Inventory Inven { get => inven; }
    public InvenOnOffDelegate onInventoryOnOff = null;

    // 마나
    private float mp = 50.0f;
    private float maxMP = 100.0f;
    public float MP
    {
        get => mp;
        set
        {
            mp = Mathf.Clamp(value, 0, MaxMP);
            onManaChange?.Invoke();
        }
    }
    public float MaxMP { get => maxMP; }
    public ManaDelegate onManaChange { get; set; }
    public bool UseRigidbody { get => false; }

    public int invenSlotCount = 8;

    // 기타 데이터
    private Animator anim = null;
    private CharacterController controller = null;
    private Vector3 inputDir = Vector2.zero;
    private Quaternion targetRotation = Quaternion.identity;
    private MoveMode moveMode = MoveMode.RUN;
    

    void Awake()
    {
        myWeapon = weaponAttachTarget.GetComponentInChildren<Weapon>();
        inven = new Inventory(invenSlotCount);
    }

    void Update()
    {
        if (myWeapon != null)
        {
            // 무기에 닿았던 적들 전부 데미지 주기
            while (myWeapon.HitTargetCount() > 0)
            {
                IBattle target = myWeapon.GetHitTarget();
                Attack(target);
            }
        }

        // 록온을 했을 때 대상을 계속 바라보기
        if(lockOnTarget != null)
        {
            //this.transform.LookAt(lockOnTarget.position);

            Vector3 dir = lockOnTarget.position - this.transform.position;
            targetRotation = Quaternion.LookRotation(dir);
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
            inputDir.y = -9.8f;
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
            
            controller.Move(inputDir * speed * Time.deltaTime);
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

    public void ShowArms(bool isShow)
    {
        // equip이 true면 무기와 방패가 보인다. false 보이지 않는다.
        weaponAttachTarget.SetActive(isShow);
        shield.SetActive(isShow);
    }

    public void EquipWeapon(ItemData_Weapon weapon)
    {
        if (weapon != equipItem)   // 다른 무기를 장착하라고 하면 장착
        {
            equipItem = weapon;
            Instantiate(weapon.prefab, weaponAttachTarget.transform);
            myWeapon = weaponAttachTarget.GetComponentInChildren<Weapon>();
        }
    }

    public void UnEquipWeapon()
    {
        while( weaponAttachTarget.transform.childCount > 0 )
        {
            Transform del = weaponAttachTarget.transform.GetChild(0);
            del.parent = null;
            Destroy(del.gameObject);
        }
        equipItem = null;
        myWeapon = null;
    }

    public bool IsEquipWeapon()
    {
        return (weaponAttachTarget.transform.childCount > 0);  // 장비중이면 true, 아니면 false
    }

    public void AttackInput()
    {
        //Debug.Log("Attack");
        anim.SetFloat("ComboState",
            Mathf.Repeat(anim.GetCurrentAnimatorStateInfo(0).normalizedTime, 1.0f));
        anim.ResetTrigger("Attack");
        anim.SetTrigger("Attack");
    }

    public void LockOnInput()
    {
        //Debug.Log("LockOnInput");
        if( lockOnTarget == null )
        {
            LockOn();            
        }
        else
        {
            LockOff();
        }
    }

    void LockOn()
    {
        Collider[] cols = Physics.OverlapSphere(
            this.transform.position, lockOnRange, LayerMask.GetMask("Enemy"));
        // LayerMask.GetMask("Enemy")           //0b_1000000
        // 1<<LayerMask.NameToLayer("Enemy")    //0b_1000000    
        // LayerMask.GetMask("Enemy","Default","Water")  //0b_1010001
        // LayerMask.NameToLayer("Enemy")       //6이 리턴

        if( cols.Length > 0 )
        {
            // 가장 가까운 적을 찾는 코드 작성
            Collider nearest = null;
            float nearestDistance = float.MaxValue; 
            foreach(Collider col in cols)
            {
                float distance = (col.transform.position - this.transform.position).sqrMagnitude;
                if(distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearest = col;
                }
            }

            lockOnTarget = nearest.transform;
            lockOnEffect.transform.position = lockOnTarget.position;
            lockOnEffect.transform.parent = lockOnTarget;
            lockOnEffect.SetActive(true);
        }
    }

    void LockOff()
    {
        lockOnTarget = null;
        lockOnEffect.transform.parent = null;
        lockOnEffect.SetActive(false);
    }

    public void LockOff(Transform target)
    {
        if(target == lockOnTarget)
        {
            LockOff();
        }
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
        HP = hp;
    }

    public void PickupInput()
    {
        onPickupAction?.Invoke();
    }

    public void InventoryOnOffInput()
    {
        onInventoryOnOff?.Invoke();
    }
}
