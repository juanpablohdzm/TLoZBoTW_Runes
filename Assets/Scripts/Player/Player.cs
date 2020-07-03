using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject leftHand;
    [SerializeField] private GameObject righttHand;

    public GameObject LeftHand => leftHand;
    public GameObject RightHand => leftHand;
}
