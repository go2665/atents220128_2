using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireData
{
    private GameObject prefab = null;
    private ShellData data = null;
    private float currentCoolTime = 0.0f;
    private float CurrentCoolTime
    {
        get => currentCoolTime;
        set
        {
            currentCoolTime = value;
            onCoolTimeChange?.Invoke(currentCoolTime / data.coolTime);
        }
    }

    public GameObject ShellPrefab { get => prefab; }
    public bool IsFireReady { get => (currentCoolTime < 0.0f); }

    public delegate void CoolTimeChangeDelegate(float ratio);
    public CoolTimeChangeDelegate onCoolTimeChange = null;

    public FireData(GameObject shellPrefab, ShellData shellData, float startDelay = 0.0f)
    {
        prefab = shellPrefab;
        data = shellData;
        CurrentCoolTime = startDelay;
    }

    public void DecreaseCoolTime(float delta)
    {
        CurrentCoolTime -= delta;
    }

    public void ResetCoolTime()
    {
        CurrentCoolTime = data.coolTime;
    }
}
