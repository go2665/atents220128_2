using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 장소 칸 위에 표시할 이미지 처리 클래스
/// </summary>
public class CoverImage_Corner : MonoBehaviour
{
    public enum Type
    {
        Start = 0,
        Island,
        Fund,
        SpaceShip
    }

    public Material[] materials;
    public Type type = Type.Start;

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
        myRenderer.material = materials[(int)newType];
        type = newType;
    }

    //private void OnValidate()
    //{
    //    SetImage(type);
    //}
}
