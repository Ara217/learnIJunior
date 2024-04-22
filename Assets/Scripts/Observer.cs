using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observer : MonoBehaviour
{
    [SerializeField] private GameObject _follower;

    // Update is called once per frame
    void Update()
    {
        var direction = (transform.position - _follower.transform.forward).normalized;
        transform.forward = direction;
    }
}
