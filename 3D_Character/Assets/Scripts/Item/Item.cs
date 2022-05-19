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
        player.onPickupAction -= Pickuped;   // 아래에서 Destroy가 실행되면 Pickuped 함수도 사라지므로 제거
        player.Inven.AddItem(data);
        Destroy(this.gameObject);   
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
