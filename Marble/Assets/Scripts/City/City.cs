using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : CityBase
{
    public BuildingData[] buildingDatas;
    public GameObject BuildingObj;
    Building building;

    protected override void Awake()
    {
        base.Awake();

        int NumOfBuildingTypes = System.Enum.GetValues(typeof(BuildingType)).Length;
        buildingDatas = new BuildingData[NumOfBuildingTypes];
        
        //buildingObj = new GameObject[NumOfBuildingTypes];
        //for (int i = 0; i < NumOfBuildingTypes; i++)
        //{
        //    buildingObj[i] = transform.GetChild(i+1).gameObject;
        //    buildingObj[i].SetActive(false);
        //}
    }

    public void MakeBuildings(int[] counts)
    {
        //buildingObj[(int)type].SetActive(true);
        building.SetColor(GameManager.Inst.PlayerColor[(int)owner]);
        building.gameObject.SetActive(true);

        Player ownerPlayer = GameManager.Inst.Players[(int)owner];

        for (int i = 0; i < counts.Length; i++)
        {
            buildingDatas[i].count += counts[i];
            
            int totalPrice = buildingDatas[i].price * counts[i];
            ownerPlayer.Money -= totalPrice;
        }
        ReCalcValue();
        ReCalcTotalUsePrice();
    }

    void ReCalcValue()
    {
        int buildingValue = price;
        foreach (var b in buildingDatas)
        {
            buildingValue += b.price * b.count;
        }

        totalValue = buildingValue;
    }

    void ReCalcTotalUsePrice()
    {
        int total = usePrice;
        foreach (var b in buildingDatas)
        {
            total += b.usePrice * b.count;
        }
        totalUsePrice = total;
    }

    void MakeBuildingsForCPU(Player cpu)
    {
        int[] counts = new int[3];
        float[] probability = new float[3]; // 빌라, 빌딩, 호텔의 건설 확율

        for( int i=2;i>=0;i--)
        {
            probability[i] = 0;
            if (cpu.Money >= buildingDatas[i].price)
            {
                // 구매 가능
                probability[i] += 0.1f; 

                // 돈이 많을 때 (3000만원 이상)
                if (cpu.Money > 3000)
                {
                    probability[i] += 0.5f;
                }

                // 건물의 효율이 높을 때(1.7)
                if ((float)buildingDatas[i].usePrice / buildingDatas[i].price > 1.7f)
                {
                    probability[i] += 0.5f;
                }

                // 앞자리의 평균값 확인
                int sumOfTotalUsePrice = 0;                
                for (int j=2; j<13;j++)
                {
                    int target = (int)ID + j;
                    if (target > (int)MapID.Seoul)
                    {
                        target -= 40;
                    }
                    CityBase city = GameManager.Inst.GameMap.GetPlace((MapID)target) as CityBase;
                    if(city != null)
                    {
                        sumOfTotalUsePrice += city.TotalUsePrice;
                    }
                }
                float average = (float)sumOfTotalUsePrice / 12.0f;
                if( (cpu.Money - buildingDatas[i].price) > average * 1.5f)
                {
                    probability[i] += 0.3f;
                }
                else
                {
                    probability[i] -= 0.3f;
                }
            }
            else
            {
                // 구매 불가
                probability[i] = 0;
            }            
        }

        for(int i=0;i<3;i++)
        {
            if( probability[i] > Random.Range(0.0f,1.0f) )
            {
                counts[i] = 1;
            }
        }

        MakeBuildings(counts);
    }

    public override void Initialize(GameObject obj, ref MapData mapData)
    {
        base.Initialize(obj, ref mapData);
        buildingDatas[(int)BuildingType.Villa].price = mapData.villaBuyPrice;
        buildingDatas[(int)BuildingType.Villa].usePrice = mapData.villaUsePrice;
        buildingDatas[(int)BuildingType.Villa].count = 0;
        buildingDatas[(int)BuildingType.Building].price = mapData.buildingBuyPrice;
        buildingDatas[(int)BuildingType.Building].usePrice = mapData.buildingUsePrice;
        buildingDatas[(int)BuildingType.Building].count = 0;
        buildingDatas[(int)BuildingType.Hotel].price = mapData.hotelBuyPrice;
        buildingDatas[(int)BuildingType.Hotel].usePrice = mapData.hotelUsePrice;
        buildingDatas[(int)BuildingType.Hotel].count = 0;

        GameObject buildingObj = Instantiate(BuildingObj, transform);
        building = buildingObj.GetComponent<Building>();
        buildingObj.transform.Translate(new(-0.5f, 0, -0.5f));
        buildingObj.SetActive(false);

    }

    public override void OnArrive(Player player)
    {
        if(player.Type == Owner)
        {
            // 건물 짓기
            if (player.Type == PlayerType.Human)
            {
                // 휴먼일때
                GameManager.Inst.UI_Manager.ShowBuildingPanel(true, player, this);
            }
            else
            {
                // CPU 일때
                MakeBuildingsForCPU(player);
                base.OnArrive(player);
            }
        }
        else
        {
            base.OnArrive(player);
        }
    }

    public override void Sell(PlayerType buyer, int sellPrice = -1)
    {        
        sellPrice = 0;  // 건물 판매 가격 계산
        for (int i = 0; i < buildingDatas.Length; i++)
        {
            sellPrice += buildingDatas[i].price * buildingDatas[i].count;
        }

        if (buyer == PlayerType.Bank)
        {
            sellPrice >>= 1;    // 판매 대상이 은행이니 가격 절반

            // 건물 초기화 작업
            building.gameObject.SetActive(false);
            for (int i = 0; i < buildingDatas.Length; i++)
            {
                buildingDatas[i].count = 0;
            }

            // 가격들 다시 계산
            ReCalcValue();
            ReCalcTotalUsePrice();
        }

        // 건물들 비용 전달
        Player ownerPlayer = GameManager.Inst.GetPlayer(owner);
        Player buyerPlayer = GameManager.Inst.GetPlayer(buyer);

        ownerPlayer.Money += sellPrice;
        buyerPlayer.Money -= sellPrice;

        base.Sell(buyer);
    }
}
