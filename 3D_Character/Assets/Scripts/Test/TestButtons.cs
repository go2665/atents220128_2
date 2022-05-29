using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestButtons : MonoBehaviour
{
    Button upButton = null;
    Button downButton = null;

    private void Awake()
    {
        upButton = transform.Find("UpButton").GetComponent<Button>();
        downButton = transform.Find("DownButton").GetComponent<Button>();
        upButton.onClick.AddListener(HealthUp);
        downButton.onClick.AddListener(HealthDown);
    }

    void HealthUp()
    {
        GameManager.Inst.MainPlayer.HP += (GameManager.Inst.MainPlayer.MaxHP * 0.1f);
        //Debug.Log($"HealthUp : {GameManager.Inst.MainPlayer.HP}");
    }

    void HealthDown()
    {
        GameManager.Inst.MainPlayer.HP -= (GameManager.Inst.MainPlayer.MaxHP * 0.1f);
        //Debug.Log($"HealthDown : {GameManager.Inst.MainPlayer.HP}");
    }
}
