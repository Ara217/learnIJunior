using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class ChestOpenTrigger : MonoBehaviour
{
    [SerializeField] private Chest _chest;

    private bool _isOpen;
    private bool _hasOpener;

    private void Start()
    {
        GetComponent<Rigidbody>().isKinematic = true;
        _chest = GetComponentInParent<Chest>();
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.GetComponent<ChestOpener>()) 
        {
            _hasOpener = true;  
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<ChestOpener>())
        {
            _hasOpener = false;
        }
    }

    private void Update()
    {
        if (_isOpen)
        {
            return;
        }

        if (_hasOpener && Input.GetKeyDown(KeyCode.E)) 
        {
            _chest.Open();
            _isOpen = true;
        }


    }
}
