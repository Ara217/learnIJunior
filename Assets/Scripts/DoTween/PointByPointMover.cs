using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointByPointMover : MonoBehaviour
{
    private Queue<Vector3> _currentPath;

    [SerializeField] private AnimationCurve _movmentCurve;
    [SerializeField] private AnimationCurve _jumpCurve;
    [SerializeField] private float _speed;
    [SerializeField] private float _coolDownBetweenMoves;
    private Vector3 _currentPoint;

    public void StartWork(IEnumerable<Vector3> path) 
    {
        _currentPath = new Queue<Vector3>(path);
        StartCoroutine(ProcessMove());
    }

    private void SwitchPoint()
    {
        _currentPoint = _currentPath.Dequeue();
        _currentPath.Enqueue(_currentPoint);
    }

    private IEnumerator ProcessMove()
    {
        Vector3 startPoint;
        float progress = 0;

        while (_currentPath.Count > 0) 
        {
            SwitchPoint();
            startPoint = transform.position;

            while (progress < 1)
            {
                progress += Time.deltaTime * _speed;
              /*  transform.position = Vector3.Lerp(startPoint, _currentPoint, progress);
                transform.position = Vector3.Lerp(startPoint, _currentPoint, progress * progress);*/
                transform.position = Vector3.LerpUnclamped(startPoint, _currentPoint, _movmentCurve.Evaluate(progress));
                transform.position = transform.position + Vector3.up * _jumpCurve.Evaluate(progress); 
                yield return null;
            }

            transform.position = _currentPoint;
            progress = 0;
            yield return new WaitForSeconds(_coolDownBetweenMoves);
        }
    }
}
