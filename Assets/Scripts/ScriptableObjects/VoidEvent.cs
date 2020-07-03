using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/VoidEvent")]
public class VoidEvent : ScriptableObject
{
    private event Action OnEvent;

    public void AddListener(Action listener) => OnEvent += listener;
    public void RemoveListener(Action listener) => OnEvent -= listener;
    public void Invoke() => OnEvent?.Invoke();
}