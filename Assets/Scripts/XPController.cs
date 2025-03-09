using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XPController : MonoBehaviour
{
    [SerializeField] private float xpAmount = 10;
    private bool attract;
    private GameObject player;
    private Rigidbody2D rb;

    public void SetAttract()
    {
        attract = true;
    }

    public float GetXPAmount()
    {
        return xpAmount;
    }

    public void SetXPAmount(float setXp)
    {
        xpAmount = setXp;
    }

    private void Start()
    {
        player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (attract)
        {
            Vector3 target = (transform.position - player.transform.position);
            rb.velocity = -target.normalized -target * (Time.deltaTime * 200);
        }
        
    }
}
