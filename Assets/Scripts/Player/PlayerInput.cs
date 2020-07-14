using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerInput : MonoBehaviour, IPlayerInput
{
    public Vector2 LeftThumbStick => OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
    public Vector2 RightThumbStick => OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
    public float GetJoyStickAngle(ControllerType controllerType)
    {
        Vector2 dir = Vector2.zero;
        switch (controllerType)
        {
            case ControllerType.Left:
                dir = LeftThumbStick;
                break;
            case ControllerType.Right:
                dir = RightThumbStick;
                break;
            
        }
        
        float angle = 0;
        if (dir.x > 0 && dir.y >= 0)
            angle = Mathf.Atan(dir.y / dir.x) * Mathf.Rad2Deg;
        else 
        if (dir.x == Mathf.Epsilon && dir.y > 0)
            angle = 90.0f;
        else 
        if (dir.x < 0)
            angle = Mathf.Atan(dir.y / dir.x) * Mathf.Rad2Deg + 180.0f;
        else
        if (dir.x == Mathf.Epsilon && dir.y < 0)
            angle = 270;
        else 
        if (dir.x > 0 && dir.y < 0) 
            angle = Mathf.Atan(dir.y / dir.x) * Mathf.Rad2Deg + 360.0f;

        return angle;
    }
    public bool Confirm => OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger);
    public bool UIRuneConfirm => OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger);
    public Vector3 RightControllerVelocity => OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RHand);
    public Vector3 RightControllerAngularVelocity => OVRInput.GetLocalControllerAngularVelocity(OVRInput.Controller.RHand);
    public Vector3 LeftControllerVelocity => OVRInput.GetLocalControllerVelocity(OVRInput.Controller.LHand);
    public bool ToggleRuneActivation =>OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger);

    public static IPlayerInput Instance { get;  set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            if(Instance != this)
                Destroy(gameObject);
        }
        
        DontDestroyOnLoad(gameObject);
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}