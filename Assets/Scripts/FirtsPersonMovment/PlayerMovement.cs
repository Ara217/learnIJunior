using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FerstPersonMove {
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private Transform _orientation;

        private float _horizontalInput;
        private float _verticalInput;
        private const string Horizontal = nameof(Horizontal);
        private const string Vertical = nameof(Vertical);
        private Vector3 _moveDirection;
        private Rigidbody _rb;

        // Start is called before the first frame update
        void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _rb.freezeRotation = true;
        }

        // Update is called once per frame
        void Update()
        {
            _horizontalInput = Input.GetAxis(Horizontal);
            _verticalInput = Input.GetAxis(Vertical);   
        }

        void FixedUpdate() 
        {
            _moveDirection = _verticalInput * _orientation.forward + _horizontalInput * _orientation.right;
            Debug.Log($"forward: {_orientation.forward}, horizontalInput: {_horizontalInput}. verticalInput:{_verticalInput} moveDire3ction:{_moveDirection}");
            _rb.AddForce(_moveDirection * 10f, ForceMode.Force);
        }
    }
}

