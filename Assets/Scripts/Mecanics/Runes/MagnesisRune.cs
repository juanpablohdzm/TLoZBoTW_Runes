using DG.Tweening;
using UnityEngine;
using UnityEngine.AddressableAssets;


public class MagnesisRune : Rune
{
    private readonly Player player;
    private readonly RaycastHit[] hits = new RaycastHit[20];
    private readonly int layerMask;
    private readonly float speed;
    
    private GameObject laserPrefab;
    private IRuneInteractable runeInteractable;
    private Rigidbody interactableRigidbody;
    private Laser laser;
    private bool hasBeenConfirmed = false;
    

    public MagnesisRune(RuneProfile profile, Player player,int layerMask,float speed): base(profile)
    {
        this.player = player;
        this.layerMask = layerMask;
        this.speed = speed;
        Addressables.LoadAssetAsync<GameObject>("Assets/Prefabs/Miscellaneous/Laser.prefab").Completed +=
            handle => { laserPrefab = handle.Result;};
    }
    public override void ActivateRune()
    {
        IsActive = true;
    }

    public override bool ConfirmRune()
    {
        if (hasBeenConfirmed) return false;
        
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
                    hasBeenConfirmed = true;
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
        hasBeenConfirmed = false;
    }
}