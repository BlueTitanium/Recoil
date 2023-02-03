using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    public Vector3 mDisplacement;
    void Start()
    {
        // this sets the base cursor as invisible
        Cursor.visible = false;
    }

    void Update()
    {
        transform.position = Input.mousePosition + mDisplacement;

    }
}