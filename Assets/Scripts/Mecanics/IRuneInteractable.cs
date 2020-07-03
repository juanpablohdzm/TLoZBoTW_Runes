using System;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody))]
public class IRuneInteractable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private RuneEvent OnRuneSelected;
    [SerializeField] private RuneEvent OnRuneConfirmed;
    [SerializeField] private RuneEvent OnRuneDeactivated;
    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

    private Material mat;
    
    private Color highlightColor;
    private bool RuneIsActive = false;
    private Color previousColor;

    private void Awake()
    {
        OnRuneSelected.AddListener(HandleRuneSelected);
        OnRuneConfirmed.AddListener(HandleRuneConfirmed);
        OnRuneDeactivated.AddListener(HandleRuneDeactivated);
        mat = GetComponent<Renderer>().material;
    }

   
    public virtual void SetColor(Color color)
    {
        if (color == previousColor) return;
        mat.SetColor(EmissionColor, color);
    }

    private void Deactivate()
    {
        RuneIsActive = false;
        mat.SetColor(EmissionColor, Color.black);
    }
    
    private void HandleRuneSelected(Rune rune)
    {
        RuneIsActive = true;
        highlightColor = rune.Profile.Color;
        mat.SetColor(EmissionColor, highlightColor);
    }

    private void HandleRuneConfirmed(Rune rune)
    {
        Deactivate();
    }
    
    private void HandleRuneDeactivated(Rune rune)
    {
        Deactivate();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(RuneIsActive)
            mat.SetColor(EmissionColor,Color.yellow);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(RuneIsActive)
            mat.SetColor(EmissionColor,highlightColor);
            
    }

    private void OnDestroy()
    {
        OnRuneSelected.RemoveListener(HandleRuneSelected);
        OnRuneConfirmed.RemoveListener(HandleRuneConfirmed);
        OnRuneDeactivated.RemoveListener(HandleRuneDeactivated);
    }
}