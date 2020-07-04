﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRuneMenu : MonoBehaviour
{
    
    [SerializeField] private UIRuneSlotHolder runeSlotHolder;
    [SerializeField] private RuneEvent OnCreatedRune;

    private RuneController runeController;

    // Start is called before the first frame update
    void Awake()
    {
        runeSlotHolder.OnRuneSlotSelected += HandleOnRuneSlotSelected;
        OnCreatedRune.AddListener(HandleOnCreatedRune);
    }

    private void Start()
    {
        runeController = FindObjectOfType<RuneController>();
    }

    private void HandleOnRuneSlotSelected(int index)
    {
        runeController.SelectRune(index);
    }

    private void FixedUpdate()
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

    private void OnDestroy()
    {
        runeSlotHolder.OnRuneSlotSelected -= HandleOnRuneSlotSelected;
        OnCreatedRune.AddListener(HandleOnCreatedRune);
    }
}