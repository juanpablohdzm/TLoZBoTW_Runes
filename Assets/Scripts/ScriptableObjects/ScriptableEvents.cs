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

public class GenericEvent<T> : ScriptableObject
{
    protected event Action<T> OnEvent;

    public void AddListener(Action<T> listener) => OnEvent += listener;
    public void RemoveListener(Action<T> listener) => OnEvent -= listener;
    public void Invoke(T value) => OnEvent?.Invoke(value);
}

[CreateAssetMenu(menuName = "Events/RuneEvent")]
public class RuneEvent : GenericEvent<Rune>{}
