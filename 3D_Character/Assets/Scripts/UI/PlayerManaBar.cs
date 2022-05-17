using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManaBar : MonoBehaviour
{
    // 플레이어의 HP 변경에 따라 slider의 value 수정
    // 플레이어의 HP 변경에 따라 text의 글자 수정

    Slider manaSlider = null;
    Text manaText = null;
    IMana target = null;

    private void Awake()
    {
        manaSlider = GetComponentInChildren<Slider>();
        manaText = GetComponentInChildren<Text>();
    }

    private void Start()
    {
        target = GameManager.Inst.MainPlayer as IMana;
        if (target != null && target.onManaChange == null)
        {
            target.onManaChange += RefreshSlider;
            target.onManaChange += RefreshText;
            RefreshSlider();
            RefreshText();
        }
    }

    private void OnEnable()
    {
        if (target != null)
        {
            target.onManaChange += RefreshSlider;
            target.onManaChange += RefreshText;
        }
    }

    private void OnDisable()
    {
        if (target != null && target.onManaChange != null)
        {
            target.onManaChange -= RefreshSlider;
            target.onManaChange -= RefreshText;
        }
    }

    void RefreshSlider()
    {
        if (target != null)
        {
            manaSlider.value = target.MP / target.MaxMP;
        }
    }

    void RefreshText()
    {
        if(target != null)
        {
            manaText.text = $"{target.MP}/{target.MaxMP}";
        }
    }
}
