using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blueprint : MonoBehaviour
{
    [SerializeField] private Color onCollisionColor = Color.red;
    [SerializeField] private Color onNoCollisionColor = Color.blue;
    [SerializeField] List<Renderer> renderers = new List<Renderer>();
    [SerializeField] Material blueprintMaterial;
    private bool isColliding = false;
    public bool IsColliding { get => isColliding; }

    private void Start()
    {
        CollectRenderersRecursively(transform);
        foreach (Renderer renderer in renderers)
        {
            renderer.material = blueprintMaterial;

        }

        blueprintMaterial.color = onNoCollisionColor;
    }

    private void OnTriggerStay(Collider other)
    {
        blueprintMaterial.color = onCollisionColor;
        isColliding = true;
    }

    private void OnTriggerExit(Collider other)
    {
        blueprintMaterial.color = onNoCollisionColor;
        isColliding = false;
    }

    private void CollectRenderersRecursively(Transform parent)
    {
        foreach (Transform child in parent)
        {
            Renderer renderer = child.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderers.Add(renderer);
            }
            CollectRenderersRecursively(child);
        }
    }


}
