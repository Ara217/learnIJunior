
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowGranade : MonoBehaviour
{
    [SerializeField] private Transform _cam;
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private GameObject _objectToThrow;

    [SerializeField] private float _throwForce;
    [SerializeField] private float _throweUpwordForce;

    public void Throw(Transform playerPosition)
    {
        GameObject projectile = Instantiate(_objectToThrow, _attackPoint.position, _cam.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        Vector3 forceDirection = _cam.transform.forward;

        RaycastHit hit;

        //calculcate diff between cam forward and attack point
        if (Physics.Raycast(_cam.position, _cam.forward, out hit, 500f)) 
        {
            forceDirection = (hit.point - _attackPoint.position).normalized;
        }


        Vector3 forceToAdd = forceDirection * _throwForce + transform.up * _throweUpwordForce;// second part to add angle to throw

        rb.AddForce(forceToAdd, ForceMode.Impulse);
    }

    public void OnCollisionEnter(Collision collision)
    {
    }
}
