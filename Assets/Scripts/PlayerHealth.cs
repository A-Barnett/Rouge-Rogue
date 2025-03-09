using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    private float health;
    private float regenRate = 0.05f;
    private float regenTimer;
    private float regenAmount = 0.03f;
    [SerializeField] private Collider2D colliderMain;
    private float healthLost = 1f;
    [SerializeField] private Image healthBarFill;
    private float startMaxHealth;
    private float startRegenAmount;
    private float startHeathLost;
    private DeathController deathController;

    void Start()
    {
        health = maxHealth;
        startMaxHealth = maxHealth;
        startHeathLost = healthLost;
        startRegenAmount = regenAmount;
        deathController = GameObject.Find("DeathController").GetComponent<DeathController>();
    }

    void Update()
    {
        regenTimer += Time.deltaTime;
        if (regenTimer >= regenRate)
        {
            regenTimer = 0;
            if (health < maxHealth)
            {
                health += regenAmount;
            }
        }

        FillHeathBar();
        Death();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Enemy") && colliderMain.IsTouching(col.collider))
        {
            health -= healthLost;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && colliderMain.IsTouching(collision.collider))
        {
            health -= healthLost;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Heart") && colliderMain.IsTouching(col))
        {
            health += 30;
            if (health > maxHealth)
            {
                health = maxHealth;
            }

            Destroy(col.gameObject);
        }
    }

    private void Death()
    {
        if (health <= 0)
        {
            deathController.DeathShow();
            Destroy(gameObject);
        }
    }

    public bool HealthFull()
    {
        return health >= maxHealth;
    }

    private void FillHeathBar()
    {
        healthBarFill.fillAmount = health / maxHealth;
    }

    private void OnEnable()
    {
        UpgradeController.OnUpgradeChosen += HandleUpgradeChosen;
    }

    private void OnDisable()
    {
        UpgradeController.OnUpgradeChosen -= HandleUpgradeChosen;
    }

    private void HandleUpgradeChosen(UpgradeController.Upgrades upgrades)
    {
        float currMaxHealth = maxHealth;
        regenAmount = startRegenAmount * Mathf.Pow((1 + 0.5f), upgrades.UpHealthRegen);
        maxHealth = startMaxHealth * Mathf.Pow((1 + 0.25f), upgrades.UpMaxHealth);
        if (currMaxHealth != maxHealth)
        {
            health += startMaxHealth * 0.25f;
        }

        healthLost = startHeathLost * Mathf.Pow((1 / 1.1f), upgrades.UpArmour);
    }
}