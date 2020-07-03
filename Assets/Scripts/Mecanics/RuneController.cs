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
   private bool runeIsActive = false;

   private void Start()
   {
      CreateRunes();

      SelectRune(0);
   }

   private void CreateRunes()
   {
      var player = GetComponent<Player>();
      for (int index = 0; index < profiles.Count; index++)
      {
         RuneProfile profile = profiles[index];
         switch (profile.RuneType)
         {
            case RuneType.Magnesis:
               runes.Add(new Magnesis(profile, interactableLayer.value, player));
               break;
            default:
               throw new ArgumentOutOfRangeException();
         }

         OnCreatedRune.Invoke(runes[index]);
      }
   }

   public bool SelectRune(int index)
   {
      Rune rune = runes[index];
      
      if (rune == currentRune) return false;
      if (currentRune != null && currentRune.IsActive) return false;
      
      currentRune = rune;
      return true;
   }

   private void ActivateRune()
   {
      if (currentRune == null) return;
      
      currentRune.ActivateRune();
      OnRuneSelected.Invoke(currentRune);
   }

   private void DeactivateRune()
   {
      if (currentRune == null) return;
      
      currentRune.DeactivateRune();
      OnRuneDeactivated.Invoke(currentRune);
      currentRune = null;
   }

   private void ToggleRuneActivation()
   {
      if(runeIsActive)
         DeactivateRune();
      else
         ActivateRune();

      runeIsActive = !runeIsActive;
   }

   private void FixedUpdate()
   {
      if(PlayerInput.Instance.ToggleRuneActivation) 
         ToggleRuneActivation();
      
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