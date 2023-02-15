using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController p;

    // Health
    public float maxHP = 3f;
    public float curHP = 3f;
    public Image healthBar;
    public Animation healthBarAnim;
    public float canTakeDamage = 0; // can only take damage while 0; used for i-frames
    public Animation bodyAnim;

    // Movement
    public Rigidbody2D rb2d;
    public float speed = 4f;
    public float maxHorizontalSpeed = 5f;
    public float recoilAmount = 4f;
    public Transform gun;
    public Animator gunAnimator;
    public Transform wheel;
    public float rotationAmount = 5f;

    // Grounded
    public bool isGrounded = false;
    public Transform GroundCheck1; // Put the prefab of the ground here
    public LayerMask groundLayer; // Insert the layer here.

    // Shooting
    public int maxBullets = 4;
    public int bulletsLeft = 4;
    public GameObject bulletPrefab;
    public bool reloading = false;
    public float cooldown = .5f;
    public float cooldownLeft = 0f;
    public Transform spawnPoint;
    public AmmoUI ammoUI;

    // VFX
    public ParticleSystem shot;
    public ParticleSystem smoke;

    // SFX
    public AudioSource aud;
    public AudioClip shoot, reload, hurt;

    // Start is called before the first frame update
    void Start()
    {
        p = this;
        curHP = maxHP;
        healthBar.fillAmount = curHP / maxHP;
    }

    public void TakeDamage(float damage)
    {
        if (canTakeDamage <= 0 && !GameManager.gm.ended)
        {
            aud.Stop();
            aud.PlayOneShot(hurt);
            CameraShake.cs.cameraShake(.5f, 1.7f);
            if (Toggles.Animation)
            {
                if(damage!= 0)
                {
                    healthBarAnim.Play();
                    bodyAnim.Play("Player_TakeDamage");
                    canTakeDamage += .3f;
                } else
                {
                    bodyAnim.Play("Player_Bounce");
                    canTakeDamage += .05f;
                }
            }
            else
            {
                if (damage != 0)
                {
                    canTakeDamage += .3f;
                }
                else
                {
                    canTakeDamage += .05f;
                }
            }
            curHP -= damage;
            healthBar.fillAmount = curHP / maxHP;
        }
        
        if(curHP <= 0)
        {
            StartCoroutine(Die());
        }
    }

    public IEnumerator Die()
    {

        GameManager.gm.LoseGame();
        yield return null;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    RestartLevel();
        //}
        if (!(GameManager.gm.paused || !GameManager.gm.started))
        {
            isGrounded = Physics2D.OverlapCircle(GroundCheck1.position, 1f, groundLayer);
            float horizontal = Input.GetAxis("Horizontal");
            //rb2d.velocity = new Vector2(horizontal * speed, rb2d.velocity.y);
            if (canTakeDamage <= 0 && isGrounded && (Mathf.Abs(rb2d.velocity.x) < maxHorizontalSpeed / 2 || Mathf.Sign(horizontal) != Mathf.Sign(rb2d.velocity.x)))
                rb2d.AddForce(new Vector2(horizontal * speed, 0), ForceMode2D.Force);

            //ROTATE GUN
            var mousePos = Input.mousePosition;
            mousePos.z = 4f;
            var objectPos = Camera.main.WorldToScreenPoint(gun.position);
            mousePos.x = mousePos.x - objectPos.x;
            mousePos.y = mousePos.y - objectPos.y;
            var angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
            gun.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            //SHOOT
            if (!reloading && Input.GetMouseButtonDown(0) && bulletsLeft > 0 && cooldownLeft <= 0)
            {
                aud.Stop();
                aud.PlayOneShot(shoot);
                if (Toggles.ParticleEffects)
                {
                    shot.Play();
                    smoke.Play();
                }
                if (Toggles.Animation)
                    gunAnimator.SetTrigger("Shoot");
                rb2d.AddForce(-mousePos.normalized * recoilAmount, ForceMode2D.Impulse);
                bulletsLeft--;
                Instantiate(bulletPrefab, spawnPoint.position, gun.rotation);
                ammoUI.removeAmmo();
                cooldownLeft = cooldown;
                CameraShake.cs.cameraShake(.3f, 1.5f);
            }
            //RELOAD
            if ((bulletsLeft <= 0 || (Input.GetMouseButtonDown(1) && bulletsLeft != maxBullets)) && !reloading && isGrounded)
            {
                
                reloading = true;
                StartCoroutine(ReloadGun(1f));
            }

            //ESSENTIALLY LIMITING VELOCITY
            if (rb2d.velocity.x > maxHorizontalSpeed)
            {
                rb2d.velocity = new Vector2(maxHorizontalSpeed, rb2d.velocity.y);
            }
            else if (rb2d.velocity.x < -maxHorizontalSpeed)
            {
                rb2d.velocity = new Vector2(-maxHorizontalSpeed, rb2d.velocity.y);
            }
            //SHOWING WHEEL ROTATION
            if (rb2d.velocity.x > 0)
            {
                wheel.eulerAngles = new Vector3(0, 0, wheel.eulerAngles.z - rotationAmount * rb2d.velocity.x);
            }
            else if (rb2d.velocity.x < 0)
            {
                wheel.eulerAngles = new Vector3(0, 0, wheel.eulerAngles.z - rotationAmount * rb2d.velocity.x);
            }
            smoke.transform.position = spawnPoint.position;
        }
        //COOLDOWN TICKS
        if (cooldownLeft > 0)
        {
            cooldownLeft -= Time.deltaTime;
        }
        if (canTakeDamage > 0)
        {
            canTakeDamage -= Time.deltaTime;
        }
    }

    IEnumerator ReloadGun(float t)
    {
        StartCoroutine(ammoUI.reload(t));
        yield return new WaitForSeconds(.1f);
        aud.Stop();
        aud.PlayOneShot(reload);
        if (Toggles.Animation)
            gunAnimator.SetTrigger("Reload");
        yield return new WaitForSeconds(t-.1f);
        bulletsLeft = maxBullets;
        reloading = false;
    }

    public void RestartLevel()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
