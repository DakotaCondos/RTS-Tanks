using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class HealthDisplay : MonoBehaviour
{
    Health health;
    [SerializeField] bool displayHealthbarOnStart = false;
    [SerializeField] GameObject HealthbarPrefab;
    [SerializeField] Transform HealthbarTransform;
    private HealthbarUI healthbarUI;

    private void Awake()
    {
        health = GetComponent<Health>();
        health.ClientOnHealthChange += HandleHealthChanged;
        GameObject HealthbarInstance = Instantiate(HealthbarPrefab, HealthbarTransform);
        healthbarUI = HealthbarInstance.GetComponent<HealthbarUI>();
    }

    private void Start()
    {
        healthbarUI.gameObject.SetActive(displayHealthbarOnStart);
    }

    private void OnDestroy()
    {
        health.ClientOnHealthChange -= HandleHealthChanged;
    }

    private void HandleHealthChanged(double currentHealth, double maxHealth)
    {
        healthbarUI.gameObject.SetActive(true);
        healthbarUI.UpdateHealthBlock((float)(currentHealth / maxHealth));
    }

}
