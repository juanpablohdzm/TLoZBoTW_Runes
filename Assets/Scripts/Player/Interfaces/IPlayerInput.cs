using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerInput 
{
   Vector2 LeftThumbStick { get; }
   Vector2 RightThumbStick { get; }
   bool Confirm { get;}
}