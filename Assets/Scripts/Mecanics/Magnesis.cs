using System;
using UnityEngine;

public class Magnesis : Rune
{
    private readonly int layerMask;

    private Player player;
    private IRuneInteractable runeInteractable;
    private Rigidbody interactableRigidbody;
    public Magnesis(RuneProfile profile, int layerMask, Player player)
    {
        this.player = player;
        this.layerMask = layerMask;
        this.profile = profile;
    }
    public override void ActivateRune()
    {
    }

    public override bool ConfirmRune()
    {
        Transform rightHandTransform = player.RightHand.transform;

        RaycastHit hit;
        if(Physics.Raycast(rightHandTransform.position,rightHandTransform.forward,out hit,100.0f,layerMask))
        {
            var interactable = hit.collider.GetComponent<IRuneInteractable>();
            if (interactable != null)
            {
                if (PlayerInput.Instance.Confirm)
                {
                    runeInteractable = interactable;
                    interactableRigidbody = interactable.GetComponent<Rigidbody>();
                    
                    interactableRigidbody.useGravity = false;
                    interactableRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
                    
                    IsActive = true;
                    return true;
                }
            }
        }

        return false;
    }

    public override void UseRune()
    {

        if(interactableRigidbody == null) return;
        
        runeInteractable.SetColor(Color.yellow);
        
        Vector2 throttle = PlayerInput.Instance.RightThumbStick;
        Vector3 controllerVelocity = PlayerInput.Instance.RightControllerVelocity;
        float controllerSpeed = Vector3.Dot(controllerVelocity.normalized, Vector3.up);
        float magnitude = Mathf.Clamp(controllerVelocity.magnitude, 0.0f, 1.0f);
        
        interactableRigidbody.velocity = interactableRigidbody.transform.forward * throttle.y + 
                                         interactableRigidbody.transform.right * throttle.x +
                                         interactableRigidbody.transform.up * (controllerSpeed * magnitude);
    }

    public override void DeactivateRune()
    {
        if (runeInteractable != null) 
            runeInteractable.SetColor(Color.black);

        if (interactableRigidbody != null)
        {
            interactableRigidbody.useGravity = true;
            interactableRigidbody.constraints = RigidbodyConstraints.None;
        }
        interactableRigidbody = null;
        IsActive = false;
    }
}