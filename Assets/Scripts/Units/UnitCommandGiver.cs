using UnityEngine;
using UnityEngine.InputSystem;

public class UnitCommandGiver : MonoBehaviour
{
    [SerializeField] LayerMask layerMask = new();
    [SerializeField] UnitSelectionHandler unitSelectionHandler;
    private Camera mainCamera;


    void Start()
    {
        mainCamera = Camera.main;

    }
    private void Update()
    {
        if (!Mouse.current.rightButton.wasPressedThisFrame) { return; }

        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask)) { return; }

        if (hit.collider.TryGetComponent<Targetable>(out Targetable targetable))
        {
            if (targetable.isOwned)
            {
                TryMove(hit.point);
                return;
            }
            TryTarget(targetable);
            return;
        }

        TryMove(hit.point);
    }

    private void TryTarget(Targetable targetable)
    {
        foreach (Unit unit in unitSelectionHandler.SelectedUnits)
        {
            unit.Targeting.CmdSetTarget(targetable.gameObject);
        }
    }

    private void TryMove(Vector3 point)
    {
        foreach (Unit unit in unitSelectionHandler.SelectedUnits)
        {
            unit.UnitMovement.CmdMove(point);
        }
    }
}
