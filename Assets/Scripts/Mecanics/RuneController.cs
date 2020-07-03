using System;
using System.Collections.Generic;
using UnityEngine;

public class RuneController : MonoBehaviour
{

   [SerializeField] private List<RuneProfile> profiles  = new List<RuneProfile>();
   [SerializeField] private LayerMask interactableLayer;
   [SerializeField] private RuneEvent OnCreatedRune;
   [SerializeField] private RuneEvent OnRuneSelected;
   [SerializeField] private RuneEvent OnRuneConfirmed;
   [SerializeField] private RuneEvent OnRuneDeactivated;
   
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
               runes.Add(new Magnesis(profile, interactableLayer.value));
               break;
            default:
               throw new ArgumentOutOfRangeException();
         }

         OnCreatedRune.Invoke(runes[index]);
      }
   }

   public void SelectRune(int index)
   {
      Rune rune = runes[index];
      
      if (rune == currentRune && currentRune.IsActive) return;
      
      if( currentRune.IsActive) DeactivateRune();

      currentRune = rune;
      currentRune.ActivateRune();

      OnRuneSelected.Invoke(currentRune);
   }

   public void DeactivateRune()
   {
      if (!currentRune.IsActive) return;
      
      currentRune.DeactivateRune();
      OnRuneDeactivated.Invoke(currentRune);
      currentRune = null;
   }

   private void FixedUpdate()
   {
      if (currentRune == null) return;
      if (!currentRune.IsActive)
      {
         if (currentRune.ConfirmRune())
         {
            OnRuneConfirmed.Invoke(currentRune);
         }
      }
      else
      {
         currentRune.UseRune();
      }
   }
}