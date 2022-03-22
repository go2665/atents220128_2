using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// 이 스크립트를 가진 게임오브젝트는 무조건 Animator가 있다(없으면 만든다)
[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour, IDead
{
    /// 9시 45분까지 구현해보기(Input System 사용)
    /// WASD로 캐릭터 움직이기
    /// W : 전진
    /// S : 후진
    /// A : 좌회전
    /// D : 우회전
    /// ㅁㄴㅇㅁㄴㅇㅁㄴㅇ

    public float moveSpeed = 5.0f;      // 플레이어 이동 속도(기본값 1초에 5)
    public float spinSpeed = 360.0f;    // 플레이어 회전 속도(기본값 1초에 한바퀴)
    private float spinInput = 0.0f;     // 회전 입력 여부(-1.0 ~ 1.0)
    private float moveInput = 0.0f;     // 앞뒤 입력 여부(-1.0 ~ 1.0)

    private Animator anim = null;       // 애니메이터 컴포넌트

    private PlayerControls pc = null;   // 입력 처리용 클래스
    private Rigidbody rigid = null;

    private bool isAlive = true;

    // 오브젝트가 만들어진 직후에 실행
    private void Awake()
    {
        anim = GetComponent<Animator>();    // 애니메이터 컴포넌트 찾아서 보관
        rigid = GetComponent<Rigidbody>();
        pc = new PlayerControls();          // Input Action Asset을 이용해 자동 생성한 클래스
        // PlayerDefault라는 액션맵에 있는 UseItem 액션이 starte일 때 UseItem 함수 실행하도록 바인딩
        pc.PlayerDefault.UseItem.started += UseItem;    
    }

    // 게임 오브젝트가 활성화 될 때 실행
    private void OnEnable()
    {
        pc.PlayerDefault.Enable();      //액션 맵도 함께 활성화
    }

    // 게임 오브젝트가 비활성화 될 때 실행
    private void OnDisable()
    {
        pc.PlayerDefault.Disable();     //액션 맵도 함께 비활성화
    }

    //// 매 프레임마다
    //private void Update()
    //{
    //    // 이동 처리 (1초에 moveSpeed만큼 moveInput쪽 방향(앞or뒤)으로 이동)
    //    transform.Translate(Vector3.forward * moveInput * moveSpeed * Time.deltaTime);

    //    // 회전 처리 (1초에 spinSpeed만큼 spinInput쪽 방향(우or좌)로 회전)
    //    transform.Rotate(Vector3.up, spinInput * spinSpeed * Time.deltaTime);
    //}

    // Time.fixedDeltaTime 간격으로 실행
    private void FixedUpdate()
    {
        // 현재 위치 + 캐릭터가 바라보는 방향으로 1초에 moveSpeed씩 이동
        rigid.MovePosition(
            rigid.position +
            transform.forward * moveInput * moveSpeed * Time.fixedDeltaTime);

        // 현재 각도 * 추가각도
        rigid.MoveRotation(
            rigid.rotation *
            Quaternion.AngleAxis(spinInput * spinSpeed * Time.fixedDeltaTime, Vector3.up));
    }

    // WASD를 눌렀을 때 실행될 함수
    public void Move(InputAction.CallbackContext context)
    {
        if (isAlive)
        {
            Vector2 input = context.ReadValue<Vector2>(); // 입력값을 받아서 회전 정도랑 이동 정도를 받아옴
            spinInput = input.x;    //A(1) D(-1)
            moveInput = input.y;    //W(1) S(-1)

            if (context.started)
            {
                anim.SetBool("IsMove", true);   // 누른 직후에 이동 애니메이션 시작
            }
            if (context.canceled)
            {
                anim.SetBool("IsMove", false);  // 키를 땠을 때 Idle 애니메이션 시작
            }
            //if( moveInput != 0.0f )           // 회전만 할 때는 이동 애니메이션이 안나오게 하고 싶을 때
            //{
            //    anim.SetBool("IsMove", true);
            //}
            //else
            //{
            //    anim.SetBool("IsMove", false);
            //}
        }
    }

    // 스페이스 키를 눌렀을 때 실행될 함수
    public void UseItem(InputAction.CallbackContext context)
    {
        //Debug.Log("UseItem");
        anim.SetTrigger("OnUseItem");   // 스페이스 키를 눌렀을 때 트리거 실행        
    }

    public void OnDead()
    {
        if (isAlive)
        {
            Debug.Log("플레이어 사망");

            // 사망 연출(죽으면 플레이어는 뒤로 넘어진다. rigidbody 사용)        
            rigid.constraints = RigidbodyConstraints.None;      // 회전과 이동 묶어두었던 것을 풀기
            rigid.drag = 0;             // 마찰력 기본값으로 복구
            rigid.angularDrag = 0.05f;
            //rigid.AddForce(-transform.forward * 3.0f);    // 플레이어의 뒤쪽방향으로 힘을 가하기
            rigid.AddForceAtPosition(-transform.forward * 3.0f, transform.position + new Vector3(0, 1.5f, 0));

            CapsuleCollider collider = GetComponent<CapsuleCollider>();
            collider.radius = 0.2f; // 충돌영역 반지름 줄이기
            //Destroy(collider, 5.0f);

            // 중복 사망 방지
            isAlive = false;

            // 죽었을 때 이동 처리 안하기 위한 처리
            spinInput = 0.0f;
            moveInput = 0.0f;
            anim.SetBool("IsMove", false);
        }
    }
}
