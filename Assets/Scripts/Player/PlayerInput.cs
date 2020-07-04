using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerInput : MonoBehaviour, IPlayerInput
{
    public Vector2 LeftThumbStick => OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
    public Vector2 RightThumbStick => OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
    public bool Confirm => OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger);
    public bool RuneConfirm => OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger);
    public Vector3 RightControllerVelocity => OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RHand);
    public Vector3 LeftControllerVelocity => OVRInput.GetLocalControllerVelocity(OVRInput.Controller.LHand);
    public bool ToggleRuneActivation =>OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger);

    public static IPlayerInput Instance { get; private set; }

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
}