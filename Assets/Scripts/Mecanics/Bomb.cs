using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.VFX;

[RequireComponent(typeof(Collider),typeof(Rigidbody))]
public class Bomb : MonoBehaviour
{
    [SerializeField] private float explosionForce = 100.0f;
    [SerializeField] private float radius = 3.0f;
    [SerializeField] private float destroyDelay = 2.0f;
    [SerializeField] private LayerMask interactables;
    [SerializeField] private GameObject explosionEffectPrefab;
    
    private AudioSource audioSource;
    private Collider[] results = new Collider[100];

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Explode()
    {
        audioSource.Play();
        GameObject effect= Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);

        var size = Physics.OverlapSphereNonAlloc(transform.position, radius,results, interactables.value);
        if (size > 0)
        {
            for (int index = 0; index < size; index++)
            {
                Collider item = results[index];
                if (item.attachedRigidbody != null)
                    item.attachedRigidbody.AddExplosionForce(explosionForce, transform.position, radius);
            }
        }

        Destroy(effect, destroyDelay);
        Destroy(gameObject);
    }
    
}