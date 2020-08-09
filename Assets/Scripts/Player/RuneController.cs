using System;
using System.Collections.Generic;
using UnityEngine;

public class RuneController : MonoBehaviour
{

   [SerializeField] private RuneProfile[] profiles;
   
   [SerializeField] private LayerMask interactableLayer;
   [SerializeField] private LayerMask cryonisLayerMask;
   [SerializeField] private LayerMask iceblockLayerMask;

   [SerializeField] private RuneEvent OnCreatedRune;
   [SerializeField] private RuneEvent OnRuneSelected;
   [SerializeField] private RuneEvent OnRuneActivated;
   [SerializeField] private RuneEvent OnRuneConfirmed;
   [SerializeField] private RuneEvent OnRuneDeactivated;
   
   private List<Rune> runes = new List<Rune>();
   private Rune currentRune;
   private bool runeIsActive = false;
   
   #region UnitTestsVariables
   #if UNITY_EDITOR
   public int AmountOfRunes => runes.Count;
   public int AmountOfProfiles => profiles.Length;
   public Rune CurrentRune => currentRune;
   #endif
   #endregion
   
   private void Start()
   {
      CreateRunes();
   }

   private void CreateRunes()
   {
      var player = GetComponent<Player>();
      for (int index = 0; index < profiles.Length; index++)
      {
         RuneProfile profile = profiles[index];
         if (profile.RuneType == RuneType.Magnesis)
         {
            runes.Add(new MagnesisRune(profile, player, interactableLayer.value, 5.0f));
         }
         else if (profile.RuneType == RuneType.RemoteBombSphere)
         {
            runes.Add(new BombRune(profile, player, this));
         }
         else if (profile.RuneType == RuneType.RemoteBombBox)
         {
            runes.Add(new BombRune(profile, player, this));
         }
         else if (profile.RuneType == RuneType.Cryonis)
         {
            runes.Add(new CryonisRune(profile,player,cryonisLayerMask,iceblockLayerMask,this));
         }
         else if (profile.RuneType == RuneType.Stasis)
         {
            runes.Add(new StasisRune(profile,player,interactableLayer.value,this));
         }
         else
         {
            throw new ArgumentOutOfRangeException();
         }

         OnCreatedRune.Invoke(runes[index]);
      }
   }

   public void SelectRune(int index)
   {
      if (index < 0 || index >= runes.Count)
      {
         currentRune = null;
         return;
      }

      Rune rune = runes[index];
      
      if (rune == currentRune) return;
      if (currentRune != null && currentRune.IsActive) return;
      
      currentRune = rune;
      OnRuneSelected.Invoke(currentRune);
   }

   private void ActivateRune()
   {
      if (currentRune == null || currentRune.IsActive) return;

      currentRune.ActivateRune(); 
      runeIsActive = currentRune.IsActive;
      if(runeIsActive)
         OnRuneActivated.Invoke(currentRune);
   }

   public void DeactivateRune()
   {
      if (currentRune == null || !currentRune.IsActive) return;

      currentRune.DeactivateRune();
      runeIsActive = currentRune.IsActive;
      if(!runeIsActive)
         OnRuneDeactivated.Invoke(currentRune);
   }


   private void FixedUpdate()
   {
      if(PlayerInput.Instance.ToggleRuneActivation) 
         ToggleRuneActivation();
      
      if (currentRune == null || !currentRune.IsActive) return;

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
   private void ToggleRuneActivation()
   {
      if(runeIsActive)
         DeactivateRune();
      else
         ActivateRune();
   }
}