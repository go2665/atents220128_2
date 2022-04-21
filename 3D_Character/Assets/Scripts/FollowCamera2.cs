using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera2 : MonoBehaviour
{
    public Transform target = null;
    public float smoothness = 3.0f;

    Vector3 offset = Vector3.zero;

    private void Start()
    {
        //target = FindObjectOfType<Player>().gameObject.transform;
        if(target != null)
        {
            offset = transform.position - target.transform.position;
        }
    }

    private void LateUpdate()
    {
        if (target != null)
        {
            transform.position = Vector3.Lerp(
                transform.position, target.position + offset, smoothness * Time.deltaTime);
            
        }
    }
}
