using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField] private float rateOfFire;
    [SerializeField] private float rotSpeed;
    [SerializeField] private float moveSpeed;
    [SerializeField] private GameObject bulletPref;
    [SerializeField] private float bulletSpeed = 4;
    [SerializeField] private float bulletDamage = 25;
    [SerializeField] private float bulletRange = 1;
    [SerializeField] private int bulletPierce = 1;
    [SerializeField] private float bulletSize = 1;
    [SerializeField] private float bulletCount = 1;
    [SerializeField] private float distanceHeldAway = 0.5f;
    [SerializeField] private GameObject bulletSpawnPoint;
    [SerializeField] private GameObject bulletHolder;
    private float startBulletSpeed;
    private float startBulletDamage;
    private float startBulletRange;
    private int startBulletPierce;
    private float startBulletCount;
    private float startRateOfFire;
    private GameObject player;
    private Vector3 targetPosition;
    private Quaternion targetRotation;
    private Quaternion rotation = Quaternion.identity;
    private float shotTimer;

    void Start()
    {
        player = gameObject.transform.parent.gameObject;
        SetStartValues();
        rotation = Quaternion.Euler(0, 0, 270);
        targetPosition = player.transform.position + rotation * new Vector3(0, distanceHeldAway, 0);
        targetRotation = Quaternion.Euler(0, 0, rotation.eulerAngles.z + 90);
    }

    private void SetStartValues()
    {
        startBulletSpeed = bulletSpeed;
        startBulletDamage = bulletDamage;
        startBulletRange = bulletRange;
        startBulletPierce = bulletPierce;
        startRateOfFire = rateOfFire;
        startBulletCount = bulletCount;
    }

    void FixedUpdate()
    {
        RotateGun(Input.GetAxisRaw("Horizontal Fire"), Input.GetAxisRaw("Vertical Fire"));
        shotTimer += Time.deltaTime;
        if (shotTimer >= rateOfFire)
        {
            Shoot();
            shotTimer = 0;
        }
    }

    private void RotateGun(float horizontal, float vertical)
    {
        if (horizontal > 0 && vertical > 0)
        {
            rotation = Quaternion.Euler(0, 0, 315);
        }
        else if (horizontal < 0 && vertical > 0)
        {
            rotation = Quaternion.Euler(0, 0, 45);
        }
        else if (horizontal < 0 && vertical < 0)
        {
            rotation = Quaternion.Euler(0, 0, 135);
        }
        else if (horizontal > 0 && vertical < 0)
        {
            rotation = Quaternion.Euler(0, 0, 225);
        }
        else if (horizontal > 0)
        {
            rotation = Quaternion.Euler(0, 0, 270);
        }
        else if (horizontal < 0)
        {
            rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (vertical > 0)
        {
            rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (vertical < 0)
        {
            rotation = Quaternion.Euler(0, 0, 180);
        }

        targetPosition = player.transform.position + rotation * new Vector3(0, distanceHeldAway, 0);
        targetRotation = Quaternion.Euler(0, 0, rotation.eulerAngles.z + 90);
        transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotSpeed * Time.deltaTime);
    }

    private void Shoot()
    {
        bool even = false;
        for (int i = 1; i <= bulletCount; i++)
        {
            float angleAdd = 0;
            if (bulletCount % 2 == 0 && (i == 1 || i == 2))
            {
                if (i == 1)
                {
                    angleAdd = -5f;
                }
                else
                {
                    angleAdd = 5f;
                }

                even = true;
            }
            else if (i == 1)
            {
                angleAdd = 0;
            }
            else if (i <= (bulletCount + 2) / 2f)
            {
                if (even)
                {
                    angleAdd = 5 + (10 * (i - 2));
                }
                else
                {
                    angleAdd = 10 * (i - 1);
                }
            }
            else if (i > (bulletCount + 2) / 2f)
            {
                angleAdd = -5 - (10 * (i - 1 - (bulletCount / 2f)));
            }

            GameObject bullet = Instantiate(bulletPref, bulletSpawnPoint.transform.position,
                Quaternion.Euler(0, 0, bulletSpawnPoint.transform.eulerAngles.z - 90 + angleAdd));
            bullet.transform.SetParent(bulletHolder.transform);
            BulletController bulletController = bullet.GetComponent<BulletController>();
            bulletController.SetSpeed(bulletSpeed);
            bulletController.SetDamage(bulletDamage);
            bulletController.SetRange(bulletRange);
            bulletController.SetPierce(bulletPierce);
            bullet.transform.localScale *= bulletSize;
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
        bulletDamage = startBulletDamage * Mathf.Pow((1 + 0.1f), upgrades.UpDamage);
        bulletRange = startBulletRange * Mathf.Pow((1 + 0.15f), upgrades.UpRange);
        bulletSpeed = startBulletSpeed * Mathf.Pow((1 + 0.15f), upgrades.UpBulletSpeed);
        bulletPierce = startBulletPierce + upgrades.UpBulletPierce;
        rateOfFire = startRateOfFire * Mathf.Pow((1 / 1.1f), upgrades.UpRoF);
        bulletCount = startBulletCount + upgrades.UpBulletCount;
    }
}