using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathController : MonoBehaviour
{
    [SerializeField] private Canvas DeathCanvas;
    [SerializeField] private Canvas GameCanvas;
    [SerializeField] private Canvas UpgradeCanvas;
    [SerializeField] private TextMeshProUGUI scoreTxt;
    private GameController gameController;
    private bool isDead;

    private void Start()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        DeathCanvas.enabled = false;
    }

    public void DeathShow()
    {
        isDead = true;
        Time.timeScale = 0f;
        UpgradeCanvas.enabled = false;
        GameCanvas.gameObject.SetActive(false);
        DeathCanvas.enabled = true;
        scoreTxt.text = gameController.GetScore().ToString();
    }

    public void NewGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public bool IsDead()
    {
        return isDead;
    }
}