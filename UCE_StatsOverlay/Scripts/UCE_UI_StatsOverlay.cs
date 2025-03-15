using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UCE_UI_StatsOverlay : MonoBehaviour
{
    private GameObject healthBar;
    private GameObject manaBar;
    private Entity entity;
    private float currentHealth;
    private float currentMana;

    // Grabs our starting components.
    private void Start()
    {
        healthBar = transform.GetChild(0).transform.GetChild(0).gameObject;
        manaBar = transform.GetChild(1).transform.GetChild(0).gameObject;
        entity = GetComponentInParent<Entity>();
        SetHealth(entity.health.current);
        SetMana(entity.mana.current);
    }

    // Update is called once per frame
    private void Update()
    {
        Player player = Player.localPlayer;
        if (player && entity.health.current != currentHealth) SetHealth(entity.health.current);

        if (player && entity.mana.current != currentMana) SetMana(entity.mana.current);
    }

    // LateUpdate so that all camera updates are finished.
    private void LateUpdate()
    {
        transform.forward = Camera.main.transform.forward;
    }

    // Sets up our visible overlay for health.
    private void SetHealth(float amount)
    {
        currentHealth = amount;

        float displayedHealth = currentHealth / entity.health.max;
        healthBar.transform.localScale = new Vector3(displayedHealth, 1, 1);
    }

    private void SetMana(float amount)
    {
        currentMana = amount;

        float displayedMana = currentMana / entity.mana.max;
        manaBar.transform.localScale = new Vector3(displayedMana, 1, 1);
    }
}