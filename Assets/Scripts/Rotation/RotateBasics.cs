using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBasics : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //transform.eulerAngles = new Vector3(0, 10, 0); //do not use this method of rotation
        Vector3 rotation = transform.eulerAngles;//get rotation 

    }

    // Update is called once per frame
    void Update()
    {

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(70, 180, 0), Time.deltaTime);
        /*if (Input.GetKeyDown(KeyCode.Space)) {
            transform.Rotate(Vector3.up, 90);
        }*/

        //transform.Rotate(5 * Time.deltaTime, 10 * Time.deltaTime, 7 * Time.deltaTime);

    }
}
