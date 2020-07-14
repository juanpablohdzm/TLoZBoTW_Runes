public abstract class Rune
{
   protected Rune(RuneProfile profile)
   {
      this.Profile = profile;
   }

   public RuneProfile Profile { get; }
   public bool IsActive { get; protected set; } = false;
   public bool IsRunning { get; protected set; } = false;

   public abstract void ActivateRune();

   public abstract bool ConfirmRune();
   public abstract void UseRune();

   public abstract void DeactivateRune();

}