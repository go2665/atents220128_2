using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 장소 칸 위에 표시할 이미지 처리 클래스
/// </summary>
public class CoverImage_Normal : MonoBehaviour
{
    public enum Type
    {
        None = -1,
        GoldenKey = 0,
        FundPay
    }

    public Material[] materials;
    public Type type = Type.None;
    public Renderer myRenderer;

    private void Awake()
    {
        myRenderer = GetComponent<Renderer>();
    }

    private void Start()
    {
        SetImage(type);
    }

    public void SetImage(Type newType)
    {
        if (newType != Type.None)
        {
            myRenderer.material = materials[(int)newType];
        }
        else
        {
            gameObject.SetActive(false);
        }
        type = newType;
    }

    //private void OnValidate()
    //{
    //    SetImage(type);
    //}
}
