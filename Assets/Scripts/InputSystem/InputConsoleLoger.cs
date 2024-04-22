using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputConsoleLoger : MonoBehaviour
{
    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
/*
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");*/
        //Debug.LogFormat("Horizontal {0}, Vertical{1}", horizontal, vertical);
    }
}
