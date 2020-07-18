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
    [SerializeField] private AudioClip[] sfx;
    
    private AudioSource audioSource;
    private Collider[] results = new Collider[20];
    private Rigidbody rb;
    private Renderer[] rends;
    private Collider col;
    public bool IsExploding { get; private set; } = false;

    #region UnitTestingVariables
    #if UNITY_EDITOR
    public float DestroyDelay => destroyDelay;
    #endif
    #endregion

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        rends = GetComponentsInChildren<Renderer>();
        col = GetComponent<Collider>();
    }

    private void Start()
    {
        audioSource.clip = sfx[0];
        audioSource.Play();
    }
    
    [ContextMenu("Explode")]
    public void Explode()
    {
        IsExploding = true;
        
        audioSource.clip =sfx[1];
        audioSource.Play();

        DisableComponents();
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
        Destroy(gameObject,destroyDelay);
    }

    private void DisableComponents()
    {
        rb.isKinematic = true;
        col.isTrigger = true;
        foreach (var item in rends)
        {
            item.enabled = false;
        }
    }
    
}