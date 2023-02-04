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
        ammoIcons[curIndex].SetActive(false);
        curIndex++;
    }

    public void reload()
    {
        foreach(var a in ammoIcons)
        {
            a.SetActive(true);
        }
        curIndex = 0;
    }
}
