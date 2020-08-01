using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class CryonisRune : Rune
{
    private readonly Player player;
    private readonly LayerMask layerMask;
    private readonly RuneController controller;
    
    private GameObject targetPrefab;
    private GameObject iceBlockPrefab;
    private RaycastHit[] hits;
    private GameObject target;
    private IceBlock[] blocks;
    private int currentIndex = -1;

    public CryonisRune(RuneProfile profile,Player player,LayerMask layerMask, RuneController controller) : base(profile)
    {
        this.player = player;
        this.layerMask = layerMask;
        Addressables.LoadAssetAsync<GameObject>("Assets/Prefabs/Miscellaneous/Target.prefab").Completed +=
            handle => { targetPrefab = handle.Result;};
        Addressables.LoadAssetAsync<GameObject>("Assets/Prefabs/Miscellaneous/Iceblock.prefab").Completed +=
            handle => { iceBlockPrefab = handle.Result;};
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
        blocks[currentIndex].transform.position = target.transform.position;
        controller.DeactivateRune();
    }

    private void GetNextIceBlock()
    {
        currentIndex = (currentIndex + 1)% blocks.Length;
        if (blocks[currentIndex] == null)
        {
            blocks[currentIndex] = GameObject.Instantiate(iceBlockPrefab, target.transform.position, Quaternion.identity).GetComponent<IceBlock>();
        }
        else
        {
            blocks[currentIndex].gameObject.SetActive(false);
        }
    }

    public override void DeactivateRune()
    {
        if(target != null)
            GameObject.DestroyImmediate(target);
        target = null;
        IsActive = false;
        IsRunning = false;
    }
}