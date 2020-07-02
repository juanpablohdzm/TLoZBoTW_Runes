using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerInput : MonoBehaviour, IPlayerInput
{
    public Vector2 LeftThumbStick => OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
    public Vector2 RightThumbStick => OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
}