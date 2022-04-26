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
        //Debug.Log($"target2 : {other.gameObject.name}");
        if (player.IsAttack && other.CompareTag("HitTarget") 
            && other.gameObject != player.gameObject)
        {
            Debug.Log($"target : {other.gameObject.transform.parent.name}");
            IBattle battle = other.GetComponentInParent<IBattle>();
            if (battle != null)
            {                
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
