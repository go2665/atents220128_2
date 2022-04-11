using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target = null;
    public float smoothness = 2.0f;

    public void SetTarget(Transform _target)
    {
        target = _target;
    }

    private void FixedUpdate()
    {
        if (target != null)
        {
            transform.position = Vector3.Lerp(
                transform.position, target.position, smoothness * Time.fixedDeltaTime);
            transform.rotation = Quaternion.Lerp(
                transform.rotation, target.rotation, smoothness * Time.fixedDeltaTime);
        }
    }
}
