using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StartController : MonoBehaviour
{
    [SerializeField] private Canvas startCanvas;
    [SerializeField] private Canvas gameCanvas;
    [SerializeField] private GameObject pistol;
    [SerializeField] private GameObject shotgun;
    [SerializeField] private GameObject sniper;
    [SerializeField] private GameObject smg;
    [SerializeField] private Image chooseTextImage;
    private bool gunChosen;
    private bool started;
    private bool flashing;

    void Start()
    {
        Time.timeScale = 0;
        startCanvas.gameObject.SetActive(true);
        gameCanvas.gameObject.SetActive(false);
    }

    public void StartGame()
    {
        if (gunChosen)
        {
            Time.timeScale = 1;
            startCanvas.gameObject.SetActive(false);
            gameCanvas.gameObject.SetActive(true);
            started = true;
        }
        else
        {
            if (!flashing)
            {
                   flashing = true;
                   StartCoroutine(FlashRed());
            }
        }
    }

    public void SelectPistol()
    {
        ChangeGun(1);
    }

    public void SelectShotgun()
    {
        ChangeGun(2);
    }

    public void SelectSniper()
    {
        ChangeGun(3);
    }

    public void SelectSmg()
    {
        ChangeGun(4);
    }

    private void ChangeGun(int gun)
    {
        gunChosen = true;
        pistol.SetActive(false);
        shotgun.SetActive(false);
        sniper.SetActive(false);
        smg.SetActive(false);
        switch (gun)
        {
            case 1:
                pistol.SetActive(true);
                break;
            case 2:
                shotgun.SetActive(true);
                break;
            case 3:
                sniper.SetActive(true);
                break;
            case 4:
                smg.SetActive(true);
                break;
        }
    }

    public bool Started()
    {
        return started;
    }

    private IEnumerator FlashRed()
    {
        Color originalColor = chooseTextImage.color;
        float timer = 0f;
        while (timer < 0.25f)
        {
            timer += Time.unscaledDeltaTime;
            float t = timer / 0.25f;
            chooseTextImage.color = Color.Lerp(originalColor, Color.red, t);
            yield return null;
        }
        timer = -0.1f;
        while (timer < 0.25f)
        {
            timer += Time.unscaledDeltaTime;
            float t = timer / 0.25f;
            chooseTextImage.color = Color.Lerp(Color.red, originalColor, t);
            yield return null;
        }
        chooseTextImage.color = originalColor;
        flashing = false;
    }
}