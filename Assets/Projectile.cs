using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Projectile : MonoBehaviour
{
    Rigidbody2D rb;
public ParticleSystem HitParticle;
public int botCount;
public TextMeshProUGUI Collected;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        HitParticle.Stop();
    }

    // Update is called once per frame
  public void Launch (Vector2 direction, float force)
  {
    rb.AddForce(direction * force);
  }
  void Update()
  {
    if(transform.position.magnitude > 1000.0f)
    {
        Destroy(gameObject);
    }
  }
  void OnCollisionEnter2D(Collision2D other)
  {
    EnemyController e = other.collider.GetComponent<EnemyController>();
    botCount= botCount + 1;
    if(e !=null){
        e.Fix();
    }
   HitParticle.Play();
    Destroy(gameObject);
  }

 
}
