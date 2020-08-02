using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRuneMenu : MonoBehaviour
{
    
    [SerializeField] private UIRuneSlotHolder runeSlotHolder;
    [SerializeField] private RuneEvent OnCreatedRune;
    [SerializeField] private RuneEvent OnActivatedRune;
    [SerializeField] private RuneEvent OnDeactivatedRune;

    private RuneController runeController;
    private bool isruneControllerNotNull;

    // Start is called before the first frame update
    void Awake()
    {
        if(runeSlotHolder != null)
            runeSlotHolder.OnRuneSlotSelected += HandleOnRuneSlotSelected;
        if(OnCreatedRune != null)
            OnCreatedRune.AddListener(HandleOnCreatedRune);
        if(OnActivatedRune != null)
            OnActivatedRune.AddListener(HandleOnActivatedRune);
        if(OnDeactivatedRune != null)
            OnDeactivatedRune.AddListener(HandleOnDeactivatedRune);
    }

    private void Start()
    {
        runeController = FindObjectOfType<RuneController>();
        isruneControllerNotNull = runeController != null;
    }

    private void Update()
    {
        Vector2 stick = PlayerInput.Instance.LeftThumbStick;
        if (stick.magnitude > 0.2f)
        {
            runeSlotHolder.gameObject.SetActive(true);
        }
        else
        {
            runeSlotHolder.gameObject.SetActive(false);
        }
    }

    private void HandleOnCreatedRune(Rune rune)
    {
        int? index = runeSlotHolder.FindOpenSlot();
        if (!index.HasValue) return;
        
        runeSlotHolder.SetSlot(rune, index.Value);
    }
    
    private void HandleOnRuneSlotSelected(int index)
    {
        if (isruneControllerNotNull)
            runeController.SelectRune(index);
    }

    private void HandleOnActivatedRune(Rune rune)
    {
        runeSlotHolder.SlotCanBeChanged = false;
    }
    private void HandleOnDeactivatedRune(Rune rune)
    {
        runeSlotHolder.SlotCanBeChanged = true;
    }


    private void OnDestroy()
    {
        if(runeSlotHolder != null)
            runeSlotHolder.OnRuneSlotSelected -= HandleOnRuneSlotSelected;
        if(OnCreatedRune != null)
            OnCreatedRune.RemoveListener(HandleOnCreatedRune);
        if(OnActivatedRune != null)
            OnActivatedRune.RemoveListener(HandleOnActivatedRune);
        if(OnDeactivatedRune != null)
            OnDeactivatedRune.RemoveListener(HandleOnDeactivatedRune);
    }


}