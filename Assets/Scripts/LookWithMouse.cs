using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookWithMouse : MonoBehaviour
{
    public float mouseSensitivity = 40f;
    public Transform playerBody;

    [Header("Rotation")]
    public float minRot;
    public float maxRot;
    private float xRotation = 0f;


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, minRot, maxRot); //Mathf.Clamp, método estático, para restringir la rotación. (Limitar)

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);       
    }

}
