using System;
using System.Collections ;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RotateAround : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Quaternion rotation;

    void Start()
    {
      /*  Vector3 originalVector = target.forward * 5;
        Vector3 offset = Quaternion.Euler(0, 30, 0) * originalVector;
        transform.position = target.position + offset;*/
    }

    // Update is called once per frame
    void Update()
    {
        RotateAroundWithOffset();
    }

    public void RotateAroundWithOffset()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 originalVector = target.forward * 5;
            Vector3 offset = Quaternion.Euler(0, 80, 0) * originalVector;
            transform.position = target.position + offset;
        }
    }

   
}
