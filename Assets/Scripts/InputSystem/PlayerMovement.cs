using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private const string Horizontal = nameof(Horizontal);
    private const string Vertical = nameof(Vertical);

    [SerializeField] private float _speed;
    [SerializeField] private float _rotationSpeed;

    private void Update()
    {
        Rotate();
        Move();
    }

    private void Rotate()
    { 
        float rotation = Input.GetAxis(Horizontal);
        transform.Rotate(rotation * _rotationSpeed * Time.deltaTime * Vector3.up);
    }

    private void Move()
    { 
        float direction = Input.GetAxis(Vertical);
        float distance = direction * _speed * Time.deltaTime;
        transform.Translate(distance * Vector3.forward);
    }
}
