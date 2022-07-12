using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Place_GoldenKey : Place
{
    private void Awake()
    {
        CoverImage_Normal cover = GetComponentInChildren<CoverImage_Normal>();
        cover.SetImage(CoverImage_Normal.Type.GoldenKey);
    }

    public override void OnArrive(Player player)
    {
        //Debug.Log($"{player} : 황금 열쇠를 엽니다.");
        base.OnArrive(player);
    }
}
