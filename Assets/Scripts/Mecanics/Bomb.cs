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
   
    private MeshRenderer mesh;
    private Collider col;
    private Rigidbody rb;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
            Explode();
    }

    private void Awake()
    {
        mesh = GetComponent<MeshRenderer>();
        col = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();

    }

    public void Explode()
    {
        GameObject effect= Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        
        mesh.enabled = false;
        col.isTrigger = true;
        rb.isKinematic = true;
        
        
        Collider[] objects = Physics.OverlapSphere(transform.position, radius,interactables.value);

        foreach (var item in objects)
        {
            if (item.attachedRigidbody != null)
                item.attachedRigidbody.AddExplosionForce(explosionForce, transform.position, radius);
        }
        Destroy(effect, destroyDelay);
        Destroy(gameObject,destroyDelay);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color= Color.red;
        Gizmos.DrawSphere(transform.position,radius);
    }
}