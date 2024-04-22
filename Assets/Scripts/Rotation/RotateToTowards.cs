using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToTowards : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float _speed = 5f;

    // Update is called once per frame
    private void Update()
    {
        IEnumerator coroutine = RotateToAngle();
        StartCoroutine(coroutine);
    }

    public IEnumerator RotateToAngle()
    {
        Quaternion target = this.target.transform.rotation * Quaternion.Euler(0, 90, 0);

        while (Quaternion.Angle(transform.rotation, target) > 0.01f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, target, _speed * Time.deltaTime);
            yield return null;
        }

        transform.rotation = target;
    }
}
