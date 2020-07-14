using UnityEngine;

public class BombRune : Rune
{
    private readonly Player player;
    private readonly GameObject prefab;
    private readonly RuneController controller;

    private Bomb bomb;
    private Rigidbody bombRb;

    public BombRune(RuneProfile runeProfile, Player player, GameObject prefab, RuneController controller) : base(runeProfile)
    {
        this.player = player;
        this.prefab = prefab;
        this.controller = controller;
    }
    
    public override void ActivateRune()
    {
        bomb = GameObject.Instantiate(prefab).GetComponent<Bomb>();
        bomb.transform.parent = player.RightHand.transform;
        bomb.transform.localPosition = Vector3.zero;
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
        if(bomb != null)
            GameObject.Destroy(bomb.gameObject);
        bomb = null;
        bombRb = null;
        IsActive = false;
        IsRunning = false;
    }
}