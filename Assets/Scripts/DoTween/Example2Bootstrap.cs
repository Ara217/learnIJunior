using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;

public class Example2Bootstrap : MonoBehaviour
{
    [SerializeField] private List<Transform> _points;
    [SerializeField] private PointByPointMover _mover;
    private void Awake()
    {
        _mover.StartWork(_points.Select(p => p.position));
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
