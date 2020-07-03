using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject leftHand;
    [SerializeField] private GameObject righttHand;

    public GameObject LeftHand => leftHand;
    public GameObject RightHand => leftHand;

    private RuneController runeController;

    private void Awake()
    {
        runeController = GetComponent<RuneController>();
    }


    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger))
        {
            Debug.Log("Select rune");
            runeController.SelectRune(0);
        }
        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        {
            Debug.Log("Unselect rune");
            runeController.DeactivateRune();
        }
    }
}
