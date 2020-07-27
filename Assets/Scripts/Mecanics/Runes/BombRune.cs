using NSubstitute;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class BombRune : Rune
{
    private readonly Player player;
    private readonly RuneController controller;

    private GameObject bombPrefab;
    private Bomb bomb;
    private Rigidbody bombRb;

    public BombRune(RuneProfile runeProfile, Player player, RuneController controller) : base(runeProfile)
    {
        this.player = player;
        this.controller = controller;
        if (runeProfile.RuneType == RuneType.RemoteBombBox)
        {
            Addressables.LoadAssetAsync<GameObject>("Assets/Prefabs/Miscellaneous/BoxBomb.prefab").Completed += 
                handle => { bombPrefab = handle.Result; };
        }
        else
        {
            if(runeProfile.RuneType == RuneType.RemoteBombSphere)
                Addressables.LoadAssetAsync<GameObject>("Assets/Prefabs/Miscellaneous/SphereBomb.prefab").Completed +=
                    handle => { bombPrefab = handle.Result; };
        }
    }
    
    public override void ActivateRune()
    {
        
        bomb = GameObject.Instantiate(bombPrefab).GetComponent<Bomb>();
        
        Transform transform = bomb.transform;
        transform.parent = player.RightHand.transform;
        transform.localPosition = Vector3.zero;
        
        bombRb = bomb.GetComponent<Rigidbody>();
        bombRb.isKinematic = true;
        IsActive = true;
    }

    public override bool ConfirmRune()
    {
        Vector3 velocity = PlayerInput.Instance.RightControllerVelocity*3.0f;
        if (PlayerInput.Instance.Confirm)
        {
            bomb.transform.parent = null;
            bombRb.isKinematic = false;
            bombRb.AddForce(velocity,ForceMode.VelocityChange);
            IsRunning = true;
            return true;
        }

        return false;
    }

    public override void UseRune()
    {
        if (PlayerInput.Instance.Confirm)
        {
            bomb.Explode();
            controller.DeactivateRune();
        }
    }

    public override void DeactivateRune()
    {
        if(bomb != null && !bomb.IsExploding)
            GameObject.Destroy(bomb.gameObject);
        bomb = null;
        bombRb = null;
        IsActive = false;
        IsRunning = false;
    }
}