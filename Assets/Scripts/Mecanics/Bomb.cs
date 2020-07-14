using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;

[RequireComponent(typeof(Collider),typeof(Rigidbody))]
public class Bomb : MonoBehaviour
{
    [SerializeField] private float explosionForce = 100.0f;
    [SerializeField] private float radius = 3.0f;
    [SerializeField] private LayerMask interactables;

    public void Explode()
    {
        Collider[] objects = Physics.OverlapSphere(transform.position, radius,interactables.value);
        foreach (var item in objects)
        {
            if (item.attachedRigidbody != null)
                item.attachedRigidbody.AddExplosionForce(explosionForce, transform.position, radius);
        }
        Destroy(gameObject);
    }
    
}