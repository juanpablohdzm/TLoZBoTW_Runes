using System.Collections;
using NSubstitute;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using Utilities;

namespace Tests
{
    public class BombRune_Tests
    {
        [UnityTest]
        public IEnumerator bomb_rune_creates_a_bomb_on_activated()
        {
            var player = UniversalTestHelpers.GetPlayer();
            var runeController = Object.FindObjectOfType<RuneController>();
            yield return new WaitForSeconds(1.0f);
            
            runeController.SelectRune(1);
            PlayerInput.Instance.ToggleRuneActivation.Returns(true);
            yield return new WaitForFixedUpdate();
            var bomb = Object.FindObjectOfType<Bomb>();
            Assert.NotNull(bomb);

            Object.DestroyImmediate(player.gameObject);
            Object.DestroyImmediate(bomb);
            yield return new WaitForUpdate();
        }
        
        [UnityTest]
        public IEnumerator bomb_rune_destroy_bomb_on_deactivated()
        {
            var player = UniversalTestHelpers.GetPlayer();
            var runeController = Object.FindObjectOfType<RuneController>();
            yield return new WaitForSeconds(1.0f);
            
            runeController.SelectRune(1);
            PlayerInput.Instance.ToggleRuneActivation.Returns(true);
            yield return new WaitForFixedUpdate();
            var bomb = Object.FindObjectOfType<Bomb>();
            Assert.NotNull(bomb);
            
            yield return new WaitForFixedUpdate();
            PlayerInput.Instance.ToggleRuneActivation.Returns(false);
            yield return null;
            bomb = Object.FindObjectOfType<Bomb>();
            Assert.IsNull(bomb);
            

            Object.DestroyImmediate(player.gameObject);
            yield return new WaitForUpdate();
        }
        
        [UnityTest]
        public IEnumerator bomb_rune_detached_bomb_on_confirm()
        {
            var player = UniversalTestHelpers.GetPlayer();
            var runeController = Object.FindObjectOfType<RuneController>();
            yield return new WaitForSeconds(1.0f);
            
            runeController.SelectRune(1);
            PlayerInput.Instance.ToggleRuneActivation.Returns(true);
            yield return new WaitForFixedUpdate();
            
            PlayerInput.Instance.ToggleRuneActivation.Returns(false);
            var bomb = Object.FindObjectOfType<Bomb>();
            Assert.NotNull(bomb);
            PlayerInput.Instance.Confirm.Returns(true);
            yield return new WaitForFixedUpdate();
            
            Assert.IsNull(bomb.transform.parent);
            
            Object.DestroyImmediate(player.gameObject);
            Object.DestroyImmediate(bomb);
            yield return new WaitForUpdate();
        }
        
        [UnityTest]
        public IEnumerator bomb_rune_explode_on_confirm()
        {
            var player = UniversalTestHelpers.GetPlayer();
            var runeController = Object.FindObjectOfType<RuneController>();
            yield return new WaitForSeconds(1.0f);
            
            runeController.SelectRune(1);
            PlayerInput.Instance.ToggleRuneActivation.Returns(true);
            yield return new WaitForFixedUpdate();
            
            PlayerInput.Instance.ToggleRuneActivation.Returns(false);
            var bomb = Object.FindObjectOfType<Bomb>();
            Assert.NotNull(bomb);
            PlayerInput.Instance.Confirm.Returns(true);
            yield return new WaitForFixedUpdate();
            //IsRunning
            yield return new WaitForSeconds(bomb.DestroyDelay+0.1f);
            bomb = Object.FindObjectOfType<Bomb>();
            Assert.IsNull(bomb);

            Object.DestroyImmediate(player.gameObject);
            yield return new WaitForUpdate();
        }
        
        [UnityTest]
        public IEnumerator bomb_rune_on_explode_deactivates_currentRune()
        {
            var player = UniversalTestHelpers.GetPlayer();
            var runeController = Object.FindObjectOfType<RuneController>();
            yield return new WaitForSeconds(1.0f);
            
            runeController.SelectRune(1);
            PlayerInput.Instance.ToggleRuneActivation.Returns(true);
            yield return new WaitForFixedUpdate();
            
            PlayerInput.Instance.ToggleRuneActivation.Returns(false);
            var bomb = Object.FindObjectOfType<Bomb>();
            Assert.NotNull(bomb);
            PlayerInput.Instance.Confirm.Returns(true);
            yield return new WaitForFixedUpdate();
            //IsRunning
            yield return new WaitForSeconds(0.5f);
            Assert.IsFalse(runeController.CurrentRune.IsActive);

            Object.DestroyImmediate(player.gameObject);
            yield return new WaitForUpdate();
        }
    }
}