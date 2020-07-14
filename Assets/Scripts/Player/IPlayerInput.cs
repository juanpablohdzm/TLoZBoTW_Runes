using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerInput 
{
   Vector2 LeftThumbStick { get; }
   Vector2 RightThumbStick { get; }
   float GetJoyStickAngle(ControllerType controllerType);
   bool Confirm { get;}
   bool UIRuneConfirm { get; }
   Vector3 RightControllerVelocity { get; }
   Vector3 RightControllerAngularVelocity { get; }
   Vector3 LeftControllerVelocity { get; }
   bool ToggleRuneActivation { get; }
}