using UnityEngine;

public class Magnesis : Rune
{
    private readonly int layerMask;
    private RaycastHit[] hits = new RaycastHit[100];
    
    private Player player;
    private IRuneInteractable runeInteractable;
    private Rigidbody interactableRigidbody;
    private Rigidbody controllerRigidBody;
    public Magnesis(RuneProfile profile, int layerMask, Player player)
    {
        this.player = player;
        this.layerMask = layerMask;
        this.profile = profile;
    }
    public override void ActivateRune()
    {
        controllerRigidBody = player.RightHand.GetComponent<Rigidbody>();
    }

    public override bool ConfirmRune()
    {
        Transform rightHandTransform = player.RightHand.transform;
        int numHit = Physics.RaycastNonAlloc(rightHandTransform.position, rightHandTransform.forward, hits, 30.0f, layerMask);
        
        RaycastHit nearest = new RaycastHit();
        double nearestDistance = double.MaxValue;
        for (int i = 0; i < numHit; i++)
        {
            float distance = Vector3.Distance(rightHandTransform.position, hits[i].point);
            if (distance < nearestDistance)
            {
                nearest = hits[i];
                nearestDistance = distance;
            }
        }
        if (nearest.transform != null)
        {
            var hit = nearest.collider.GetComponent<IRuneInteractable>();
            if (hit != null)
            {
                if (PlayerInput.Instance.Confirm)
                {
                    runeInteractable = hit;
                    interactableRigidbody = hit.GetComponent<Rigidbody>();
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
        
        interactableRigidbody.isKinematic = true;
        Vector2 throttle = PlayerInput.Instance.RightThumbStick;
        float controllerSpeed = Vector3.Dot(controllerRigidBody.velocity.normalized, Vector3.up);
        interactableRigidbody.velocity = interactableRigidbody.transform.forward * throttle.y + 
                                         interactableRigidbody.transform.right * throttle.x +
                                         interactableRigidbody.transform.up * controllerSpeed;
    }

    public override void DeactivateRune()
    {
        if (runeInteractable != null) 
            runeInteractable.SetColor(Color.black);
        controllerRigidBody = null;
        interactableRigidbody = null;
        IsActive = false;
    }
}