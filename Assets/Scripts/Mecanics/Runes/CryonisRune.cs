using System;
using UnityEngine;

public class CryonisRune : Rune
{
    private readonly Player player;
    private readonly LayerMask layerMask;
    private readonly GameObject targetPrefab;
    private readonly GameObject iceBlockPrefab;
    private readonly RuneController controller;
    
    private RaycastHit[] hits;
    private GameObject target;
    private IceBlock[] blocks;
    private int currentIndex = -1;

    public CryonisRune(RuneProfile profile,Player player,LayerMask layerMask,GameObject targetPrefab,GameObject iceBlockPrefab, RuneController controller) : base(profile)
    {
        this.player = player;
        this.layerMask = layerMask;
        this.targetPrefab = targetPrefab;
        this.iceBlockPrefab = iceBlockPrefab;
        this.controller = controller;
        hits = new RaycastHit[20];
        blocks = new IceBlock[3];
    }

    public override void ActivateRune()
    {
        IsActive = true;
    }

    public override bool ConfirmRune()
    {
        Transform rightHandTransform = player.RightHand.transform;
        int size = Physics.RaycastNonAlloc(rightHandTransform.position, rightHandTransform.forward, hits, 10.0f, layerMask.value);

        if (size <= 0) return false;
        
        float min = float.MaxValue;
        int index = -1;
        for (int i = 0; i < size; i++)
        {
            if (hits[i].distance < min)
            {
                index = i;
                min = hits[i].distance;
            }

        }
        if (index == -1) return false;

        if (target == null) 
            target = GameObject.Instantiate(targetPrefab, hits[index].point, Quaternion.identity);
        else
        {
            if(!target.activeSelf)
                target.SetActive(true);
            target.transform.position = hits[index].point;
        }

        if (PlayerInput.Instance.Confirm)
        {
            target.SetActive(false);
            IsRunning = true;
            return true;
        }
        return false;
    }

    public override void UseRune()
    {
        GetNextIceBlock();
        blocks[currentIndex].gameObject.SetActive(true);
        controller.DeactivateRune();
    }

    private void GetNextIceBlock()
    {
        currentIndex = (currentIndex + 1)% blocks.Length;
        if (blocks[currentIndex] == null)
        {
            blocks[currentIndex] = GameObject.Instantiate(iceBlockPrefab, target.transform.position, Quaternion.identity).GetComponent<IceBlock>();
        }
    }

    public override void DeactivateRune()
    {
        if(target != null)
            GameObject.DestroyImmediate(target);
        foreach (IceBlock block in blocks)
        {
            block.gameObject.SetActive(false);
        }
        target = null;
        IsActive = false;
        IsRunning = false;
    }
}