using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTurretAI : MonoBehaviour
{
     public GameObject bulletPrefab;
     public Transform spawnPoint;

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
        yield return new WaitForSeconds(1.5f);
        yield return new WaitUntil(() => !(GameManager.gm.paused || !GameManager.gm.started) && GetComponent<Renderer>().isVisible);
        Instantiate(bulletPrefab, spawnPoint.position, spawnPoint.rotation);
        StartCoroutine(Shoot());
    }
}
