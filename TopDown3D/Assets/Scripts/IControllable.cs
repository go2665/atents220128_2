using UnityEngine;

public interface IControllable
{
    /// <summary>
    /// 컨트롤러가 입력받은 이동 방향
    /// </summary>
    Vector3 KeyboardInputDir { set; }
}
