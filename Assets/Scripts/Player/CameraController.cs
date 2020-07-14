using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; }
    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
        {
            if (Instance != this)
            {
                throw new Exception("Camera controller has more than one instance");
            }
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}
