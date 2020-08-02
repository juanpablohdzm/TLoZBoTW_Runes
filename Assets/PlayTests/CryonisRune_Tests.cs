using System.Collections;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Utilities;

namespace Tests
{
    public class CryonisRune_Tests
    {
        [UnityTest]
        public IEnumerator cryonis_rune_creates_iceBlock_target_on_collision_with_correct_layer()
        {
            var player = UniversalTestHelpers.GetPlayer();
            var runeController = Object.FindObjectOfType<RuneController>();
            var plane = GameObject.CreatePrimitive(PrimitiveType.Quad);
            plane.layer = 10;
            plane.transform.position = player.RightHand.transform.position + player.RightHand.transform.forward * 2;
            yield return new WaitForSeconds(1.0f);
            
            runeController.SelectRune(3);
            PlayerInput.Instance.ToggleRuneActivation.Returns(true);
            yield return new WaitForFixedUpdate();
            PlayerInput.Instance.ToggleRuneActivation.Returns(false);
            var target = GameObject.FindObjectOfType<IceBlockTarget>();
            Assert.IsNotNull(target);
            Assert.IsTrue(target.gameObject.activeSelf);
            
            
            Object.DestroyImmediate(player.gameObject);
            Object.DestroyImmediate(plane);
            if(target != null)
                Object.DestroyImmediate(target.gameObject);
            yield return new WaitForUpdate();
        }
        
        [UnityTest]
        public IEnumerator cryonis_rune_turns_off_iceBlock_target_on_collision_with_incorrect_layer()
        {
            var player = UniversalTestHelpers.GetPlayer();
            var runeController = Object.FindObjectOfType<RuneController>();
            var plane = GameObject.CreatePrimitive(PrimitiveType.Quad);
            plane.layer = 10;
            plane.transform.position = player.RightHand.transform.position + player.RightHand.transform.forward * 2;
            yield return new WaitForSeconds(1.0f);
            
            runeController.SelectRune(3);
            PlayerInput.Instance.ToggleRuneActivation.Returns(true);
            yield return new WaitForFixedUpdate();
            PlayerInput.Instance.ToggleRuneActivation.Returns(false);
            var target = GameObject.FindObjectOfType<IceBlockTarget>();
            Assert.IsNotNull(target);
            Assert.IsTrue(target.gameObject.activeSelf);
            plane.layer = 8;
            yield return null;
            Assert.IsFalse(target.gameObject.activeSelf);
            
            
            Object.DestroyImmediate(player.gameObject);
            Object.DestroyImmediate(plane);
            yield return new WaitForUpdate();
        }
        
        [UnityTest]
        public IEnumerator cryonis_rune_creates_iceBlock_on_confirm_with_correct_layer()
        {
            var player = UniversalTestHelpers.GetPlayer();
            var runeController = Object.FindObjectOfType<RuneController>();
            var plane = GameObject.CreatePrimitive(PrimitiveType.Quad);
            plane.layer = 10;
            plane.transform.position = player.RightHand.transform.position + player.RightHand.transform.forward * 2;
            yield return new WaitForSeconds(1.0f);
            
            runeController.SelectRune(3);
            PlayerInput.Instance.ToggleRuneActivation.Returns(true);
            
            yield return new WaitForFixedUpdate();
            PlayerInput.Instance.ToggleRuneActivation.Returns(false);
            var target = GameObject.FindObjectOfType<IceBlockTarget>();
            Assert.IsNotNull(target);
            Assert.IsTrue(target.gameObject.activeSelf);

            PlayerInput.Instance.Confirm.Returns(true);
            yield return  new WaitForSeconds(1.0f);
            PlayerInput.Instance.Confirm.Returns(false);
            var iceblock = GameObject.FindObjectOfType<IceBlock>();
            Assert.IsNotNull(iceblock);

            
            Object.DestroyImmediate(player.gameObject);
            Object.DestroyImmediate(plane);
            Object.DestroyImmediate(iceblock);
            yield return new WaitForUpdate();
        }
        
        [UnityTest]
        public IEnumerator cryonis_rune_reuses_iceBlock_on_confirm_with_correct_layer_for_time_number_4()
        {
            var player = UniversalTestHelpers.GetPlayer();
            var runeController = Object.FindObjectOfType<RuneController>();
            var plane = GameObject.CreatePrimitive(PrimitiveType.Quad);
            plane.layer = 10;
            plane.transform.position = player.RightHand.transform.position + player.RightHand.transform.forward * 2;
            plane.transform.localScale = new Vector3(10.0f,10.0f,10.0f);
            yield return new WaitForSeconds(1.0f);
            
            runeController.SelectRune(3);
            PlayerInput.Instance.ToggleRuneActivation.Returns(true);
            
            yield return new WaitForFixedUpdate();
            PlayerInput.Instance.ToggleRuneActivation.Returns(false);
            var target = GameObject.FindObjectOfType<IceBlockTarget>();
            Assert.IsNotNull(target);
            Assert.IsTrue(target.gameObject.activeSelf);

            PlayerInput.Instance.Confirm.Returns(true);
            yield return  new WaitForSeconds(1.0f);
            PlayerInput.Instance.Confirm.Returns(false);
            var iceblock = GameObject.FindObjectOfType<IceBlock>(); //First ice block spwan
            Assert.IsNotNull(iceblock);
            
            
            PlayerInput.Instance.ToggleRuneActivation.Returns(true);
            yield return new WaitForFixedUpdate();
            PlayerInput.Instance.ToggleRuneActivation.Returns(false);
            PlayerInput.Instance.Confirm.Returns(true); //Second ice block spawn
            yield return  new WaitForSeconds(1.0f);
            PlayerInput.Instance.Confirm.Returns(false);
            
            PlayerInput.Instance.ToggleRuneActivation.Returns(true);
            yield return new WaitForFixedUpdate();
            PlayerInput.Instance.ToggleRuneActivation.Returns(false);
            PlayerInput.Instance.Confirm.Returns(true); //Third ice block spwan
            yield return  new WaitForSeconds(1.0f);
            PlayerInput.Instance.Confirm.Returns(false);
            
            PlayerInput.Instance.ToggleRuneActivation.Returns(true);
            yield return new WaitForFixedUpdate();
            PlayerInput.Instance.ToggleRuneActivation.Returns(false);
            PlayerInput.Instance.Confirm.Returns(true); //Fourth ice block spwan
            yield return  new WaitForSeconds(1.0f);
            PlayerInput.Instance.Confirm.Returns(false);
            var iceblocks = GameObject.FindObjectsOfType<IceBlock>();
            Assert.AreEqual(3,iceblocks.Length);

            Object.DestroyImmediate(player.gameObject);
            Object.DestroyImmediate(plane);
            for (int i = 0; i < iceblocks.Length; i++)
            {
                Object.DestroyImmediate(iceblocks[i]);
            }
            yield return new WaitForUpdate();
        }
    }
}