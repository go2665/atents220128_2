using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Place_GoldenKey : Place
{
    GoldenKeyManager gkManager;
    Player arrivePlayer;

    private void Awake()
    {
        CoverImage_Normal cover = GetComponentInChildren<CoverImage_Normal>();
        cover.SetImage(CoverImage_Normal.Type.GoldenKey);
    }

    private void Start()
    {        
        gkManager = GameManager.Inst.GoldenKeyManager;
    }

    public override void OnArrive(Player player)
    {
        //Debug.Log($"{player} : 황금 열쇠를 엽니다.");
        arrivePlayer = player;
        GameManager.Inst.UI_Manager.ShowGoldenKeyDrawPanel(true, player);

    }

    //void OnPanelClose()
    //{
    //    base.OnArrive(arrivePlayer);
    //    arrivePlayer = null;
    //}
}
