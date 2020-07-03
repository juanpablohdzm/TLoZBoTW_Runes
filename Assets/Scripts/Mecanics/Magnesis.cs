using UnityEngine;

public class Magnesis : Rune
{
    private readonly int layerMask;
    private RaycastHit[] hits = new RaycastHit[100];
    
    private Player player;
    private Rigidbody interactableRigidbody;
    private Rigidbody controllerRigidBody;
    public Magnesis(RuneProfile profile, int layerMask)
    {
        this.layerMask = layerMask;
        this.profile = profile;
    }
    public override void ActivateRune()
    {
        if (player != null)
            player = GameObject.FindObjectOfType<Player>();
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
                hit.SetHighLight();
                if (PlayerInput.Instance.Confirm)
                {
                    hit.SetConfirm();
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
        
        interactableRigidbody.isKinematic = true;
        Vector2 throttle = PlayerInput.Instance.RightThumbStick;
        float controllerSpeed = Vector3.Dot(controllerRigidBody.velocity.normalized, Vector3.up);
        interactableRigidbody.velocity = interactableRigidbody.transform.forward * throttle.y + 
                                         interactableRigidbody.transform.right * throttle.x +
                                         interactableRigidbody.transform.up * controllerSpeed;
    }

    public override void DeactivateRune()
    {
        controllerRigidBody = null;
        interactableRigidbody = null;
        IsActive = false;
    }
}