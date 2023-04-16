using Mirror;
using UnityEngine;

[RequireComponent(typeof(Targeting))]
public class UnitFiring : NetworkBehaviour
{
    Targeting targeting;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform projectileSpawnPoint;
    [SerializeField] float shootingRange = 5f;
    [SerializeField] float shotsPerSecond = .2f;
    [SerializeField] float rotationSpeed = 20f;

    private float lastShotTime;

    private void Awake()
    {
        targeting = GetComponent<Targeting>();
    }

    [ServerCallback]
    private void Update()
    {
        if (!targeting.HasTarget) { return; }
        ShootingLogic();
    }

    private void ShootingLogic()
    {
        if (!CanFireAtTarget()) { return; }
        Quaternion targetRotation = Quaternion.LookRotation(targeting.Target.transform.position - transform.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        if (Time.time > lastShotTime + (1 / shotsPerSecond))
        {
            Quaternion projectileRotation = Quaternion.LookRotation(targeting.Target.TargetPoint.transform.position - projectileSpawnPoint.position);
            GameObject projectileInstance = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileRotation);
            NetworkServer.Spawn(projectileInstance, connectionToClient);
            lastShotTime = Time.time;
        }
    }

    [Server]
    private bool CanFireAtTarget()
    {
        return (targeting.Target.transform.position - transform.position).sqrMagnitude <= shootingRange * shootingRange;
    }
}
