            using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character: MonoBehaviour
{
    [SerializeField] private Vector3 _movmentDirection;
    [SerializeField] private GameObject protector;                      

    // Update is called once per frame
    void Update()
    {
        //transform.position += _movmentDirection;
        transform.Translate(_movmentDirection, Space.World);

        if (protector) {
            //protector.transform.Translate(_movmentDirection, Space.World);
        }
        
    }
}
