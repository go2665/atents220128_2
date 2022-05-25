using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Item Data", menuName = "Scriptable Object/Item Data", order = 1)]
public class ItemData : ScriptableObject
{
    [Header("아이템 기본 정보")]
    public uint id = 0;
    public string itemName = "아이템";
    public Sprite itemImage;
    public string itemDesc = "아이템설명";
    public int price = 0;
    public GameObject prefab = null;
    public int maxStackCount = 1;

    [Header("컬라이더 정보")]
    public float triggerSize = 1.0f;
}
