using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoUI : MonoBehaviour
{
    public GameObject[] ammoIcons;
    int curIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void removeAmmo()
    {
        ammoIcons[curIndex].GetComponent<Animation>().Play("Ammo_Drop");
        //ammoIcons[curIndex].SetActive(false);
        curIndex++;
    }

    public IEnumerator reload(float t)
    {
        while(curIndex < ammoIcons.Length)
        {
            removeAmmo();
        }
        yield return new WaitForSeconds(t);
        foreach(var a in ammoIcons)
        {
            a.GetComponent<Animation>().Play("Ammo_Back");
            //a.SetActive(true);
        }
        curIndex = 0;
    }
}
