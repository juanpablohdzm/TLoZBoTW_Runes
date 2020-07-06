using UnityEngine;

[RequireComponent(typeof(Collider),typeof(Rigidbody))]
public class Bomb : MonoBehaviour
{
    [SerializeField] private float explosionForce = 10.0f;
    [SerializeField] private float radius = 10.0f;
    public void Explode()
    {
        Collider[] objects = Physics.OverlapSphere(transform.position, radius);
        foreach (var item in objects)
        {
            item.attachedRigidbody.AddExplosionForce(explosionForce,transform.position,radius);
        }
        Destroy(gameObject);
    }
}