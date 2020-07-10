using NSubstitute;
using UnityEditor;
using UnityEngine;

namespace Tests
{
    public static class UniversalTestHelpers
    {
        public static Player GetPlayer()
        {
            var prefab = AssetDatabase.LoadAssetAtPath<Player>("Assets/Prefabs/Player/Player Variant.prefab");
            CreatePlayerInput();
            return Object.Instantiate(prefab);
        }

        public static void CreatePlayerInput()
        {
            PlayerInput.Instance = Substitute.For<IPlayerInput>(); 
        }
        
        public static UIRuneMenu GetRuneMenu()
        {
            var prefab = AssetDatabase.LoadAssetAtPath<UIRuneMenu>("Assets/Prefabs/UI/RuneCanvas.prefab");
            return Object.Instantiate(prefab);
        }
    }
}