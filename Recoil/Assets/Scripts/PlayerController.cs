using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float maxHP = 3f;
    public float curHP = 3f;
    public Image healthBar;

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
    public bool reloading = false;
    public Transform spawnPoint;
    public AmmoUI ammoUI;

    // Start is called before the first frame update
    void Start()
    {
        curHP = maxHP;
        healthBar.fillAmount = curHP / maxHP;
    }

    public void TakeDamage(float damage)
    {
        curHP -= damage;
        healthBar.fillAmount = curHP / maxHP;
        if(curHP <= 0)
        {
            StartCoroutine(Die());
        }
    }

    public IEnumerator Die()
    {
        yield return null;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartLevel();
        }
        
        isGrounded = Physics2D.OverlapCircle(GroundCheck1.position, 1f, groundLayer);
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
        if (isGrounded && (Mathf.Abs(rb2d.velocity.x) < maxHorizontalSpeed / 2 || Mathf.Sign(horizontal) != Mathf.Sign(rb2d.velocity.x)))
            rb2d.AddForce(new Vector2(horizontal * speed, 0), ForceMode2D.Force);
        
        var mousePos = Input.mousePosition;
        mousePos.z = 4f;
        var objectPos = Camera.main.WorldToScreenPoint(gun.position);
        mousePos.x = mousePos.x - objectPos.x;
        mousePos.y = mousePos.y - objectPos.y;
        var angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        gun.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        if (!reloading && Input.GetMouseButtonDown(0) && bulletsLeft > 0)
        {
            rb2d.AddForce(-mousePos.normalized*recoilAmount, ForceMode2D.Impulse);
            bulletsLeft--;
            Instantiate(bulletPrefab, spawnPoint.position, gun.rotation);
            ammoUI.removeAmmo();
        }
        if((bulletsLeft <= 0 || (Input.GetMouseButtonDown(1) && bulletsLeft != maxBullets)) && !reloading && isGrounded)
        {
            reloading = true;
            StartCoroutine(ReloadGun(1f));
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

    IEnumerator ReloadGun(float t)
    {
        yield return new WaitForSeconds(t);
        ammoUI.reload();
        bulletsLeft = maxBullets;
        reloading = false;
    }

    public void RestartLevel()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
