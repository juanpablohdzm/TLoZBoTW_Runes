using UnityEngine;
using UnityEngine.AddressableAssets;

public class StasisRune : Rune
{
    private readonly Player player;
    private readonly int layerMask;
    private readonly RuneController controller;
    private AudioClip countdownSFX;

    private const float delayTime = 10.0f;

    
    private RaycastHit[] hits = new RaycastHit[10];
    private Rigidbody interactableRb;
    private IRuneInteractable runeInteractable;
    private bool interactableIsNotNull = false;
    private float activatedTime;


    public StasisRune(RuneProfile profile, Player player, int layerMask,RuneController controller) : base(profile)
    {
        this.player = player;
        this.layerMask = layerMask;
        this.controller = controller;
        Addressables.LoadAssetAsync<AudioClip>("Assets/Art/Music/Stasis Rune Sound Effect.wav").Completed +=
            handle => countdownSFX = handle.Result;
    }

    public override void ActivateRune()
    {
        IsActive = true;
    }

    public override bool ConfirmRune()
    {
        Transform rightHandTransform = player.RightHand.transform;
        
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
                    runeInteractable = interactable;
                    runeInteractable.PlaySFX(countdownSFX);
                    interactableRb = interactable.GetComponent<Rigidbody>();
                    interactableIsNotNull = true;
                    activatedTime = Time.realtimeSinceStartup+delayTime;
                    IsRunning = true;
                    return true;
                }
            }
        }

        return false;
    }

    public override void UseRune()
    {
        interactableRb.isKinematic = true;
        runeInteractable.SetColor(Color.yellow);

        if (Time.realtimeSinceStartup >= activatedTime)
        {
            controller.DeactivateRune();
            return;
        }
        

    }

    public override void DeactivateRune()
    {
        IsActive = false;
        IsRunning = false;
        if (interactableIsNotNull)
        {
            interactableIsNotNull = false;
            interactableRb.isKinematic = false;
            runeInteractable.SetColor(Color.black);
            runeInteractable = null;
            interactableRb = null;
        }
        
    }
}