using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoolTimeUI : MonoBehaviour
{
    public Sprite coolTimeProgress = null;
    public Sprite coolTimeComplete = null;

    Image backreoundImage = null;
    Image selecedImage = null;
    Slider coolTimeSlider = null;

    private void Awake()
    {
        coolTimeSlider = GetComponent<Slider>();
        backreoundImage = transform.Find("Background").GetComponent<Image>();
        selecedImage = transform.Find("Selected").GetComponent<Image>();
    }

    void Refresh(float ratio)
    {
        coolTimeSlider.value = ratio;
        if( coolTimeSlider.value > 0.0f )
        {
            backreoundImage.sprite = coolTimeProgress;
        }
        else
        {
            backreoundImage.sprite = coolTimeComplete;
        }
    }

    public void BindingCoolTime(FireData data)
    {
        data.onCoolTimeChange = Refresh;
    }

    public void SetSelected(bool select)
    {
        if( select )
        {
            selecedImage.color = Color.white;
        }
        else
        {
            selecedImage.color = Color.clear;
        }
    }
}
