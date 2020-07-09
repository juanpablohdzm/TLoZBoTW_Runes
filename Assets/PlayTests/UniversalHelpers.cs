using NSubstitute;
using UnityEditor;
using UnityEngine;

namespace Tests
{
    public static class UniversalHelpers
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
    }
}