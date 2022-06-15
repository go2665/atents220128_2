using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Field : MonoBehaviour
{
    public Ship[] ships;
    public BattleField field;

    private void Start()
    {
        Test_FieldDeployment();
    }

    private void Test_FieldDeployment()
    {
        field.ShipDeployment(new Vector2Int(0, 0), ships[0]);
        field.ShipDeployment(new Vector2Int(3, 3), ships[1]);
        field.ShipDeployment(new Vector2Int(5, 5), ships[2]);
        field.ShipDeployment(new Vector2Int(9, 9), ships[4]);   // 밖을 벗어나서 안됨
        field.ShipDeployment(new Vector2Int(0, 0), ships[4]);   // 다른배와 겹쳐서 안됨(머리)
        field.ShipDeployment(new Vector2Int(3, 2), ships[4]);   // 다른배와 겹쳐서 안됨(꼬리)

        ships[3].Rotate();  // 3칸짜리. 동쪽을 바라보도록
        field.ShipDeployment(new Vector2Int(4, 4), ships[3]);   // 다른배와 겹쳐서 안됨(몸통)
    }
}
