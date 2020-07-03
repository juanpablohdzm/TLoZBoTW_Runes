using UnityEngine;

public class ScanEffectController : MonoBehaviour
{
    [SerializeField] private RuneEvent OnRuneSelected;
    [SerializeField] private RuneEvent OnRuneConfirmed;
    [SerializeField] private Material scanMaterial;
    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");
    private static readonly int Activate = Shader.PropertyToID("_Activate");

    private void Awake()
    {
        OnRuneSelected.AddListener(HandleRuneSelected);
        OnRuneConfirmed.AddListener(HandleRuneConfirmed);
    }

    private void HandleRuneSelected(Rune rune)
    {
        scanMaterial.SetColor(EmissionColor,rune.Profile.Color);
        scanMaterial.SetFloat(Activate,1.0f);
    }
    private void HandleRuneConfirmed(Rune rune)
    {
        scanMaterial.SetFloat(Activate,0.0f);
        
    }


    private void OnDestroy()
    {
        OnRuneSelected.RemoveListener(HandleRuneSelected);
        OnRuneConfirmed.RemoveListener(HandleRuneConfirmed);
    }
}
