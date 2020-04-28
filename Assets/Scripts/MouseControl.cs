using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseControl : MonoBehaviour
{
    [SerializeField] private float MouseSensivity = 100f;
    [SerializeField] Transform PlayerObject;
    float RotationX;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (PlayerObject == null)
        {
            PlayerObject = gameObject.GetComponentInParent<Transform>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        MouseMovement();
    }

    void MouseMovement()
    {
        float mouseX = Input.GetAxis("Mouse X") * MouseSensivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * MouseSensivity * Time.deltaTime;

        RotationX -= mouseY;
        RotationX = Mathf.Clamp(RotationX, -80f, 80f);

        transform.localRotation = Quaternion.Euler(RotationX , 0f, 0f);
        PlayerObject.Rotate(Vector3.up * mouseX);
    }
}
