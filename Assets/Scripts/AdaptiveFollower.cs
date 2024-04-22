using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class AdaptiveFollower : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private float _speed = 1f;
    // Update is called once per frame
    void Update()
    {
        transform.LookAt(_player.position);
        transform.position = Vector3.Lerp(transform.position, _player.position, _speed * Time.deltaTime);
    }
}
