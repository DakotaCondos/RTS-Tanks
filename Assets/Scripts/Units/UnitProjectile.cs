using Mirror;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class UnitProjectile : NetworkBehaviour
{
    Rigidbody rb;
    [SerializeField] float timeToLive = 5f;
    [SerializeField] float launchForce = 25f;
    [SerializeField] double projectileDamage = 25;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Start()
    {
        rb.velocity = transform.forward * launchForce;
    }

    public override void OnStartServer()
    {
        Invoke(nameof(DestroySelf), timeToLive);
    }

    [Server]
    private void DestroySelf()
    {
        NetworkServer.Destroy(gameObject);
    }

    [ServerCallback]
    private void OnTriggerEnter(Collider other)
    {
        print($"Collided with {other.name}");
        other.TryGetComponent<Health>(out Health health);
        other.TryGetComponent<NetworkIdentity>(out NetworkIdentity networkIdentity);
        if (health == null || networkIdentity == null) { print("Returning Early"); return; }
        if (health.OwnerlessEntity || networkIdentity.connectionToClient != connectionToClient)
        {
            health.DealDamage(projectileDamage);
            DestroySelf();
        }
        print("Nothing Called");
    }
}
