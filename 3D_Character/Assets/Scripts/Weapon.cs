using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    Queue<IBattle> hitTarget = new Queue<IBattle>(16);
    Player player = null;

    private void Start()
    {
        player = GameManager.Inst.MainPlayer;
    }

    private void OnTriggerEnter(Collider other)
    {        
        if (player.IsAttack && other.isTrigger == false && other.gameObject != player.gameObject)
        {
            IBattle battle = other.GetComponent<IBattle>();
            if (battle != null)
            {
                //Debug.Log($"target : {other.gameObject.name}");
                hitTarget.Enqueue(battle);
            }
        }
    }

    public IBattle GetHitTarget()
    {
        return hitTarget.Dequeue();
    }

    public int HitTargetCount()
    {
        return hitTarget.Count;
    }
}
