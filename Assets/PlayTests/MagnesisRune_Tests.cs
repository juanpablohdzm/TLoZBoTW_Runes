using System.Collections;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Utilities;

namespace Tests
{
    public class MagnesisRune_Tests
    {
        [UnityTest]
        public IEnumerator mangesis_rune_gets_activated()
        {
            var player = UniversalTestHelpers.GetPlayer();
            var runeController = Object.FindObjectOfType<RuneController>();
            yield return new WaitForSeconds(1.0f);

            runeController.SelectRune(0);
            PlayerInput.Instance.ToggleRuneActivation.Returns(true);
            yield return new WaitForFixedUpdate();
            Assert.IsTrue(runeController.CurrentRune.IsActive);
            
            Object.DestroyImmediate(player.gameObject);
            yield return new WaitForUpdate();
        }
        
        [UnityTest]
        public IEnumerator mangesis_rune_doesnt_run_with_normal_object_()
        {
            var player = UniversalTestHelpers.GetPlayer();
            var runeController = Object.FindObjectOfType<RuneController>();
            var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = player.transform.position + player.transform.forward;
            yield return new WaitForSeconds(1.0f);

            runeController.SelectRune(0);
            PlayerInput.Instance.ToggleRuneActivation.Returns(true);
            yield return new WaitForFixedUpdate();
            Assert.IsTrue(runeController.CurrentRune.IsActive);
            PlayerInput.Instance.ToggleRuneActivation.Returns(false);
            PlayerInput.Instance.Confirm.Returns(true);
            yield return new WaitForFixedUpdate();
            Assert.IsFalse(runeController.CurrentRune.IsRunning);
            
            Object.DestroyImmediate(player.gameObject);
            Object.DestroyImmediate(cube);
            yield return new WaitForUpdate();

        }
        
        [UnityTest]
        public IEnumerator mangesis_rune_run_with_interactable_object()
        {
            var player = UniversalTestHelpers.GetPlayer();
            var runeController = Object.FindObjectOfType<RuneController>();
            var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = player.transform.position + player.transform.forward;
            cube.AddComponent<IRuneInteractable>();
            cube.layer = 8;
            cube.GetComponent<Rigidbody>().useGravity = false;
            yield return new WaitForSeconds(1.0f);

            runeController.SelectRune(0);
            PlayerInput.Instance.ToggleRuneActivation.Returns(true);
            yield return new WaitForFixedUpdate();
            Assert.IsTrue(runeController.CurrentRune.IsActive);
            PlayerInput.Instance.ToggleRuneActivation.Returns(false);
            PlayerInput.Instance.Confirm.Returns(true);
            Assert.IsFalse(runeController.CurrentRune.IsRunning);
            yield return new WaitForSeconds(1.1f); // Wait for animation
            Assert.IsTrue(runeController.CurrentRune.IsRunning);
            
            Object.DestroyImmediate(player.gameObject);
            Object.DestroyImmediate(cube);
            yield return new WaitForUpdate();

        }
        
        [UnityTest]
        public IEnumerator mangesis_rune_interactable_object_is_moved_when_controller_moves()
        {
            var player = UniversalTestHelpers.GetPlayer();
            var runeController = Object.FindObjectOfType<RuneController>();
            var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = player.RightHand.transform.position + player.RightHand.transform.forward;
            cube.AddComponent<IRuneInteractable>();
            cube.layer = 8;
            cube.GetComponent<Rigidbody>().useGravity = false;
            yield return new WaitForSeconds(1.0f);

            runeController.SelectRune(0);
            PlayerInput.Instance.ToggleRuneActivation.Returns(true);
            yield return new WaitForFixedUpdate();
            Assert.IsTrue(runeController.CurrentRune.IsActive);
            PlayerInput.Instance.ToggleRuneActivation.Returns(false);
            PlayerInput.Instance.Confirm.Returns(true);
            Assert.IsFalse(runeController.CurrentRune.IsRunning);
            yield return new WaitForSeconds(1.1f); // Wait for animation
            Assert.IsTrue(runeController.CurrentRune.IsRunning);

            PlayerInput.Instance.RightControllerVelocity.Returns(new Vector3(1.0f, 1.0f, 0.0f));
            yield return new WaitForSeconds(0.5f);
            Assert.Greater(cube.transform.position.x,0.5f);
            Assert.Greater(cube.transform.position.y,0.5f);
            
            Object.DestroyImmediate(player.gameObject);
            Object.DestroyImmediate(cube);
            yield return new WaitForUpdate();

        }
        
        [UnityTest]
        public IEnumerator mangesis_rune_gets_deactivated()
        {
            var player = UniversalTestHelpers.GetPlayer();
            var runeController = Object.FindObjectOfType<RuneController>();
            yield return new WaitForSeconds(1.0f);

            runeController.SelectRune(0);
            PlayerInput.Instance.ToggleRuneActivation.Returns(true);
            yield return new WaitForFixedUpdate();
            Assert.IsTrue(runeController.CurrentRune.IsActive);
            yield return new WaitForFixedUpdate();
            Assert.IsFalse(runeController.CurrentRune.IsActive);
            
            Object.DestroyImmediate(player.gameObject);
            yield return new WaitForUpdate();
        }
    }
}