using UnityEngine;
using System.Collections;

public class AircraftController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float cruiseSpeed = 80f;
    [SerializeField] private float maxSpeed = 120f;
    [SerializeField] private float minSpeed = 40f;
    [SerializeField] private float acceleration = 20f;
    [SerializeField] private float deceleration = 30f;

    [Header("Turn Settings")]
    [SerializeField] private float maxTurnRate = 45f; // градусов в секунду при максимальной скорости
    [SerializeField] private float minTurnRate = 90f; // градусов в секунду при минимальной скорости
    [SerializeField] private float maxBankAngle = 60f;
    [SerializeField] private float bankSpeed = 90f; // скорость изменения банка

    [Header("Altitude Settings")]
    [SerializeField] private float cruiseAltitude = 100f;
    [SerializeField] private float climbRate = 15f;
    [SerializeField] private float diveRate = 25f;

    [Header("Physics")]
    [SerializeField] private float turnRadius = 50f; // минимальный радиус поворота
    [SerializeField] private float stallSpeed = 30f; // скорость сваливания
    [SerializeField] private AnimationCurve turnRateCurve = AnimationCurve.Linear(0, 1, 1, 0.3f);

    [Header("Input")]
    [SerializeField] private LayerMask groundLayer = 1;
    [SerializeField] private bool showDebugInfo = true;

    // Состояние полета
    private Vector3 targetPosition;
    private bool hasTarget = false;
    private float currentSpeed;
    private float targetSpeed;
    private Vector3 velocity;

    // Поворот и банк
    private float currentBankAngle = 0f;
    private float targetBankAngle = 0f;
    private float currentTurnRate;

    // Высота
    private float targetAltitude;

    // Для плавного поворота
    private Vector3 currentDirection;
    private Vector3 targetDirection;

    // Камера для ray cast
    private Camera playerCamera;

    void Start()
    {
        playerCamera = Camera.main;
        if (playerCamera == null)
            playerCamera = FindObjectOfType<Camera>();

        // Инициализация
        currentSpeed = cruiseSpeed;
        targetSpeed = cruiseSpeed;
        targetAltitude = cruiseAltitude;
        currentDirection = transform.forward;

        // Поднимаем самолет на нужную высоту
        Vector3 pos = transform.position;
        pos.y = cruiseAltitude;
        transform.position = pos;
    }

    void Update()
    {
        HandleInput();
        UpdateMovement();
        UpdateRotation();
        UpdateAltitude();

        if (showDebugInfo)
        {
            DrawDebugInfo();
        }
    }

    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
            {
                SetTarget(hit.point);
            }
        }
    }

    private void UpdateMovement()
    {
        if (!hasTarget) return;

        // Вычисляем расстояние до цели
        Vector3 targetPos = targetPosition;
        targetPos.y = transform.position.y; // игнорируем высоту для расчета направления
        Vector3 currentPos = transform.position;
        currentPos.y = transform.position.y;

        float distanceToTarget = Vector3.Distance(currentPos, targetPos);

        // Определяем целевое направление
        if (distanceToTarget > 5f)
        {
            targetDirection = (targetPos - currentPos).normalized;
        }
        else
        {
            // Достигли цели
            hasTarget = false;
            targetSpeed = minSpeed; // замедляемся
            return;
        }

        // Вычисляем текущую скорость поворота на основе скорости самолета
        float speedRatio = (currentSpeed - minSpeed) / (maxSpeed - minSpeed);
        speedRatio = Mathf.Clamp01(speedRatio);
        currentTurnRate = Mathf.Lerp(minTurnRate, maxTurnRate, turnRateCurve.Evaluate(speedRatio));

        // Плавно поворачиваем направление
        float maxTurnThisFrame = currentTurnRate * Time.deltaTime;
        currentDirection = Vector3.RotateTowards(currentDirection, targetDirection,
            maxTurnThisFrame * Mathf.Deg2Rad, 0f);

        // Вычисляем угол поворота для банка
        float turnAngle = Vector3.SignedAngle(transform.forward, currentDirection, Vector3.up);
        targetBankAngle = Mathf.Clamp(turnAngle * 2f, -maxBankAngle, maxBankAngle);

        // Управление скоростью в зависимости от угла поворота
        float turnSharpness = Mathf.Abs(turnAngle) / 90f; // нормализуем угол поворота
        float speedLoss = turnSharpness * 0.3f; // теряем скорость при резких поворотах

        // Корректируем целевую скорость
        if (distanceToTarget > 100f)
        {
            targetSpeed = maxSpeed * (1f - speedLoss);
        }
        else if (distanceToTarget > 50f)
        {
            targetSpeed = cruiseSpeed * (1f - speedLoss);
        }
        else
        {
            targetSpeed = Mathf.Lerp(minSpeed, cruiseSpeed, distanceToTarget / 50f) * (1f - speedLoss);
        }

        // Плавно изменяем скорость
        if (currentSpeed < targetSpeed)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, deceleration * Time.deltaTime);
        }

        // Проверяем сваливание
        if (currentSpeed < stallSpeed)
        {
            currentSpeed = stallSpeed;
            // Можно добавить эффект сваливания
        }

        // Применяем движение
        velocity = currentDirection * currentSpeed;
        transform.position += velocity * Time.deltaTime;
    }

    private void UpdateRotation()
    {
        // Плавно изменяем банк
        currentBankAngle = Mathf.MoveTowards(currentBankAngle, targetBankAngle, bankSpeed * Time.deltaTime);

        // Создаем ротацию
        Quaternion forwardRotation = Quaternion.LookRotation(currentDirection, Vector3.up);
        Quaternion bankRotation = Quaternion.AngleAxis(currentBankAngle, currentDirection);

        // Применяем ротацию
        transform.rotation = Quaternion.Slerp(transform.rotation, forwardRotation * bankRotation,
            Time.deltaTime * 3f);
    }

    private void UpdateAltitude()
    {
        float currentAlt = transform.position.y;
        float altitudeDifference = targetAltitude - currentAlt;

        if (Mathf.Abs(altitudeDifference) > 2f)
        {
            float climbSpeed = altitudeDifference > 0 ? climbRate : -diveRate;

            // Корректируем скорость набора/снижения высоты в зависимости от скорости
            float speedFactor = currentSpeed / maxSpeed;
            climbSpeed *= speedFactor;

            Vector3 pos = transform.position;
            pos.y += climbSpeed * Time.deltaTime;
            transform.position = pos;
        }
    }

    public void SetTarget(Vector3 target)
    {
        targetPosition = target;
        targetPosition.y = cruiseAltitude; // устанавливаем высоту цели
        hasTarget = true;

        // Если цель далеко, увеличиваем высоту для лучшего обзора
        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);
        if (distanceToTarget > 200f)
        {
            targetAltitude = cruiseAltitude + 20f;
        }
        else
        {
            targetAltitude = cruiseAltitude;
        }
    }

    private void DrawDebugInfo()
    {
        if (hasTarget)
        {
            // Линия к цели
            Debug.DrawLine(transform.position, targetPosition, Color.red);

            // Текущее направление
            Debug.DrawRay(transform.position, currentDirection * 20f, Color.green);

            // Целевое направление
            Debug.DrawRay(transform.position, targetDirection * 15f, Color.yellow);

            // Радиус поворота
            Vector3 perpendicular = Vector3.Cross(currentDirection, Vector3.up).normalized;
            Vector3 turnCenter = transform.position + perpendicular * turnRadius;

            // Рисуем круг поворота (упрощенно)
            for (int i = 0; i < 16; i++)
            {
                float angle1 = i * 22.5f * Mathf.Deg2Rad;
                float angle2 = (i + 1) * 22.5f * Mathf.Deg2Rad;

                Vector3 point1 = turnCenter + new Vector3(Mathf.Cos(angle1), 0, Mathf.Sin(angle1)) * turnRadius;
                Vector3 point2 = turnCenter + new Vector3(Mathf.Cos(angle2), 0, Mathf.Sin(angle2)) * turnRadius;

                Debug.DrawLine(point1, point2, Color.blue, 0.1f);
            }
        }

        // Информация о скорости и банке в консоли (только при изменении)
        if (Time.frameCount % 30 == 0) // каждые полсекунды
        {
            Debug.Log($"Speed: {currentSpeed:F1} m/s, Bank: {currentBankAngle:F1}°, Turn Rate: {currentTurnRate:F1}°/s");
        }
    }

    // Публичные методы
    public void SetCruiseAltitude(float altitude)
    {
        cruiseAltitude = altitude;
        targetAltitude = altitude;
    }

    public void SetMaxSpeed(float speed)
    {
        maxSpeed = speed;
        if (targetSpeed > maxSpeed)
            targetSpeed = maxSpeed;
    }

    public Vector3 GetVelocity()
    {
        return velocity;
    }

    public float GetCurrentSpeed()
    {
        return currentSpeed;
    }

    public bool IsMoving()
    {
        return hasTarget;
    }

    // Для отладки в инспекторе
    void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) return;

        // Цель
        if (hasTarget)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(targetPosition, 5f);
        }

        // Радиус поворота
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, turnRadius);

        // Направление движения
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, currentDirection * 20f);
    }
}