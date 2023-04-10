using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RTSNetworkManager : NetworkManager
{
    [SerializeField] GameObject unitBasePrefab;
    [SerializeField] GameObject gameOverHandlerPrefab;


    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);
        RTSPlayer rtsPlayer = conn.identity.GetComponent<RTSPlayer>();
        rtsPlayer.SetTeamColor(new Color(
            UnityEngine.Random.Range(0f, 1f),
            UnityEngine.Random.Range(0f, 1f),
            UnityEngine.Random.Range(0f, 1f)
            ));

        GameObject unitSpawner = Instantiate(unitBasePrefab, conn.identity.transform.position, conn.identity.transform.rotation);
        NetworkServer.Spawn(unitSpawner, conn);

    }

    public override void OnServerSceneChanged(string sceneName)
    {
        //Change This Later
        Debug.LogWarning("Dont forget to change how this works");
        if (SceneManager.GetActiveScene().name.StartsWith("Map"))
        {
            GameObject gameOverHandlerInstance = Instantiate(gameOverHandlerPrefab);
            NetworkServer.Spawn(gameOverHandlerInstance);
        }
    }
}
