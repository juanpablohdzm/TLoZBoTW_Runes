using System.Collections;
using NSubstitute;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using Utilities;
using RangeAttribute = NUnit.Framework.RangeAttribute;

namespace Tests
{
    public class UI_Rune_Menu_Tests
    {


        [UnityTest]
        public IEnumerator occupied_amount_of_slots_is_the_same_as_runes_created()
        {
            
            var player = UniversalTestHelpers.GetPlayer();
            var slotHolder = Object.FindObjectOfType<UIRuneSlotHolder>();
            var runeController = Object.FindObjectOfType<RuneController>();
            
            yield return new WaitForEndOfFrame();

            var index = slotHolder.FindOpenSlot();
            if (index.HasValue)
            {
                Assert.AreEqual(runeController.AmountOfRunes,index.Value);
            }
            else
            {
                Assert.AreEqual(runeController.AmountOfRunes,slotHolder.AmountOfSlots);
            }
            
            Object.DestroyImmediate(player.gameObject);
            yield return null;
        }

        [UnityTest]
        public IEnumerator cant_interact_with_menu_until_animation_is_done()
        {
            var player = UniversalTestHelpers.GetPlayer();
            var slotHolder = Object.FindObjectOfType<UIRuneSlotHolder>();
            
            Assert.AreEqual(false,slotHolder.IsActive);
            PlayerInput.Instance.LeftThumbStick.Returns(new Vector2(1.0f, 0.0f));
            Assert.AreEqual(false,slotHolder.IsActive);
            yield return new WaitForSeconds(0.6f);
            Assert.AreEqual(false,slotHolder.IsActive);
            yield return new WaitForSeconds(0.6f);
            Assert.AreEqual(true,slotHolder.IsActive);
            
            Object.DestroyImmediate(player.gameObject);
            yield return null;
            
        }

        [UnityTest]
        public IEnumerator highlight_selected_slot([NUnit.Framework.Range(0, 4)] int slotNumber)
        {
            var player = UniversalTestHelpers.GetPlayer();
            var slotHolder = Object.FindObjectOfType<UIRuneSlotHolder>();
            
            PlayerInput.Instance.LeftThumbStick.Returns(new Vector2(1.0f, 0.0f));
            yield return new WaitForSeconds(1.1f);
            var angle = 269.0f - 72.0f * slotNumber;
            if (angle < 0)
                angle = 360 + angle;
            PlayerInput.Instance.GetJoyStickAngle(ControllerType.Left).Returns(angle);
            yield return new WaitForUpdate();
            
            for (int index = 0; index < slotHolder.Slots.Length; index++)
            {
                var slot = slotHolder.Slots[index];
                Assert.AreEqual(slotNumber==index,slot.IsHighlited);
            }
            
            Object.DestroyImmediate(player.gameObject);
            yield return null;
        }

        [UnityTest]
        public IEnumerator all_slots_are_unhighlight_on_enable()
        {
            var player = UniversalTestHelpers.GetPlayer();
            var slotHolder = Object.FindObjectOfType<UIRuneSlotHolder>();
            PlayerInput.Instance.LeftThumbStick.Returns(new Vector2(1.0f, 0.0f));
            for (int index = 0; index < slotHolder.Slots.Length; index++)
            {
                var slot = slotHolder.Slots[index];
                Assert.AreEqual(false,slot.IsHighlited);
            }
            yield return new WaitForUpdate();
            
            Object.DestroyImmediate(player.gameObject);
            yield return null;
            
        }
        
        [UnityTest]
        public IEnumerator all_slots_are_unhighlight_on_disable()
        {
            var player = UniversalTestHelpers.GetPlayer();
            var slotHolder = Object.FindObjectOfType<UIRuneSlotHolder>();
            PlayerInput.Instance.LeftThumbStick.Returns(new Vector2(1.0f, 0.0f));
            yield return new WaitForSeconds(1.1f);
            var angle = 269.0f;
            PlayerInput.Instance.GetJoyStickAngle(ControllerType.Left).Returns(angle);
            yield return new WaitForUpdate();
            
            Assert.AreEqual(true, slotHolder.Slots[0].IsHighlited);
            yield return new WaitForUpdate();
            
            PlayerInput.Instance.LeftThumbStick.Returns(new Vector2(0.0f, 0.0f));
            yield return new WaitForUpdate();
            
            foreach (var slot in slotHolder.Slots)
            {
                Assert.AreEqual(false,slot.IsHighlited);
            }
            
            Object.DestroyImmediate(player.gameObject);
            yield return null;
        }

        [UnityTest]
        public IEnumerator on_slot_selected_selectionSlot_changes()
        {
            var player = UniversalTestHelpers.GetPlayer();
            var selectionSlot = Object.FindObjectOfType<UIRuneSelectionSlot>();
            PlayerInput.Instance.LeftThumbStick.Returns(new Vector2(1.0f, 0.0f));
            
            yield return new WaitForSeconds(1.1f);
            PlayerInput.Instance.GetJoyStickAngle(ControllerType.Left).Returns(268.0f);
            
            yield return new WaitForUpdate();
            var icon = selectionSlot.Icon;
            Assert.AreEqual(null,icon);
            PlayerInput.Instance.UIRuneConfirm.Returns(true);
            
            yield return null;
            Assert.AreNotEqual(icon,selectionSlot.Icon);
            icon = selectionSlot.Icon;
            PlayerInput.Instance.GetJoyStickAngle(ControllerType.Left).Returns(90.0f);
            PlayerInput.Instance.UIRuneConfirm.Returns(true);

            yield return null;
            Assert.AreNotEqual(icon,selectionSlot.Icon);
            
            Object.DestroyImmediate(player.gameObject);
            yield return null;
        }
            
            
    }
}
