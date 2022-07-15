using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    Material mat;

    private void Awake()
    {
        Renderer renderer = GetComponentInChildren<Renderer>();
        mat = renderer.material;
    }

    public void SetColor(Color color)
    {
        mat.color = color;
    }

}
