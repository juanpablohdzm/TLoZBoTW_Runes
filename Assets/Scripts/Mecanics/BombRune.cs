using UnityEngine;

public class BombRune : Rune
{
    private readonly Player player;
    private readonly GameObject prefab;
    private readonly RuneController controller;

    private Bomb bomb;
    private Rigidbody bombRb;

    public BombRune(RuneProfile runeProfile, Player player, GameObject prefab, RuneController controller)
    {
        this.profile = runeProfile;
        this.player = player;
        this.prefab = prefab;
        this.controller = controller;
    }
    
    public override void ActivateRune()
    {
        bomb = Object.Instantiate(prefab, player.RightHand.transform).GetComponent<Bomb>();
        bombRb = bomb.GetComponent<Rigidbody>();
        bombRb.isKinematic = false;
        IsActive = true;
    }

    public override bool ConfirmRune()
    {
        Vector3 velocity = PlayerInput.Instance.RightControllerAngularVelocity;
        if (PlayerInput.Instance.RuneConfirm)
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
        if (PlayerInput.Instance.RuneConfirm)
        {
            bomb.Explode();
            controller.DeactivateRune();
        }
    }

    public override void DeactivateRune()
    {
        if(bomb != null)
            Object.Destroy(bomb);
        bomb = null;
        bombRb = null;
        IsActive = false;
        IsRunning = false;
    }
}