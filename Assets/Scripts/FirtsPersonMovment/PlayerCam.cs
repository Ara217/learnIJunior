using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{

    [SerializeField] private float sensX;
    [SerializeField] private float sensY;
    [SerializeField] private Transform orentation;

    private float xRotation;
    private float yRotation;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * sensX * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * sensY * Time.deltaTime;

        yRotation += mouseX;
        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f); //to limit axis between -90 to 90 degress

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orentation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
