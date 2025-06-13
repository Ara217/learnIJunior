using UnityEngine;
using UnityEngine.UI;

public class AircraftInputManager : MonoBehaviour
{
    [Header("Selection")]
    [SerializeField] private LayerMask aircraftLayer = 1 << 8; // слой для самолетов
    [SerializeField] private LayerMask groundLayer = 1; // слой земли
    [SerializeField] private Material selectionMaterial;
    [SerializeField] private GameObject targetIndicatorPrefab;

    [Header("UI")]
    [SerializeField] private Text speedText;
    [SerializeField] private Text altitudeText;
    [SerializeField] private Text statusText;

    private Camera playerCamera;
    private AircraftController selectedAircraft;
    private GameObject selectionIndicator;
    private GameObject targetIndicator;

    // Визуальные эффекты
    private Renderer selectedAircraftRenderer;
    private Material originalMaterial;

    void Start()
    {
        playerCamera = Camera.main;
        if (playerCamera == null)
            playerCamera = FindObjectOfType<Camera>();
    }

    void Update()
    {
        HandleSelection();
        HandleMovementCommand();
        UpdateUI();
    }

    private void HandleSelection()
    {
        if (Input.GetMouseButtonDown(1)) // правая кнопка мыши для выбора
        {
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, aircraftLayer))
            {
                AircraftController aircraft = hit.collider.GetComponent<AircraftController>();
                if (aircraft != null)
                {
                    SelectAircraft(aircraft);
                }
            }
            else
            {
                DeselectAircraft();
            }
        }
    }

    private void HandleMovementCommand()
    {
        if (selectedAircraft != null && Input.GetMouseButtonDown(0)) // левая кнопка для команды
        {
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
            {
                // Проверяем, что не кликнули по UI
                if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
                {
                    GiveMovementCommand(hit.point);
                }
            }
        }
    }

    private void SelectAircraft(AircraftController aircraft)
    {
        // Снимаем выделение с предыдущего самолета
        DeselectAircraft();

        selectedAircraft = aircraft;

        // Добавляем визуальное выделение
        selectedAircraftRenderer = aircraft.GetComponent<Renderer>();
        if (selectedAircraftRenderer != null && selectionMaterial != null)
        {
            originalMaterial = selectedAircraftRenderer.material;
            selectedAircraftRenderer.material = selectionMaterial;
        }

        // Создаем индикатор выделения
        if (selectionIndicator == null)
        {
            selectionIndicator = CreateSelectionIndicator();
        }

        selectionIndicator.transform.SetParent(aircraft.transform);
        selectionIndicator.transform.localPosition = Vector3.zero;
        selectionIndicator.SetActive(true);

        Debug.Log($"Selected aircraft: {aircraft.name}");
    }

    private void DeselectAircraft()
    {
        if (selectedAircraft != null)
        {
            // Восстанавливаем оригинальный материал
            if (selectedAircraftRenderer != null && originalMaterial != null)
            {
                selectedAircraftRenderer.material = originalMaterial;
            }

            selectedAircraft = null;
            selectedAircraftRenderer = null;
            originalMaterial = null;
        }

        // Скрываем индикатор выделения
        if (selectionIndicator != null)
        {
            selectionIndicator.SetActive(false);
        }
    }

    private void GiveMovementCommand(Vector3 targetPosition)
    {
        if (selectedAircraft == null) return;

        // Вызываем публичный метод SetTarget
        selectedAircraft.SetTarget(targetPosition);

        // Показываем индикатор цели
        ShowTargetIndicator(targetPosition);

        Debug.Log($"Aircraft commanded to move to: {targetPosition}");
    }

    private void ShowTargetIndicator(Vector3 position)
    {
        if (targetIndicator == null && targetIndicatorPrefab != null)
        {
            targetIndicator = Instantiate(targetIndicatorPrefab);
        }

        if (targetIndicator != null)
        {
            targetIndicator.transform.position = position;
            targetIndicator.SetActive(true);

            // Автоматически скрываем через несколько секунд
            StartCoroutine(HideTargetIndicatorAfterDelay(3f));
        }
    }

    private System.Collections.IEnumerator HideTargetIndicatorAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (targetIndicator != null)
        {
            targetIndicator.SetActive(false);
        }
    }

    private GameObject CreateSelectionIndicator()
    {
        GameObject indicator = new GameObject("SelectionIndicator");

        // Создаем кольцо под самолетом
        GameObject ring = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        ring.transform.SetParent(indicator.transform);
        ring.transform.localPosition = new Vector3(0, -2f, 0);
        ring.transform.localScale = new Vector3(8f, 0.1f, 8f);

        // Убираем коллайдер
        Destroy(ring.GetComponent<Collider>());

        // Настраиваем материал
        Renderer ringRenderer = ring.GetComponent<Renderer>();
        ringRenderer.material = new Material(Shader.Find("Standard"));
        ringRenderer.material.color = Color.green;
        ringRenderer.material.SetFloat("_Mode", 3); // Transparent mode
        ringRenderer.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        ringRenderer.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        ringRenderer.material.SetInt("_ZWrite", 0);
        ringRenderer.material.DisableKeyword("_ALPHATEST_ON");
        ringRenderer.material.EnableKeyword("_ALPHABLEND_ON");
        ringRenderer.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        ringRenderer.material.renderQueue = 3000;

        Color color = ringRenderer.material.color;
        color.a = 0.5f;
        ringRenderer.material.color = color;

        // Добавляем анимацию
        SelectionRingAnimator animator = indicator.AddComponent<SelectionRingAnimator>();

        indicator.SetActive(false);
        return indicator;
    }

    private void UpdateUI()
    {
        if (selectedAircraft != null)
        {
            if (speedText != null)
                speedText.text = $"Speed: {selectedAircraft.GetCurrentSpeed():F1} m/s";

            if (altitudeText != null)
                altitudeText.text = $"Altitude: {selectedAircraft.transform.position.y:F0} m";

            if (statusText != null)
            {
                string status = selectedAircraft.IsMoving() ? "Moving" : "Idle";
                statusText.text = $"Status: {status}";
            }
        }
        else
        {
            if (speedText != null) speedText.text = "Speed: --";
            if (altitudeText != null) altitudeText.text = "Altitude: --";
            if (statusText != null) statusText.text = "Status: No aircraft selected";
        }
    }

    void OnGUI()
    {
        // Простой GUI для отладки
        if (selectedAircraft == null)
        {
            GUI.Label(new Rect(10, 10, 300, 20), "Right-click to select aircraft");
            GUI.Label(new Rect(10, 30, 300, 20), "Left-click to command movement");
        }
        else
        {
            GUI.Label(new Rect(10, 10, 300, 20), $"Selected: {selectedAircraft.name}");
            GUI.Label(new Rect(10, 30, 300, 20), "Left-click to command movement");
            GUI.Label(new Rect(10, 50, 300, 20), $"Speed: {selectedAircraft.GetCurrentSpeed():F1} m/s");
        }
    }
}

// Компонент для анимации кольца выделения
public class SelectionRingAnimator : MonoBehaviour
{
    private float rotationSpeed = 30f;
    private float pulseSpeed = 2f;
    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.GetChild(0).localScale;
    }

    void Update()
    {
        // Вращение
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);

        // Пульсация
        float pulse = 1f + Mathf.Sin(Time.time * pulseSpeed) * 0.1f;
        Transform ring = transform.GetChild(0);
        ring.localScale = new Vector3(originalScale.x * pulse, originalScale.y, originalScale.z * pulse);
    }
}