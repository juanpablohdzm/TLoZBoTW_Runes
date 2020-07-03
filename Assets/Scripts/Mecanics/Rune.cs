public abstract class Rune
{
   protected RuneProfile profile;

   public RuneProfile Profile => profile;
   public bool IsActive { get; protected set; }

   public abstract void ActivateRune();
   public abstract void UseRune();
   public abstract void DeactivateRune();
   
}