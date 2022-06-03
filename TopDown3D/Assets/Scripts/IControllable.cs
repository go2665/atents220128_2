using UnityEngine;

public interface IControllable
{    
    /// <summary>
    /// 컨트롤러가 입력받은 이동 방향
    /// </summary>
    Vector3 KeyboardInputDir { set; }

    /// <summary>
    /// 마우스의 위치
    /// </summary>
    Vector2 MouseInputPosition { set; }

    /// <summary>
    /// 입력용 델리게이트 선언
    /// </summary>
    public delegate void InputActionDelegate();

    /// <summary>
    /// 마우스 왼쪽 버튼(일반공격)이 클릭되었을 때 실행될 델리게이트
    /// </summary>
    public InputActionDelegate onFireNormal { get; set; }

    /// <summary>
    /// 마우스 오른쪽 버튼(특수공격)이 클릭되었을 때 실행될 델리게이트
    /// </summary>
    public InputActionDelegate onFireSpecial { get; set; }

    /// <summary>
    /// 단축키 1번이 눌러졌을 때 실행될 델리게이트
    /// </summary>
    public InputActionDelegate onShortCut01 { get; set; }

    /// <summary>
    /// 단축키 2번이 눌러졌을 때 실행될 델리게이트
    /// </summary>
    public InputActionDelegate onShortCut02 { get; set; }
}
