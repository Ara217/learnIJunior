using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private ThrowGranade _throwGranade;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _jumoForce;
    [SerializeField] private float _jumpHeight;
    [SerializeField] private float _gravityScale;
    [SerializeField] private Transform _feet;
    [SerializeField] private LayerMask _LayerMask;

    private float _velocity;
    private bool _isGrounded;
    private Collider[] _results = new Collider[1];

    private const string Horizontal = nameof(Horizontal);
    private const string Vertical = nameof(Vertical);
    private Rigidbody rb;

    public KeyCode throwKey = KeyCode.Mouse0;
    public KeyCode jumpKey = KeyCode.Space;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        JumpWithoutPhysics();
        Move();
        Rotate();
        //JumpByPhysics()

        if (Input.GetKeyDown(throwKey))
        {
            _throwGranade.Throw(transform);
        }


    }

    private void JumpWithoutPhysics()
    {
        _velocity += Physics.gravity.y * _gravityScale * Time.deltaTime;
        _results = Physics.OverlapBox(_feet.position, _feet.localScale, Quaternion.identity, _LayerMask);
        Debug.Log($"vel {_velocity}, {_results}");
        if (_results.Length > 0 && _velocity < 0)
        {
            Debug.Log($"is grounded {_velocity}");
            _velocity = 0;
            _isGrounded = true;
        }
        else 
        {
            _isGrounded = false;
        }


        if (Input.GetKeyDown(jumpKey) && _isGrounded)
        {
            _velocity = Mathf.Sqrt(_jumpHeight * -2 * (Physics.gravity.y * _gravityScale));
        }

        transform.Translate(new Vector3(0, _velocity, 0) * Time.deltaTime);

    }


    private void JumpByPhysics()
    {
        if (Input.GetKeyDown(jumpKey))
        {
            rb.AddForce(Vector3.up * 20, ForceMode.Impulse);
        }
        
    }

    private void Rotate()
    {
        float rotation = Input.GetAxis(Horizontal);
        transform.Rotate(rotation * _rotationSpeed * Time.deltaTime * Vector3.up);
    }

    private void Move()
    {
        float move = Input.GetAxis(Vertical);
        transform.Translate(move * _moveSpeed * Time.deltaTime * Vector3.forward);
    }
}
