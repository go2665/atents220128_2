using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
