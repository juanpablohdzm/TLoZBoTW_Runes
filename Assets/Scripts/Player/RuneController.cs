using System;
using System.Collections.Generic;
using UnityEngine;

public class RuneController : MonoBehaviour
{

   [SerializeField] private RuneProfile[] profiles;
   
   [SerializeField] private LayerMask interactableLayer;
   
   [SerializeField] private GameObject laserPrefab;
   [SerializeField] private GameObject prefabSphereBomb;
   [SerializeField] private GameObject prefabBoxBomb;
   
   [SerializeField] private RuneEvent OnCreatedRune;
   [SerializeField] private RuneEvent OnRuneSelected;
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
            runes.Add(new MagnesisRune(profile, player, interactableLayer.value, 5.0f, laserPrefab));
         }
         else if (profile.RuneType == RuneType.RemoteBombSphere)
         {
            runes.Add(new BombRune(profile, player, prefabSphereBomb, this));
         }
         else if (profile.RuneType == RuneType.RemoteBombBox)
         {
            runes.Add(new BombRune(profile, player, prefabBoxBomb, this));
         }
         else
         {
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
      if (currentRune == null || currentRune.IsActive) return;

      runeIsActive = true;
      currentRune.ActivateRune();
      if(currentRune.Profile.ShouldNotify) OnRuneSelected.Invoke(currentRune);
   }

   public void DeactivateRune()
   {
      if (currentRune == null || !currentRune.IsActive) return;

      runeIsActive = false;
      currentRune.DeactivateRune();
      if(currentRune.Profile.ShouldNotify) OnRuneDeactivated.Invoke(currentRune);
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
            if(currentRune.Profile.ShouldNotify) OnRuneConfirmed.Invoke(currentRune);
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