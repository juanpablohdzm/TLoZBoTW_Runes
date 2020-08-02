using UnityEngine;

public class ScanEffectController : MonoBehaviour
{
    [SerializeField] private RuneEvent OnRuneActivated;
    [SerializeField] private RuneEvent OnRuneConfirmed;
    [SerializeField] private RuneEvent OnRuneDeactivated;
    [SerializeField] private Material scanMaterial;
    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");
    private static readonly int Activate = Shader.PropertyToID("_Activate");

    private void Awake()
    {
        if (OnRuneActivated != null) 
            OnRuneActivated.AddListener(HandleRuneActivated);
        if (OnRuneConfirmed != null) 
            OnRuneConfirmed.AddListener(HandleRuneConfirmed);
        if (OnRuneDeactivated != null) 
            OnRuneDeactivated.AddListener(HandleRuneDeactivated);
    }

    private void HandleRuneDeactivated(Rune rune)
    {
        scanMaterial.SetFloat(Activate,0.0f);
    }

    private void HandleRuneActivated(Rune rune)
    {
        if (rune.Profile.RuneType == RuneType.RemoteBombBox ||
            rune.Profile.RuneType == RuneType.RemoteBombSphere) return;
        scanMaterial.SetColor(EmissionColor,rune.Profile.Color);
        scanMaterial.SetFloat(Activate,1.0f);
    }
    private void HandleRuneConfirmed(Rune rune)
    {
        scanMaterial.SetFloat(Activate,0.0f);
        
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
