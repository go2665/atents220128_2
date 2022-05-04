using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    // 이동 관련
    public float speed = 10.0f;
    Vector2 inputDirection = Vector2.zero;
    Rigidbody2D rigid = null;
    
    // 애니메이터 관련
    Animator anim = null;
    readonly int hashState = Animator.StringToHash("State");

    // 공격 관련
    public GameObject shoot = null;
    public Transform fireTransform = null;
    GameObject flash = null;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        flash = transform.Find("Flash").gameObject;
    }

    //private void Update()
    //{
    //    //transform.Translate(inputDirection * speed * Time.deltaTime);
    //}

    private void FixedUpdate()
    {
        rigid.MovePosition((Vector2)transform.position + inputDirection * speed * Time.fixedDeltaTime);
    }

    public void MoveInput(InputAction.CallbackContext context)
    {
        inputDirection = context.ReadValue<Vector2>();
        //anim.SetFloat("InputY", inputDirection.y);
        if (inputDirection.y > 0.0f)
        {
            anim.SetInteger(hashState, 1);
        }
        else if (inputDirection.y < 0.0f)
        {
            anim.SetInteger(hashState, 2);
        }
        else
        {
            anim.SetInteger(hashState, 0);
        }
    }

    public void FireInput(InputAction.CallbackContext context)
    {
        if( context.started )
        {
            Fire();
        }
    }

    private void Fire()
    {
        //Instantiate(shoot, fireTransform.position, fireTransform.rotation);
        GameObject shootObj = GameManager.Inst.GetShootObject();      // 게임메니저 추가하면서 수정해야 함.
        shootObj.transform.position = fireTransform.position;
        flash.SetActive(true);
        StartCoroutine(FlashOff());
    }

    IEnumerator FlashOff()
    {
        yield return new WaitForSeconds(0.1f);
        flash.SetActive(false);
    }
}
