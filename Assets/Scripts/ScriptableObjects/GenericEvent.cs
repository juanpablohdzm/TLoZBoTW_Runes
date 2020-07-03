using System;
using UnityEngine;


public class GenericEvent<T> : ScriptableObject
{
    protected event Action<T> OnEvent;

    public void AddListener(Action<T> listener) => OnEvent += listener;
    public void RemoveListener(Action<T> listener) => OnEvent -= listener;
    public void Invoke(T value) => OnEvent?.Invoke(value);
}