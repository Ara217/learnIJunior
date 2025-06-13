using UnityEngine;

public class Spawner : MonoBehaviour
{

    [SerializeField] private Transform mainChar;
    [SerializeField] private GameObject prefab;       // Префаб объекта
    [SerializeField] private int count = 10;          // Сколько объектов
    [SerializeField] private float radius = 5f;       // Радиус круга
    [SerializeField] private float startAngle = 0f;   // Начальный угол в градусах
    [SerializeField] private float arcLength = 90f;   // Длина дуги (например, 90° — четверть круга)
    [SerializeField] private float minRadius = 3f;      // Минимальное расстояние появления
    [SerializeField] private float maxRadius = 8f;
    [SerializeField] private float moveSpeed = 3f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnObjectsInArc();
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    void SpawnObjectsInArc()
    {
        for (int i = 0; i < count; i++)
        {
            float angleDeg = Random.Range(0f, 360f);
            float angleRad = angleDeg * Mathf.Deg2Rad;

            // Генерируем случайный радиус в пределах заданного диапазона
            float randomRadius = Random.Range(minRadius, maxRadius);

            // Вычисляем позицию по окружности с этим радиусом
            float x = Mathf.Cos(angleRad) * randomRadius;
            float z = Mathf.Sin(angleRad) * randomRadius;

            // Финальная позиция — смещённая от центра (основного объекта)
            Vector3 position = new Vector3(x, 0, z) + mainChar.position;

            // Создаём объект в вычисленной позиции
            GameObject enemy = Instantiate(prefab, position, Quaternion.identity);
            enemy.transform.LookAt(mainChar.position);

            // Добавляем движение к центру (через скрипт на самом враге)
            EnemyMover mover = enemy.AddComponent<EnemyMover>();
            mover.target = mainChar;       // Главный объект — цель
            mover.speed = moveSpeed;
        }
    }
}
