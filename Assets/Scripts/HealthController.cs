using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
public AudioClip collectedClip;
public ParticleSystem PowerUp;

    void OnTriggerEnter2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();
        

        if (controller != null)
        {
            if(controller.health < controller.maxHealth)
            {
                
                controller.ChangeHealth(+ 1);
                
                Destroy(gameObject);
                PowerUp.Play();

                
            }
        }
    }
}