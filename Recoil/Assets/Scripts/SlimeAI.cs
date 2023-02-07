using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAI : MonoBehaviour
{
    public GameObject explosion;
    public Transform castPoint;
    Rigidbody2D _rigidbody;
    public LayerMask groundLayer;
    public LayerMask wallLayer;
    bool isGrounded;
    bool isWall;
    float groundCheckDist = 0.3f;
    [SerializeField] float moveSpeed = 1f;
    public Animation anim;
    public Vector3 originalScale;

    void Start()
    {
        _rigidbody = transform.parent.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() 
    {
        isGrounded = Physics2D.OverlapCircle(castPoint.position, groundCheckDist, groundLayer);    
        isWall = Physics2D.OverlapCircle(castPoint.position, groundCheckDist, wallLayer);
    }

    void Update()
    {
        _rigidbody.velocity = new Vector2(moveSpeed * transform.parent.localScale.x, 0);
        if (GameManager.gm.paused || !GameManager.gm.started)
        {
            _rigidbody.velocity = new Vector2(0,0);
        }
        // Flips sprite if not grounded / hitting wall
        else if (!isGrounded || isWall)
        {
            transform.parent.localScale *= new Vector2(-1, 1);
        }
        if (!anim.isPlaying)
        {
            transform.localScale = originalScale;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("Bullet"))
        {
            if(Toggles.ParticleEffects)
                Instantiate(explosion, transform.position, explosion.transform.rotation);
            Destroy(other.gameObject);
            // _animator.SetTrigger("Die");
            Destroy(transform.parent.gameObject);
        }
    
    }
}   
