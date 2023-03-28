using Nova;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthbarUI : MonoBehaviour
{
    public UIBlock2D currentHealthBlock;
    Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        transform.rotation = mainCamera.transform.rotation;
    }


    [ContextMenu("Test Health Block")]
    public void Test()
    {
        var v = Random.value;
        UpdateHealthBlock(v);
        print($"Setting healthbar to {v}%");
    }
    public void UpdateHealthBlock(float percentage)
    {
        percentage = Mathf.Clamp(percentage, 0, 1);
        currentHealthBlock.Size.X.Percent = percentage;
    }
}
