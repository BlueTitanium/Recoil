using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 8f;
    public Rigidbody2D rb2d;
    public GameObject explosion;
    public bool playerBullet = false;

    // Start is called before the first frame update
    void Start()
    {
        rb2d.AddRelativeForce(Vector2.right * speed, ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground") || (collision.CompareTag("Player")&&!playerBullet))
        {
            if (Toggles.ParticleEffects)
                Instantiate(explosion, transform.position, explosion.transform.rotation);
            Destroy(gameObject);
            CameraShake.cs.cameraShake(.2f, 1.5f, false);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground") || (collision.CompareTag("Player") && !playerBullet))
        {
            if (Toggles.ParticleEffects)
                Instantiate(explosion, transform.position, explosion.transform.rotation);
            Destroy(gameObject);
            CameraShake.cs.cameraShake(.2f, 1.5f, false);
        }
    }
}
