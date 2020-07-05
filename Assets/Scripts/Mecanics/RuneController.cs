using System;
using System.Collections.Generic;
using UnityEngine;

public class RuneController : MonoBehaviour
{

   [SerializeField] private List<RuneProfile> profiles  = new List<RuneProfile>();
   
   [SerializeField] private LayerMask interactableLayer;
   [SerializeField] private GameObject laserPrefab;
   
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
               runes.Add(new Magnesis(profile,player, interactableLayer.value, 5.0f,laserPrefab));
               break;
            default:
               throw new ArgumentOutOfRangeException();
         }

         OnCreatedRune.Invoke(runes[index]);
      }
   }

   public bool SelectRune(int index)
   {
      if (index < 0 || index >= runes.Count)
      {
         currentRune = null;
         return false;
      }

      Rune rune = runes[index];
      
      if (rune == currentRune) return false;
      if (currentRune != null && currentRune.IsActive) return false;
      
      currentRune = rune;
      return true;
   }

   private void ActivateRune()
   {
      if (currentRune == null) return;

      runeIsActive = true;
      currentRune.ActivateRune();
      OnRuneSelected.Invoke(currentRune);
   }

   private void DeactivateRune()
   {
      if (currentRune == null) return;

      runeIsActive = false;
      currentRune.DeactivateRune();
      OnRuneDeactivated.Invoke(currentRune);
   }


   private void FixedUpdate()
   {
      if(PlayerInput.Instance.ToggleRuneActivation) 
         ToggleRuneActivation();
      
      if (currentRune == null) return;
      if (currentRune.IsActive)
      {
         if (!currentRune.IsRunning)
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
   private void ToggleRuneActivation()
   {
      if(runeIsActive)
         DeactivateRune();
      else
         ActivateRune();
   }
}