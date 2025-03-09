using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscController : MonoBehaviour
{
    [SerializeField] private Canvas EscCanvas;
    [SerializeField] private Canvas gameCanvas;
    [SerializeField] private Canvas upgradeCanvas;
    private StartController startController;
    private DeathController deathController;
    private bool inUpdate;
    private bool inEsc;

    private void Start()
    {
        startController = GameObject.Find("StartController").GetComponent<StartController>();
        deathController = GameObject.Find("DeathController").GetComponent<DeathController>();
        EscCanvas.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && startController.Started() && !deathController.IsDead())
        {
            if (!inEsc)
            {
                inEsc = true;
                Time.timeScale = 0;
                EscCanvas.gameObject.SetActive(true);
                gameCanvas.gameObject.SetActive(false);
                inUpdate = upgradeCanvas.isActiveAndEnabled;
                upgradeCanvas.enabled = false;
            }
            else
            {
                Resume();
            }
        }
    }

    public void Resume()
    {
        inEsc = false;
        EscCanvas.gameObject.SetActive(false);
        if (inUpdate)
        {
            Time.timeScale = 0;
            gameCanvas.gameObject.SetActive(false);
            upgradeCanvas.enabled = true;
        }
        else
        {
            Time.timeScale = 1;
            gameCanvas.gameObject.SetActive(true);
        }

        inUpdate = false;
    }
}