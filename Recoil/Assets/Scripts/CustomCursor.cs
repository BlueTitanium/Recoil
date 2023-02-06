using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomCursor : MonoBehaviour
{
    public Vector3 mDisplacement;
    public Animation a;
    public Image crosshair;
    void Start()
    {
        // this sets the base cursor as invisible
        Cursor.visible = false;
    }

    void Update()
    {
        if (Cursor.visible)
        {
            Cursor.visible = false;
        }
        transform.position = Input.mousePosition + mDisplacement;
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            if (Toggles.Animation)
                a.Play();
            GetComponent<AudioSource>().Play();
        }
        crosshair.fillAmount = (PlayerController.p.cooldown - PlayerController.p.cooldownLeft) / PlayerController.p.cooldown;
    }
}