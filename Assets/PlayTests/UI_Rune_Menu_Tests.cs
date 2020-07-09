using System.Collections;
using NSubstitute;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using RangeAttribute = NUnit.Framework.RangeAttribute;

namespace Tests
{
    public class UI_Rune_Menu_Tests
    {
        public static UIRuneMenu GetRuneMenu()
        {
            var prefab = AssetDatabase.LoadAssetAtPath<UIRuneMenu>("Assets/Prefabs/UI/RuneCanvas.prefab");
            return Object.Instantiate(prefab);
        }

        [UnityTest]
        public IEnumerator occupied_amount_of_slots_is_the_same_as_runes_created()
        {
            
            var player = UniversalHelpers.GetPlayer();
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
        }

        [UnityTest]
        public IEnumerator cant_interact_with_menu_until_animation_is_done()
        {
            var player = UniversalHelpers.GetPlayer();
            var slotHolder = Object.FindObjectOfType<UIRuneSlotHolder>();
            
            Assert.AreEqual(false,slotHolder.IsActive);
            PlayerInput.Instance.LeftThumbStick.Returns(new Vector2(1.0f, 0.0f));
            Assert.AreEqual(false,slotHolder.IsActive);
            yield return new WaitForSeconds(0.6f);
            Assert.AreEqual(false,slotHolder.IsActive);
            yield return new WaitForSeconds(0.6f);
            Assert.AreEqual(true,slotHolder.IsActive);
            
            Object.DestroyImmediate(player.gameObject);
            
        }

        [UnityTest]
        public IEnumerator highlight_selected_slot([NUnit.Framework.Range(0, 4)] int slotNumber)
        {
            var player = UniversalHelpers.GetPlayer();
            var slotHolder = Object.FindObjectOfType<UIRuneSlotHolder>();
            
            PlayerInput.Instance.LeftThumbStick.Returns(new Vector2(1.0f, 0.0f));
            
            yield return new WaitForSeconds(1.1f);
            Vector3 vec = new Vector3(-0.1f,-1.0f,0.0f).normalized;
            vec = Quaternion.Euler(0.0f, slotNumber*20.0f, 0.0f)* vec;
            Debug.LogError(vec);
            
            PlayerInput.Instance.LeftThumbStick.Returns(new Vector2(vec.x,vec.y));
            yield return null;
            for (int index = 0; index < slotHolder.Slots.Length; index++)
            {
                var slot = slotHolder.Slots[index];
                Assert.AreEqual(slotNumber==index,slot.IsHighlited);
            }


            Object.DestroyImmediate(player.gameObject);
            yield return null;
        }
            
            
    }
}
