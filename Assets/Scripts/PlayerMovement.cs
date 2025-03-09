using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    private GameController gameController;
    private Rigidbody2D rb;
    private float startSpeed;
    [SerializeField] private Collider2D colliderMain;
    [SerializeField] private Collider2D colliderPickUp;
    private GameObject XpHolder;
    private float isAttracting;
    [SerializeField] private GameObject enemyHolder;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        startSpeed = speed;
        XpHolder = GameObject.Find("XpHolder");
    }

    void FixedUpdate()
    {
        rb.rotation = 0;
        rb.angularVelocity = 0;
        Movement(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        isAttracting = Mathf.Clamp(isAttracting-Time.deltaTime,0,10);
    }

    private void Movement(float horizontal, float vertical)
    {
        rb.velocity = new Vector2(horizontal, vertical).normalized * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("XP") && colliderMain.IsTouching(other))
        {
            gameController.GetXp(other.GetComponent<XPController>().GetXPAmount());
            Destroy(other.gameObject);
        }else if (other.gameObject.CompareTag("XpAttract") && colliderMain.IsTouching(other))
        {
            if (isAttracting == 0)
            {
                AttractXp();
                isAttracting = 5f;
            }
            Destroy(other.gameObject);
        }
        else if (other.gameObject.CompareTag("Bomb") && colliderMain.IsTouching(other))
        {
            foreach (EnemyController enemy in enemyHolder.GetComponentsInChildren<EnemyController>())
            {
                enemy.Bomb();
            }
            Destroy(other.gameObject);
        }
    }

    private void AttractXp()
    {
        foreach (XPController xp in XpHolder.GetComponentsInChildren<XPController>())
        {
            xp.SetAttract();
        }
        
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
        speed = startSpeed * Mathf.Pow((1 + 0.1f), upgrades.UpPlayerSpeed);
        colliderPickUp.transform.localScale = new Vector3(1, 1, 1) + new Vector3(upgrades.UpPickUpRange / 2f,
            upgrades.UpPickUpRange / 2f, upgrades.UpPickUpRange / 2f);
    }
}