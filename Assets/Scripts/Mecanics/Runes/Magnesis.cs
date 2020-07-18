using DG.Tweening;
using UnityEngine;

public class Magnesis : Rune
{
    private readonly int layerMask;
    private readonly GameObject laserPrefab;

    private Player player;
    private IRuneInteractable runeInteractable;
    private Rigidbody interactableRigidbody;
    private float speed;
    private Laser laser;
    private RaycastHit[] hits = new RaycastHit[20];
    

    public Magnesis(RuneProfile profile, Player player,int layerMask,float speed, GameObject laserPrefab): base(profile)
    {
        this.player = player;
        this.layerMask = layerMask;
        this.speed = speed;
        this.laserPrefab = laserPrefab;
    }
    public override void ActivateRune()
    {
        IsActive = true;
    }

    public override bool ConfirmRune()
    {
        Transform rightHandTransform = player.RightHand.transform;

        RaycastHit hit;
        int size = Physics.RaycastNonAlloc(rightHandTransform.position, rightHandTransform.forward, hits, 100.0f, layerMask);
        
        if(size > 0)
        {
            float min = float.MaxValue;
            int index = -1;
            for (int i = 0; i < size; i++)
            {
                if (hits[i].distance < min)
                {
                    index = i;
                    min = hits[i].distance;
                }

            }

            if (index == -1) return false;
            
            IRuneInteractable interactable = hits[index].collider.GetComponent<IRuneInteractable>();
            if (interactable != null)
            {
                if (PlayerInput.Instance.Confirm)
                {
                    laser = GameObject.Instantiate(laserPrefab, rightHandTransform).GetComponent<Laser>();
                    Sequence s = DOTween.Sequence();
                    s.Append(DOTween.To(() => laser.GetPosition(1), x => laser.SetPosition(1, x),
                        laser.transform.InverseTransformPoint(interactable.transform.position), 1));
                    s.OnComplete(()=>
                    {
                        PrepareInteractable(interactable);
                    });
                    return true;
                }
            }
        }

        return false;
    }

    private void PrepareInteractable(IRuneInteractable interactable)
    {
        runeInteractable = interactable;
        interactableRigidbody = interactable.GetComponent<Rigidbody>();
                    
        interactableRigidbody.useGravity = false;
        interactableRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        interactableRigidbody.drag = 0.3f;

        IsRunning = true;
    }

    public override void UseRune()
    {

        if(interactableRigidbody == null) return;
        
        runeInteractable.SetColor(Color.yellow);
        
        
        Vector2 throttle = PlayerInput.Instance.RightThumbStick;
        Vector3 controllerVelocity = PlayerInput.Instance.RightControllerVelocity;
        Transform rightControllerTransform = player.RightHand.transform;
        
        laser.SetPosition(laser.PositionCount-1,laser.transform.InverseTransformPoint(interactableRigidbody.transform.position));
        laser.Direction = -throttle.x;

        Vector3 controllerVelocityNormalized = controllerVelocity.normalized;
        float controllerUpDirection = Vector3.Dot(controllerVelocityNormalized, Vector3.up);
        float controllerRightDirection = Vector3.Dot(controllerVelocityNormalized, rightControllerTransform.right);
        float scalar = Mathf.Clamp(controllerVelocity.magnitude, 0.0f, 1.0f);

        Transform transform = CameraController.Instance.transform;
        interactableRigidbody.velocity = transform.forward * (throttle.y * speed * 0.5f) + 
                                         transform.right * (controllerRightDirection * scalar * speed)+ transform.right *(throttle.x *speed * 0.5f)+
                                         transform.up * (controllerUpDirection * scalar * speed);
    }

    public override void DeactivateRune()
    {
        if (laser != null)
        {
            GameObject.Destroy(laser.gameObject);
            laser = null;
        }

        if (runeInteractable != null) 
            runeInteractable.SetColor(Color.black);

        if (interactableRigidbody != null)
        {
            interactableRigidbody.useGravity = true;
            interactableRigidbody.constraints = RigidbodyConstraints.None;
        }
        interactableRigidbody = null;
        IsActive = false;
        IsRunning = false;
    }
}