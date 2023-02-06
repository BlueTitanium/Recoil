using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAI : MonoBehaviour
{
    public GameObject explosion;
    Transform player;
    public Transform castPoint;
    Animator _animator;
    Rigidbody2D _rigidbody;
    
    public LayerMask groundLayer;
    public LayerMask wallLayer;
    bool isGrounded;
    bool isWall;
    float groundCheckDist = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        // _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(SlimeMove());
        
    }

    private void FixedUpdate() 
    {
        isGrounded = Physics2D.OverlapCircle(castPoint.position, groundCheckDist, groundLayer);    
        isWall = Physics2D.OverlapCircle(castPoint.position, groundCheckDist, wallLayer);
    }

    // Update is called once per frame
    void Update()
    {

        // Flips sprite if not grounded / hitting wall
        if (!isGrounded || isWall)
        {
            transform.localScale *= new Vector2(-1, 1);
        }

        // Flips sprite to face player direction
        // else if (player.position.x > transform.position.x && transform.localScale.x < 0 ||
        // player.position.x < transform.position.x && transform.localScale.x > 0) 
        // {
        //     transform.localScale *= new Vector2(-1,1);
        // }
    }

    IEnumerator SlimeMove()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(1f, 5f));
            yield return new WaitUntil(() => !(GameManager.gm.paused || !GameManager.gm.started));
            _rigidbody.AddForce(new Vector2(transform.localScale.x * 100, 100));
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
