using System;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody))]
public class IRuneInteractable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private RuneEvent OnRuneActivated;
    [SerializeField] private RuneEvent OnRuneConfirmed;
    [SerializeField] private RuneEvent OnRuneDeactivated;
    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

    private Material mat;
    
    private Color highlightColor;
    private bool RuneIsActive = false;
    private bool RuneIsValid = false;
    private Color previousColor;

    private void Awake()
    {
        if (OnRuneActivated != null) 
            OnRuneActivated.AddListener(HandleRuneActivated);
        if (OnRuneConfirmed != null) 
            OnRuneConfirmed.AddListener(HandleRuneConfirmed);
        if (OnRuneDeactivated != null) 
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
        RuneIsValid = false;
        mat.SetColor(EmissionColor, Color.black);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(RuneIsActive && RuneIsValid)
            mat.SetColor(EmissionColor,Color.yellow);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(RuneIsActive && RuneIsValid)
            mat.SetColor(EmissionColor,highlightColor);
            
    }
    
    private void HandleRuneActivated(Rune rune)
    {
        RuneIsValid = rune.Profile.RuneType == RuneType.Magnesis;
        if (!RuneIsValid) return;
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