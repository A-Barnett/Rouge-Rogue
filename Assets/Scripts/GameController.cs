using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public float xpNeed = 100;
    public float xpHas;
    [SerializeField] private Image xpBar;
    private int level;
    private int rampLevel;
    private UpgradeController upgradeController;
    private UpgradeNav upgradeNav;
    private EnemySpawner enemySpawner;
    private float rampUpInterval = 30;
    private float rampUpTimer;
    private float score;
    [SerializeField] private TextMeshProUGUI gameLevelTxt;

    private void Start()
    {
        upgradeController = GameObject.Find("UpgradeController").GetComponent<UpgradeController>();
        enemySpawner = GameObject.Find("Enemy Spawner").GetComponent<EnemySpawner>();
        upgradeNav = GameObject.Find("UpgradeController").GetComponent<UpgradeNav>();
    }

    private void Update()
    {
        rampUpTimer += Time.deltaTime;
        if (rampUpTimer >= rampUpInterval)
        {
            rampUpTimer = 0;
            rampLevel++;
            enemySpawner.IncreaseAmount(rampLevel);
        }

        if (xpHas >= xpNeed)
        {
            score += xpNeed;
            xpHas -= xpNeed;
            level++;
            xpNeed = Mathf.Pow(level, 2f) + (10*level)+ 100;
            gameLevelTxt.text = level.ToString();
            upgradeController.UpdateChoice(level);
            upgradeNav.isMenuNav = true;
            xpBar.fillAmount = 0f;
        }
    }

    public void GetXp(float amount)
    {
        xpHas += amount;
        xpBar.fillAmount = xpHas / xpNeed;
    }

    public float GetScore()
    {
        return score;
    }

    public bool IsUpgrade5()
    {
        return level % 5 == 0;
    }
}