using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float maxHealth;
    [SerializeField] private GameObject xpDropPref;
    [SerializeField] private GameObject healthDropPref;
    [SerializeField] private GameObject xpAttractDropPref;
    [SerializeField] private GameObject bombPickupPref;
    [SerializeField] private Color damageFlashColour;
    private GameObject player;
    private float health;
    private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private float xpAmount;
    private bool beenHit;
    private GameObject xpHolder;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
        health = maxHealth;
        xpHolder = GameObject.Find("XpHolder");
    }

    public void SetHealth(int setHealth)
    {
        health = setHealth;
        maxHealth = setHealth;
    }

    public void SetXpAmount(float setXP)
    {
        xpAmount = setXP;
    }

    public void SetSpeed(float setSpeed)
    {
        speed = setSpeed;
    }

    private void FixedUpdate()
    {
        MoveToPlayer();
    }

    private void MoveToPlayer()
    {
        Vector3 targetPos = rb.transform.position - player.transform.position;
        float distance = targetPos.magnitude;
        if (distance > 100)
        {
            Destroy(gameObject);
        }
        rb.velocity = -targetPos.normalized * speed;
        if (transform.rotation != new Quaternion(0, 0, 0, 0))
        {
            transform.rotation = new Quaternion(0, 0, 0, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            health -= other.GetComponent<BulletController>().GetDamage();
            other.GetComponent<BulletController>().Pierce();
            Death();
            if (!beenHit)
            {
                StartCoroutine(DamageFlash());
            }
        }
    }

    private IEnumerator DamageFlash()
    {
        beenHit = true;
        Color originalColor = spriteRenderer.color;
        float timer = 0f;
        while (timer < 0.1f)
        {
            timer += Time.deltaTime;
            float t = timer / 0.1f;
            spriteRenderer.color = Color.Lerp(originalColor, damageFlashColour, t);
            yield return null;
        }
        
        timer = -0.1f;
        while (timer < 0.1f)
        {
            timer += Time.deltaTime;
            float t = timer / 0.1f;
            spriteRenderer.color = Color.Lerp(damageFlashColour, originalColor, t);
            yield return null;
        }

        spriteRenderer.color = originalColor;
        beenHit = false;
    }

    private void Death()
    {
        if (health <= 0)
        {
            Vector3 pos = transform.position;
            if (Random.Range(0, 200) == 0)
            {
                Instantiate(healthDropPref, pos, Quaternion.identity, xpHolder.transform);
            }
            if (Random.Range(0, 300) == 0)
            {
                Instantiate(xpAttractDropPref, pos, Quaternion.identity, xpHolder.transform);
            }
            if (Random.Range(0, 300) == 0)
            {
                Instantiate(bombPickupPref, pos, Quaternion.identity, xpHolder.transform);
            }

            pos.z += 0.1f;
            GameObject xp = Instantiate(xpDropPref, pos, Quaternion.identity, xpHolder.transform);
            xp.GetComponent<XPController>().SetXPAmount(xpAmount);
            Destroy(gameObject);
        }
    }
    public void Bomb()
    {
        health = 0;
        Death();
    }
}