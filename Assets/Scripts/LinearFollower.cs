using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearFollower : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private float _speed = 5f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /* var direction = (_player.position - transform.position).normalized; //normalized because as far player from follower as faster moves follower
         transform.Translate(direction * _speed);*/


        transform.position = Vector3.MoveTowards(transform.position, _player.position, _speed * Time.deltaTime);
    }
}
