using System;
using System.Collections.Generic;
using UnityEngine;

public class RuneController : MonoBehaviour
{

   [SerializeField] private List<RuneProfile> profiles  = new List<RuneProfile>();
   [SerializeField] private EventRune OnCreatedRune;
   [SerializeField] private EventRune OnRunActivated;
   [SerializeField] private EventRune OnRunDeactivated;
   
   private List<Rune> runes = new List<Rune>();
   private Rune currentRune;

   private void Start()
   {
      CreateRunes();
   }

   private void CreateRunes()
   {
      for (int index = 0; index < profiles.Count; index++)
      {
         RuneProfile profile = profiles[index];
         switch (profile.RuneType)
         {
            case RuneType.Magnesis:
               runes.Add(new Magnesis(profile));
               break;
            default:
               throw new ArgumentOutOfRangeException();
         }

         OnCreatedRune.Invoke(runes[index]);
      }
   }

   public void ActivateRune(int index)
   {
      Rune rune = runes[index];
      
      if (rune == currentRune && currentRune.IsActive) return;
      
      if( currentRune.IsActive) DeactivateRune();

      currentRune = rune;
      currentRune.ActivateRune();

      OnRunActivated.Invoke(currentRune);
   }

   public void DeactivateRune()
   {
      if (!currentRune.IsActive) return;
      
      currentRune.DeactivateRune();
      OnRunDeactivated.Invoke(currentRune);
      currentRune = null;
   }

}