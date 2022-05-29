using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    // 플레이어의 HP 변경에 따라 slider의 value 수정
    // 플레이어의 HP 변경에 따라 text의 글자 수정

    Slider healthSlider = null;
    Text healthText = null;
    IHealth target = null;

    private void Awake()
    {
        healthSlider = GetComponentInChildren<Slider>();
        healthText = GetComponentInChildren<Text>();
    }

    private void Start()
    {
        target = GameManager.Inst.MainPlayer as IHealth;
        if (target != null && target.onHealthChange == null)
        {
            target.onHealthChange += RefreshSlider;
            target.onHealthChange += RefreshText;
            RefreshSlider();
            RefreshText();
        }
    }

    private void OnEnable()
    {
        if (target != null)
        {
            target.onHealthChange += RefreshSlider;
            target.onHealthChange += RefreshText;
        }
    }

    private void OnDisable()
    {
        if (target != null && target.onHealthChange != null)
        {
            target.onHealthChange -= RefreshSlider;
            target.onHealthChange -= RefreshText;
        }
    }

    void RefreshSlider()
    {
        if (target != null)
        {
            healthSlider.value = target.HP / target.MaxHP;
        }
    }

    void RefreshText()
    {
        if(target != null)
        {
            healthText.text = $"{target.HP}/{target.MaxHP}";
        }
    }
}
