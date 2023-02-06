using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoUI : MonoBehaviour
{
    public GameObject[] ammoIcons;
    public Vector2[] originalSpots;
    public Vector3 originalScale;
    public Color originalColor;
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
        if (Toggles.Animation)
            ammoIcons[curIndex].GetComponent<Animation>().Play("Ammo_Drop");
        else
            ammoIcons[curIndex].SetActive(false);
        curIndex++;
    }

    public IEnumerator reload(float t)
    {
        while(curIndex < ammoIcons.Length)
        {
            removeAmmo();
        }
        yield return new WaitForSeconds(t);
        for(int i = 0; i < ammoIcons.Length; i++)
        {
            if (Toggles.Animation)
                ammoIcons[i].GetComponent<Animation>().Play("Ammo_Back");
            else
            {
                ammoIcons[i].SetActive(true);
                ammoIcons[i].transform.localPosition = originalSpots[i];
                ammoIcons[i].transform.localScale = originalScale;
                ammoIcons[i].GetComponent<Image>().color = originalColor;
            }
        }
        curIndex = 0;
    }
}
