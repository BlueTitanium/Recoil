using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTurretAI : MonoBehaviour
{
     public GameObject bulletPrefab;
     public Transform spawnPoint;
    public ParticleSystem shotEffect;
    public Animation anim;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Shoot());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Shoot()
    {
        yield return new WaitForSeconds(0.15f);
        yield return new WaitUntil(() => !(GameManager.gm.paused || !GameManager.gm.started) && GetComponent<Renderer>().isVisible);
        if (Toggles.Animation)
            anim.Play();
        yield return new WaitForSeconds(1.5f);
        if(Toggles.ParticleEffects)
            shotEffect.Play();
        Instantiate(bulletPrefab, spawnPoint.position, spawnPoint.rotation);
        GetComponent<AudioSource>().Play();
        StartCoroutine(Shoot());
    }
}
