using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    IHealth target = null;
    Image healthImage = null;

    private void Awake()
    {
        healthImage = transform.Find("Health").gameObject.GetComponent<Image>();
        target = transform.parent.parent.GetComponent<IHealth>();
        target.onHealthChange = Refresh;
    }

    void Refresh()
    {
        healthImage.fillAmount = target.HP / target.MaxHP;
    }
}
