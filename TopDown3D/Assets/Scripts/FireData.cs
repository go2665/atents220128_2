using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireData
{
    private GameObject prefab = null;
    private float currentCoolTime = 0.0f;
    private ShellData data = null;

    public GameObject ShellPrefab { get => prefab; }
    public bool IsFireReady { get => (currentCoolTime < 0.0f); }

    public FireData(GameObject shellPrefab, ShellData shellData, float startDelay = 0.0f)
    {
        prefab = shellPrefab;
        data = shellData;
        currentCoolTime = startDelay;
    }

    public void DecreaseCoolTime(float delta)
    {
        currentCoolTime -= delta;
    }

    public void ResetCoolTime()
    {
        currentCoolTime = data.coolTime;
    }
}
