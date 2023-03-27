using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class UnitSpawner : NetworkBehaviour, IPointerClickHandler
{
    [SerializeField] private List<GameObject> unitPrefabs = new();
    [SerializeField] private Transform unitSpawnLocation;



    #region Server
    [Command]
    private void CmdSpawnUnit()
    {
        //for now just grab a unit from the list
        GameObject newUnit = Instantiate(unitPrefabs.FirstOrDefault(), unitSpawnLocation.position, unitSpawnLocation.rotation);
        NetworkServer.Spawn(newUnit, connectionToClient);
    }
    #endregion

    #region Client
    public void OnPointerClick(PointerEventData eventData)
    {
        print("Clicked");
        if (eventData.button != PointerEventData.InputButton.Left) { return; }
        if (!isOwned) { return; }
        CmdSpawnUnit();
    }

    #endregion
}
