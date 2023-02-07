using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    public float damage = 1f;
    public float knockbackStrength = 3f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            
            PlayerController.p.TakeDamage(damage);
            //get direction between this object and player and knock it back
            Vector3 dir = transform.position - PlayerController.p.transform.position;
            PlayerController.p.rb2d.velocity = Vector2.zero;
            PlayerController.p.rb2d.AddForce(-dir * knockbackStrength, ForceMode2D.Impulse);
            
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController.p.TakeDamage(damage);
            //get direction between this object and player and knock it back
            Vector3 dir = transform.position - PlayerController.p.transform.position;
            PlayerController.p.rb2d.velocity = Vector2.zero;
            PlayerController.p.rb2d.AddForce(-dir * knockbackStrength, ForceMode2D.Impulse);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController.p.TakeDamage(damage);
            //get direction between this object and player and knock it back
            Vector3 dir = transform.position - PlayerController.p.transform.position;
            PlayerController.p.rb2d.velocity = Vector2.zero;
            PlayerController.p.rb2d.AddForce(-dir * knockbackStrength, ForceMode2D.Impulse);
        }
    }
}
