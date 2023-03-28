using Mirror;
using UnityEngine;

public class RTSNetworkManager : NetworkManager
{
    public GameObject unitSpawnerPrefab;
    
    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);

        GameObject unitSpawner =  Instantiate(unitSpawnerPrefab, conn.identity.transform.position, conn.identity.transform.rotation);
        NetworkServer.Spawn(unitSpawner, conn);
    }
}
