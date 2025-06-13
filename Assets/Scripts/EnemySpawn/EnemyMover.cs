using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    public Transform target;    // Цель (главный объект)
    public float speed = 3f;    // Скорость движения

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    void Update()
    {
        if (target == null) return;

        // Движение к цели
        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        // Повернуть в сторону цели (если надо обновлять поворот)

        transform.LookAt(target);
    }
}
