using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb2d;
    public float speed = 4f;
    public float maxHorizontalSpeed = 5f;
    public float recoilAmount = 4f;
    public Transform gun;

    public bool isGrounded = false;
    public Transform GroundCheck1; // Put the prefab of the ground here
    public LayerMask groundLayer; // Insert the layer here.

    public int maxBullets = 4;
    public int bulletsLeft = 4;
    public GameObject bulletPrefab;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(GroundCheck1.position, 0.15f, groundLayer);
        float horizontal = Input.GetAxis("Horizontal");
        if (horizontal < 0)
        {
            transform.localScale = new Vector2(-1, 1);
            gun.localScale = new Vector2(-1, 1);
        }
        if (horizontal > 0)
        {
            transform.localScale = new Vector2(1, 1);
            gun.localScale = new Vector2(1, 1);
        }
        //rb2d.velocity = new Vector2(horizontal * speed, rb2d.velocity.y);
        if (Mathf.Abs(rb2d.velocity.x) < maxHorizontalSpeed / 2 || Mathf.Sign(horizontal) != Mathf.Sign(rb2d.velocity.x))
            rb2d.AddForce(new Vector2(horizontal * speed, 0), ForceMode2D.Force);
        
        var mousePos = Input.mousePosition;
        mousePos.z = 4f;
        var objectPos = Camera.main.WorldToScreenPoint(gun.position);
        mousePos.x = mousePos.x - objectPos.x;
        mousePos.y = mousePos.y - objectPos.y;
        var angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        gun.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        if (Input.GetMouseButtonDown(0) && bulletsLeft > 0)
        {
            rb2d.AddForce(-mousePos.normalized*recoilAmount, ForceMode2D.Impulse);
        }
        if(rb2d.velocity.x > maxHorizontalSpeed)
        {
            rb2d.velocity = new Vector2(maxHorizontalSpeed, rb2d.velocity.y);
        } 
        else if (rb2d.velocity.x < -maxHorizontalSpeed)
        {
            rb2d.velocity = new Vector2(-maxHorizontalSpeed, rb2d.velocity.y);
        }

    }
}
