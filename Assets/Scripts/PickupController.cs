using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupController : MonoBehaviour
{
    private PlayerHealth playerHealth;

    private void Start()
    {
        playerHealth = GetComponentInParent<PlayerHealth>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("XP"))
        {
            other.GetComponent<XPController>().SetAttract();
        }
        if (other.gameObject.CompareTag("Bomb"))
        {
            other.GetComponent<XPController>().SetAttract();
        }
        if (other.gameObject.CompareTag("Heart") && !playerHealth.HealthFull())
        {
            other.GetComponent<XPController>().SetAttract();
        }
    }
}