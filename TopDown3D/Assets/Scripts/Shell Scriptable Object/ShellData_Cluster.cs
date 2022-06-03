using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Shell Data(Cluster)", menuName = "Scriptable Object/Shell Data(Cluster)", order = 2)]
public class ShellData_Cluster : ShellData
{
    public float lifeTime = 1.0f;
    public float upPower = 20.0f;

    public void Explosion(Transform objTransform, Vector3 up, Shell_Submunition[] submunitions)
    {
        foreach (Shell_Submunition sub in submunitions)
        {
            sub.transform.parent = null;
            sub.gameObject.SetActive(true);
            sub.RandomSpread(-up);
        }
        Explosion(objTransform, up);
    }
}
