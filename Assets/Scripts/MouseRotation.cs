using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class MouseRotation : MonoBehaviour
{

    [SerializeField] private float MouseSensitivity = 5f;
    [SerializeField] private string InputManagerTag = "InputManager";

    IInputManager input;

    private void Start()
    {
        input = InputManager.Instance;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        RotateCharacter();
    }


    void RotateCharacter()
    {
        float RotationX = input.GetXRotation * MouseSensitivity;
        float RotationY = input.GetYRotation * MouseSensitivity;

        Quaternion Rotation = transform.rotation *  Quaternion.AngleAxis(RotationY,Vector3.left) 
            * Quaternion.AngleAxis(RotationX, Vector3.up);
        

        transform.eulerAngles = new Vector3(Rotation.eulerAngles.x, Rotation.eulerAngles.y, 0);
    }
}
