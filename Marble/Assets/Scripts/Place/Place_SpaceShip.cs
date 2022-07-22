using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Place_SpaceShip : Place
{
    public const int shipUsePrice = 200;
    Map map;

    private void Awake()
    {
        CoverImage_Corner cover = GetComponentInChildren<CoverImage_Corner>();
        cover.SetImage(CoverImage_Corner.Type.SpaceShip);
        cover.transform.Rotate(0, 0, 180);
    }

    private void Start()
    {
        map = GameManager.Inst.GameMap;
    }

    public override void OnArrive(Player player)
    {
        //Debug.Log($"{player} : 우주왕복선입니다.");
        // UI로 사용 여부 확인
        if (player.Type == PlayerType.Human)
        {
            GameManager.Inst.UI_Manager.ShowSpaceShipPanel(true, player);
        }
        else
        {
            float risk = map.TravelRiskCheck(player);
            float riskCheck = Random.Range(0, 1.0f);
            MapID target = MapID.Start;
            if ((player.Money - shipUsePrice) > 500)    // 여유 금액이 500만원 이상일 때만 사용
            {
                if (risk < riskCheck)
                {
                    // 안전
                    float ratio = map.RemainingPlaceRatio();
                    if (ratio < 0.8f)
                    {
                        // 아직 안산 비싼 땅으로 간다(게임이 아직 많이 진행되지 않았을 때, 빈땅이 많을 때)
                        Place place = map.NotOwnedExpensivePlace(player);
                        if (place != null)
                        {
                            target = place.ID;
                        }
                        else
                        {
                            target = MapID.Start;
                        }
                    }
                    else
                    {
                        // 건물을 안지은 내땅으로 간다.(게임이 많이 진행되었고 내 땅이 있고 내가 돈이 있을 때)
                        City city = player.FindNoBuildCity();
                        if(city != null)
                        {
                            target = city.ID;
                        }
                        else
                        {
                            target = MapID.Start;
                        }
                    }
                }
                else
                {
                    if ((player.Money - shipUsePrice) >= 0)
                    {
                        // 위험
                        if (risk < Random.Range(0, 0.5f))
                        {
                            // 건물을 안지은 내땅으로 간다.(게임이 많이 진행되었고 빈땅이 있고 내가 돈이 있을 때)
                            City city = player.FindNoBuildCity();
                            if (city != null)
                            {
                                target = city.ID;
                            }
                            else
                            {
                                // 적당한 도시가 없으면 무인도로
                                target = MapID.Island;
                            }
                        }
                        else
                        {
                            // 무인도로 간다.(게임이 많이 진행되었을 때, 내가 돈이 없을 때)
                            target = MapID.Island;
                        }
                    }
                }
            }

            if ((player.Money - shipUsePrice) >= 0)
            {
                CityBase coumbia = GameManager.Inst.GameMap.GetPlace(MapID.Columbia) as CityBase;
                Player ownerPlayer = GameManager.Inst.GetPlayer(coumbia.Owner);

                player.Money -= shipUsePrice;
                ownerPlayer.Money += shipUsePrice;

                player.Move(target);
                return;
            }

            base.OnArrive(player);
        }
    }
}
