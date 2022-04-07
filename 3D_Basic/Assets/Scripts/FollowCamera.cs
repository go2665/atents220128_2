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
}
