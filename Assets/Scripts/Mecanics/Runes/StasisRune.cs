using UnityEngine;
using UnityEngine.AddressableAssets;

public class StasisRune : Rune
{
    private readonly Player player;
    private readonly int layerMask;
    private readonly RuneController controller;
    private AudioClip countdownSfx;
    private GameObject stasisVfxPrefab;

    private float delayTime = 12.0f;

    
    private RaycastHit[] hits = new RaycastHit[10];
    private Rigidbody interactableRb;
    private IRuneInteractable runeInteractable;
    private bool interactableIsNotNull = false;
    private float activatedTime;
    private ParticleSystem stasisVfx;
    private bool isStasisVfxNotNull = false ;

    #region UnitTestingVariables
#if UNITY_EDITOR
    public float DelayTime
    {
        get => delayTime;
        set => delayTime = value;
    }
    
    
#endif
    #endregion


    public StasisRune(RuneProfile profile, Player player, int layerMask,RuneController controller) : base(profile)
    {
        this.player = player;
        this.layerMask = layerMask;
        this.controller = controller;
        Addressables.LoadAssetAsync<AudioClip>("Assets/Art/Music/Stasis Rune Sound Effect.wav").Completed +=
            handle => countdownSfx = handle.Result;
        Addressables.LoadAssetAsync<GameObject>("Assets/Effects/StasisChains_Particles.prefab").Completed +=
            handle => stasisVfxPrefab = handle.Result;
    }

    public override void ActivateRune()
    {
        IsActive = true;
        stasisVfx = GameObject.Instantiate(stasisVfxPrefab).GetComponent<ParticleSystem>();
        isStasisVfxNotNull = true;
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
                    stasisVfx.gameObject.SetActive(true);
                    stasisVfx.transform.position = interactable.transform.position;
                    stasisVfx.Play();
                    
                    runeInteractable = interactable;
                    runeInteractable.PlaySFX(countdownSfx);
                    
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

        if (isStasisVfxNotNull)
        {
            stasisVfx.gameObject.SetActive(false);
            stasisVfx.Stop();
        }
    }
}