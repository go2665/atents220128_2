using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    IHealth target = null;
    Image healthImage = null;

    public enum Mode
    {
        Normal = 0,
        Charging
    }

    private void Awake()
    {
        healthImage = transform.Find("Health").gameObject.GetComponent<Image>();
        target = transform.parent.parent.GetComponent<IHealth>();
        target.onHealthChange = Refresh;
        target.onDead = () => SetHealthBarMode(Mode.Charging);
        target.onResurrection = () => SetHealthBarMode(Mode.Normal);
    }

    private void Start()
    {
        SetHealthBarMode(Mode.Normal);
        transform.LookAt(transform.parent.position - transform.parent.up);
        transform.Translate(Vector3.forward * 4.3f, Space.World);
    }

    void Refresh()
    {
        healthImage.fillAmount = target.HP / target.MaxHP;
    }

    public void SetHealthBarMode(Mode mode)
    {
        switch (mode)
        {
            case Mode.Normal:
                // 일반 모드(데미지를 입는 상황)
                healthImage.color = Color.red;
                break;
            case Mode.Charging:
                // 충전 모드(복구되는 상황)
                healthImage.color = Color.green;
                break;
            default:
                healthImage.color = Color.red;
                break;
        }
    }
}
