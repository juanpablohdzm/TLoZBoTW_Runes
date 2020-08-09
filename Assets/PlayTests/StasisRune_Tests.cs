using System.Collections;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Utilities;

namespace Tests
{
    public class StasisRune_Tests
    {
        [UnityTest]
        public IEnumerator stasis_rune_gets_activated()
        {
            var player = UniversalTestHelpers.GetPlayer();
            var runeController = Object.FindObjectOfType<RuneController>();
            yield return new WaitForSeconds(1.0f);

            runeController.SelectRune(4);
            PlayerInput.Instance.ToggleRuneActivation.Returns(true);
            yield return new WaitForFixedUpdate();
            Assert.IsTrue(runeController.CurrentRune.IsActive);
            
            Object.DestroyImmediate(player.gameObject);
            yield return new WaitForUpdate();
        }
        
        [UnityTest]
        public IEnumerator stasis_rune_doesnt_run_with_normal_object_()
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
        public IEnumerator stasis_rune_run_with_interactable_object()
        {
            var player = UniversalTestHelpers.GetPlayer();
            var runeController = Object.FindObjectOfType<RuneController>();
            var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = player.transform.position + player.transform.forward;
            cube.AddComponent<IRuneInteractable>();
            cube.layer = 8;
            cube.GetComponent<Rigidbody>().useGravity = false;
            yield return new WaitForSeconds(1.0f);

            runeController.SelectRune(4);
            PlayerInput.Instance.ToggleRuneActivation.Returns(true);
            yield return new WaitForFixedUpdate();
            Assert.IsTrue(runeController.CurrentRune.IsActive);
            PlayerInput.Instance.ToggleRuneActivation.Returns(false);
            Assert.IsFalse(runeController.CurrentRune.IsRunning);
            PlayerInput.Instance.Confirm.Returns(true);
            yield return new WaitForFixedUpdate();
            Assert.IsTrue(runeController.CurrentRune.IsRunning);
            
            Object.DestroyImmediate(player.gameObject);
            Object.DestroyImmediate(cube);
            yield return new WaitForUpdate();

        }

        [UnityTest]
        public IEnumerator interactable_object_is_not_kinematic_before_confirm()
        {
            var player = UniversalTestHelpers.GetPlayer();
            var runeController = Object.FindObjectOfType<RuneController>();
            var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = player.transform.position + player.transform.forward;
            cube.AddComponent<IRuneInteractable>();
            cube.layer = 8;
            cube.GetComponent<Rigidbody>().useGravity = false;
            yield return new WaitForSeconds(1.0f);

            runeController.SelectRune(4);
            PlayerInput.Instance.ToggleRuneActivation.Returns(true);
            yield return new WaitForFixedUpdate();
            Assert.IsTrue(runeController.CurrentRune.IsActive);
            PlayerInput.Instance.ToggleRuneActivation.Returns(false);
            Assert.IsFalse(cube.GetComponent<Rigidbody>().isKinematic);
            
            Object.DestroyImmediate(player.gameObject);
            Object.DestroyImmediate(cube);
            yield return new WaitForUpdate();
        }

        [UnityTest]
        public IEnumerator interactable_object_is_kinematic_after_confirm()
        {
            var player = UniversalTestHelpers.GetPlayer();
            var runeController = Object.FindObjectOfType<RuneController>();
            var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = player.transform.position + player.transform.forward;
            cube.AddComponent<IRuneInteractable>();
            cube.layer = 8;
            cube.GetComponent<Rigidbody>().useGravity = false;
            yield return new WaitForSeconds(1.0f);

            runeController.SelectRune(4);
            PlayerInput.Instance.ToggleRuneActivation.Returns(true);
            yield return new WaitForFixedUpdate();
            Assert.IsTrue(runeController.CurrentRune.IsActive);
            PlayerInput.Instance.ToggleRuneActivation.Returns(false);
            Assert.IsFalse(cube.GetComponent<Rigidbody>().isKinematic);
            PlayerInput.Instance.Confirm.Returns(true);
            yield return new WaitForSeconds(0.5f);
            Assert.IsTrue(cube.GetComponent<Rigidbody>().isKinematic);
            
            
            Object.DestroyImmediate(player.gameObject);
            Object.DestroyImmediate(cube);
            yield return new WaitForUpdate();
        }

        [UnityTest]
        public IEnumerator interactable_toggles_kinematic_when_rune_is_activated()
        {
            var player = UniversalTestHelpers.GetPlayer();
            var runeController = Object.FindObjectOfType<RuneController>();
            var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = player.transform.position + player.transform.forward;
            cube.AddComponent<IRuneInteractable>();
            cube.layer = 8;
            cube.GetComponent<Rigidbody>().useGravity = false;
            yield return new WaitForSeconds(1.0f);

            runeController.SelectRune(4);
            (runeController.CurrentRune as StasisRune).DelayTime = 1.0f;
            PlayerInput.Instance.ToggleRuneActivation.Returns(true);
            yield return new WaitForFixedUpdate();
            Assert.IsTrue(runeController.CurrentRune.IsActive);
            PlayerInput.Instance.ToggleRuneActivation.Returns(false);
            Assert.IsFalse(cube.GetComponent<Rigidbody>().isKinematic);
            PlayerInput.Instance.Confirm.Returns(true);
            yield return new WaitForSeconds(0.5f);
            Assert.IsTrue(cube.GetComponent<Rigidbody>().isKinematic);
            yield return new WaitForSeconds(0.6f);
            Assert.IsFalse(cube.GetComponent<Rigidbody>().isKinematic);
            Assert.IsFalse(runeController.CurrentRune.IsActive);
            
            
            Object.DestroyImmediate(player.gameObject);
            Object.DestroyImmediate(cube);
            yield return new WaitForUpdate();
        }
    }
}