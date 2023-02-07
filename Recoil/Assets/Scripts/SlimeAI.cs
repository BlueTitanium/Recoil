using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAI : MonoBehaviour
{
    public GameObject explosion;
    public Transform castPoint;
    Animator _animator;
    Rigidbody2D _rigidbody;
    public LayerMask groundLayer;
    public LayerMask wallLayer;
    bool isGrounded;
    bool isWall;
    float groundCheckDist = 0.3f;
    [SerializeField] float moveSpeed = 1f;

    void Start()
    {
        // _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() 
    {
        isGrounded = Physics2D.OverlapCircle(castPoint.position, groundCheckDist, groundLayer);    
        isWall = Physics2D.OverlapCircle(castPoint.position, groundCheckDist, wallLayer);
    }

    void Update()
    {
        _rigidbody.velocity = new Vector2(moveSpeed * transform.localScale.x, _rigidbody.velocity.y);
        if (GameManager.gm.paused || !GameManager.gm.started)
        {
            _rigidbody.velocity = new Vector2(0,0);
        }
        // Flips sprite if not grounded / hitting wall
        else if (!isGrounded || isWall)
        {
            transform.localScale *= new Vector2(-1, 1);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("Bullet"))
        {
            // Instantiate(explosion, transform.position, Quarternion.identity);
            Destroy(other.gameObject);
            // _animator.SetTrigger("Die");
            Destroy(gameObject, .15f);
        }
    
    }
}   
