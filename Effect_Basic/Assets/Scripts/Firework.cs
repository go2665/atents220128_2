using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Firework : MonoBehaviour
{
    public ParticleSystem particle = null;
    public float upSpeed = 1.0f;
    bool fire = false;

    void Update()
    {
        if(Keyboard.current.anyKey.wasPressedThisFrame == true)
        {
            particle.Play();
            fire = true;
        }

        if( fire)
        {
            transform.Translate(Vector3.up * upSpeed * Time.deltaTime);
        }
    }
}
