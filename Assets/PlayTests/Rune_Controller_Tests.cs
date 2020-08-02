using System.Collections;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Utilities;

namespace Tests
{
    public class Rune_Controller_Tests
    {
        [UnityTest]
        public IEnumerator amount_of_runes_is_equal_to_amount_of_profiles()
        {
           var player =UniversalTestHelpers.GetPlayer();
           var runeController = player.GetComponent<RuneController>();
           yield return new WaitForSeconds(1.0f);
           
           Assert.AreEqual(runeController.AmountOfProfiles,runeController.AmountOfRunes);
           
           Object.DestroyImmediate(player.gameObject);
           yield return new WaitForUpdate();
        }

        [UnityTest]
        public IEnumerator current_rune_starts_null()
        {
            var player =UniversalTestHelpers.GetPlayer();
            var runeController = player.GetComponent<RuneController>();
            yield return new WaitForSeconds(1.0f);
            
            Assert.Null(runeController.CurrentRune);
            
            Object.DestroyImmediate(player.gameObject);
            yield return new WaitForUpdate();
        }
        
        [UnityTest]
        public IEnumerator selected_rune_starts_deactivated()
        {
            var player =UniversalTestHelpers.GetPlayer();
            var runeController = player.GetComponent<RuneController>();
            yield return new WaitForSeconds(1.0f);
            
            runeController.SelectRune(0);
            Assert.AreEqual(false,runeController.CurrentRune.IsActive);

            Object.DestroyImmediate(player.gameObject);
            yield return new WaitForUpdate();
        }
        
        [UnityTest]
        public IEnumerator selected_rune_activates()
        {
            var player =UniversalTestHelpers.GetPlayer();
            var runeController = player.GetComponent<RuneController>();
            yield return new WaitForSeconds(1.0f);
            
            runeController.SelectRune(0);
            PlayerInput.Instance.ToggleRuneActivation.Returns(true);
            yield return new WaitForFixedUpdate();
            Assert.AreEqual(true,runeController.CurrentRune.IsActive);

            Object.DestroyImmediate(player.gameObject);
            yield return new WaitForUpdate();
        }
        
        [UnityTest]
        public IEnumerator active_rune_is_not_running()
        {
            var player =UniversalTestHelpers.GetPlayer();
            var runeController = player.GetComponent<RuneController>();
            yield return new WaitForSeconds(1.0f);
            
            runeController.SelectRune(0);
            PlayerInput.Instance.ToggleRuneActivation.Returns(true);
            yield return null;
            Assert.AreEqual(false,runeController.CurrentRune.IsRunning);

            Object.DestroyImmediate(player.gameObject);
            yield return new WaitForUpdate();
        }

        [UnityTest]
        public IEnumerator cant_change_current_active_rune()
        {
            var player =UniversalTestHelpers.GetPlayer();
            var runeController = player.GetComponent<RuneController>();
            yield return new WaitForSeconds(1.0f);
            
            runeController.SelectRune(0);
            PlayerInput.Instance.ToggleRuneActivation.Returns(true);
            yield return null;
            var current = runeController.CurrentRune;
            PlayerInput.Instance.ToggleRuneActivation.Returns(false);
            runeController.SelectRune(1);
            Assert.AreEqual(current,runeController.CurrentRune);

            Object.DestroyImmediate(player.gameObject);
            yield return new WaitForUpdate();
        }
        
        [UnityTest]
        public IEnumerator give_an_index_in_bounds_makes_current_rune_not_null()
        {
            var player =UniversalTestHelpers.GetPlayer();
            var runeController = player.GetComponent<RuneController>();
            yield return new WaitForSeconds(1.0f);

            runeController.SelectRune(0);
            Assert.NotNull(runeController.CurrentRune);
            
            Object.DestroyImmediate(player.gameObject);
            yield return new WaitForUpdate();
        }

        [UnityTest]
        public IEnumerator give_an_index_out_of_bounds_makes_current_rune_null()
        {
            var player =UniversalTestHelpers.GetPlayer();
            var runeController = player.GetComponent<RuneController>();
            yield return new WaitForSeconds(1.0f);
            
            runeController.SelectRune(0);
            Assert.NotNull(runeController.CurrentRune);

            runeController.SelectRune(1000);
            Assert.Null(runeController.CurrentRune);
            
            Object.DestroyImmediate(player.gameObject);
            yield return new WaitForUpdate();
        }
        
        [UnityTest]
        public IEnumerator give_a_different_index_in_bounds_makes_current_rune_change()
        {
            var player =UniversalTestHelpers.GetPlayer();
            var runeController = player.GetComponent<RuneController>();
            yield return new WaitForSeconds(1.0f);
            
            runeController.SelectRune(0);
            Assert.NotNull(runeController.CurrentRune);
            Rune current = runeController.CurrentRune;

            runeController.SelectRune(1);
            Assert.AreNotEqual(current,runeController.CurrentRune);
            
            Object.DestroyImmediate(player.gameObject);
            yield return new WaitForUpdate();
        }
        

        
        

    }
}