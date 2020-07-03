using System;
using UnityEngine;

public class Magnesis : Rune
{
    private readonly int layerMask;

    private Player player;
    private IRuneInteractable runeInteractable;
    private Rigidbody interactableRigidbody;
    private float speed;

    public Magnesis(RuneProfile profile, Player player,int layerMask,float speed)
    {
        this.player = player;
        this.layerMask = layerMask;
        this.profile = profile;
        this.speed = speed;
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
                    interactableRigidbody.drag = 0.3f;
                    
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
        float controllerDirection = Vector3.Dot(controllerVelocity.normalized, Vector3.up);
        float scalar = Mathf.Clamp(controllerVelocity.magnitude, 0.0f, 1.0f);

        var transform = player.transform;
        interactableRigidbody.velocity = transform.forward * (throttle.y * speed * 0.5f) + 
                                         transform.right * (throttle.x * speed * 0.5f)+
                                         transform.up * (controllerDirection * scalar * speed);
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