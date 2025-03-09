using System;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float range;
    private Rigidbody2D rb;
    private float rangeTimer;
    private int pierceCount = 1;
    [SerializeField] private float damage = 25;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rangeTimer += Time.deltaTime;
        if (rangeTimer > range)
        {
            Destroy(gameObject);
        }

        rb.velocity = transform.up * bulletSpeed;
    }

    public void Pierce()
    {
        pierceCount--;
        if (pierceCount <= 0)
        {
            Destroy(gameObject);
        }
    }

    public float GetDamage()
    {
        return damage;
    }

    public void SetDamage(float setDamage)
    {
        damage = setDamage;
    }

    public void SetPierce(int pierce)
    {
        pierceCount = pierce;
    }

    public void SetSpeed(float speed)
    {
        bulletSpeed = speed;
    }

    public void SetRange(float rangeIn)
    {
        range = rangeIn;
    }
}