using UnityEngine;
using Mirror;

public class Targetable : NetworkBehaviour
{
    [SerializeField] Transform target;
    public Transform TargetPoint { get => target; }
}
