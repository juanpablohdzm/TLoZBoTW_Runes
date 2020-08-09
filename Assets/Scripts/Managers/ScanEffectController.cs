using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class ScanEffectController : MonoBehaviour
{
    [SerializeField] private RuneEvent OnRuneActivated;
    [SerializeField] private RuneEvent OnRuneConfirmed;
    [SerializeField] private RuneEvent OnRuneDeactivated;
    [SerializeField] private Material scanMaterial;
    [SerializeField] private AudioClip scanSFX;
    
    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");
    private static readonly int Activate = Shader.PropertyToID("_Activate");

    private AudioSource audioSource;

    private void Awake()
    {
        if (OnRuneActivated != null) 
            OnRuneActivated.AddListener(HandleRuneActivated);
        if (OnRuneConfirmed != null) 
            OnRuneConfirmed.AddListener(HandleRuneConfirmed);
        if (OnRuneDeactivated != null) 
            OnRuneDeactivated.AddListener(HandleRuneDeactivated);

        audioSource = GetComponent<AudioSource>();
        audioSource.clip = scanSFX;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
    }

    private void HandleRuneDeactivated(Rune rune)
    {
        scanMaterial.SetFloat(Activate,0.0f);
        audioSource.Stop();
    }

    private void HandleRuneActivated(Rune rune)
    {
        if (rune.Profile.RuneType == RuneType.RemoteBombBox ||
            rune.Profile.RuneType == RuneType.RemoteBombSphere) return;
        scanMaterial.SetColor(EmissionColor,rune.Profile.Color);
        scanMaterial.SetFloat(Activate,1.0f);
        audioSource.Play();
    }
    private void HandleRuneConfirmed(Rune rune)
    {
        scanMaterial.SetFloat(Activate,0.0f);
        audioSource.Stop();
        
    }


    private void OnDestroy()
    {
        if (OnRuneActivated != null) 
            OnRuneActivated.RemoveListener(HandleRuneActivated);
        if (OnRuneConfirmed != null) 
            OnRuneConfirmed.RemoveListener(HandleRuneConfirmed);
        if (OnRuneDeactivated != null) 
            OnRuneDeactivated.RemoveListener(HandleRuneDeactivated);
    }
}
