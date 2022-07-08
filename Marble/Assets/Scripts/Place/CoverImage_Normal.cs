using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverImage_Normal : MonoBehaviour
{
    public enum Type
    {
        None = -1,
        GoldenKey = 0
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

    public void SetImage(Type type)
    {
        if (type != Type.None)
        {
            myRenderer.material = materials[(int)type];
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    //private void OnValidate()
    //{
    //    SetImage(type);
    //}
}
