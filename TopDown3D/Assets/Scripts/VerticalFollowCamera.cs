using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalFollowCamera : MonoBehaviour
{
    public Transform target = null;
    public float maxZ = 25.0f;
    public float minZ = -25.0f;
    public float smoothness = 2.0f;

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    private void FixedUpdate()
    {
        if(target != null)
        {
            transform.position = Vector3.Lerp(transform.position,
                new Vector3(0, transform.position.y, Mathf.Clamp(target.position.z, minZ, maxZ)),
                smoothness * Time.fixedDeltaTime);
        }
    }
}
