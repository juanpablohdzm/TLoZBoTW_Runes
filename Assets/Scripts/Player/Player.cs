using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject leftHand;
    [SerializeField] private GameObject rightHand;

    public GameObject LeftHand => leftHand;
    public GameObject RightHand => rightHand;

    private RuneController runeController;

    private void Awake()
    {
        runeController = GetComponent<RuneController>();
    }
}
