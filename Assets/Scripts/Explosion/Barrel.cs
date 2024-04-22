using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    [SerializeField] private float _explosionForce;
    [SerializeField] private float _explosionRadius;
    [SerializeField] private ParticleSystem _effect;


    private void OnCollisionEnter(Collision collider) 
    {
        Rigidbody barrelRb = collider?.rigidbody;

        if (barrelRb?.GetComponent<Granade>()) 
        {
            TriggerExplosion();
        }
    }

    private void OnMouseUpAsButton()
    {
        TriggerExplosion();
    }

    private void TriggerExplosion()
    {
        Explode();
        GetBarrelsInRange();
        Instantiate(_effect, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    private void Explode()
    { 
        foreach (Rigidbody barrel in GetExplodableObjects()) 
        {
            barrel.AddExplosionForce(_explosionForce, transform.position, _explosionRadius);
        }
    }

    private Collider[] GetBarrelsInRange()
    {
        Collider[] barrelsInRange = Physics.OverlapSphere(transform.position, Mathf.Lerp(0, _explosionRadius, 0.0f));
        Debug.Log($"barrels {barrelsInRange}");
        return barrelsInRange;
    }
    private List<Rigidbody> GetExplodableObjects() 
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _explosionRadius);
        List<Rigidbody> barrels = new();
        
        foreach (Collider hit in hits)
        {
            if (hit.attachedRigidbody != null && !hit.GetComponent<Barrel>())
            { 
                barrels.Add(hit.attachedRigidbody);
            }
        }

        return barrels;
    }
}
