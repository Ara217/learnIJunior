using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[SelectionBase]
public class Leader : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private Transform[] _waypoints;
    [SerializeField] private float _speed;

    private int _currentWaypoint = 0;

    // Update is called once per frame
    void Update()
    {   
        if (Math.Floor(Vector3.Distance(transform.position, _waypoints[_currentWaypoint].position)) == 0)
        {
            _currentWaypoint = (_currentWaypoint + 1) % _waypoints.Length;
            transform.LookAt(_waypoints[_currentWaypoint], transform.up);

           /* Vector3 newDirection = Vector3.RotateTowards(transform.forward, _waypoints[_currentWaypoint].position - transform.position, 1.0f, 0.0f);
            // Draw a ray pointing at our target in
            Debug.DrawRay(transform.position, newDirection, Color.red);

            // Calculate a rotation a step closer to the target and applies rotation to this object
            transform.rotation = Quaternion.LookRotation(newDirection);*/
        }
        transform.position = Vector3.MoveTowards(transform.position, _waypoints[_currentWaypoint].position, _speed * Time.deltaTime);
    }
}
