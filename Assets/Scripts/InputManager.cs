using System.Collections;
using System.Collections.Generic;
using UnityEngine;


interface IInputManager
{ 
    float GetXRotation { get; }
    float GetYRotation { get; }
    bool GetRightMouseButton { get; }
    bool GetShootButton(bool isHandGun);
    bool GetScrollWheel { get; }
    bool GetReloadButton { get; }
}

public class InputManager : MonoBehaviour, IInputManager
{
    [SerializeField] private string MouseX;
    [SerializeField] private string MouseY;
    [SerializeField] private int RightMouseButton = 1;
    [SerializeField] private int LeftMouseButton = 0;
    [SerializeField] private string MouseScrollWheel;
    [SerializeField] private string ReloadButton;

    [HideInInspector] public static InputManager Instance;
    
    private void Awake()
    {
        Instance = this;       
    }
    public float GetXRotation
    {
        get { return Input.GetAxis(MouseX); }
    }

    public float GetYRotation
    {
        get { return Input.GetAxis(MouseY); }
    }

    public bool GetRightMouseButton
    {
        get { return Input.GetMouseButton(RightMouseButton); }
    }

    public bool GetShootButton(bool isHandGun)
    {
        if (isHandGun)
        {
            return Input.GetButtonDown("Fire1");
        }
        else
            return Input.GetMouseButton(LeftMouseButton);
    }

    public bool GetScrollWheel
    {
        get { return Input.GetAxisRaw(MouseScrollWheel) != 0; }
    }

    public bool GetReloadButton
    {
        get { return Input.GetButtonDown(ReloadButton); }
    }
}
