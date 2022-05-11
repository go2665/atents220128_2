using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Item : MonoBehaviour
{
    public ItemData data = null;

    protected SphereCollider trigger = null;

    private void Awake()
    {
        trigger = GetComponent<SphereCollider>();
        trigger.isTrigger = true;
    }

    protected virtual void Start()
    {
        trigger.radius = data.triggerSize;
        Instantiate(data.prefab, transform.position, transform.rotation, transform);
    }

    public void Pickuped()
    {
        Player player = GameManager.Inst.MainPlayer;
        player.onPickupAction -= Pickuped;
        Destroy(this.gameObject);   //나중에 인벤토리로 들어가는 것으로 바뀔 예정
    }

    private void OnTriggerEnter(Collider other)
    {
        Player player = GameManager.Inst.MainPlayer;
        if(other.gameObject == player.gameObject)
        {
            player.onPickupAction += Pickuped;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Player player = GameManager.Inst.MainPlayer;
        if (other.gameObject == player.gameObject)
        {
            player.onPickupAction -= Pickuped;
        }
    }
}
