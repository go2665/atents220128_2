using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public GameObject placeNormal;
    public GameObject placeCorner;

    Place[] places;
    const int SideSize = 10;
    const int NumOfSize = 4;
    const int NumOfPlaces = SideSize * NumOfSize;

    private void Awake()
    {
        places = new Place[NumOfPlaces];

        GameObject prefab = null;
        Vector3 makeDir = Vector3.left;
        float makeAngle = 0.0f;
        Vector3 startPos = transform.position;
        for (int i=0;i< NumOfSize; i++)
        {
            GameObject place = null;
            for (int j=0; j<SideSize;j++)
            {
                if(j==9)
                {
                    prefab = placeCorner;
                }
                else
                {
                    prefab = placeNormal;
                }
                place = Instantiate(prefab, this.transform);
                int id = 1 + i * SideSize + j;
                id %= 40;
                place.name = $"ID_{id}";

                makeAngle = 90.0f * i;
                place.transform.Rotate(0, makeAngle, 0);
                place.transform.Translate(j * makeDir + startPos, Space.World);

                if(id == 0)
                {
                    place.transform.SetAsFirstSibling();
                }
            }
            startPos = place.transform.position;
            makeDir = Quaternion.Euler(0, 90, 0) * makeDir;
        }
    }

    public Place GetPlace(MapID id)
    {
        return places[(int)id];
    }

    public void Move(Player player, int count)
    {
        int dest = (int)player.Position + count;
        if(dest >= NumOfPlaces)
        {
            dest -= NumOfPlaces;
            Place_Start start = GetPlace(MapID.Start) as Place_Start;
            start.GetSalaray(player);
        }

        player.Position = (MapID)dest;
        GetPlace(player.Position).OnArrive(player);
    }

    public void Move(Player player, MapID mapID)
    {
        int move = mapID - player.Position;
        if( move < 0 )
        {
            move = NumOfPlaces + move;
        }
        Move(player, move);
    }
}
