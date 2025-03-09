using UnityEngine;
using UnityEngine.UI;

public class UpgradeNav : MonoBehaviour
{
    [SerializeField] private Button[] upgradeButtons;
    private int selectedUpgrade;
    [SerializeField] private Color defaultColor;
    [SerializeField] private Color selectedColor;
    public bool isMenuNav;

    void Update()
    {
        if (isMenuNav)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                upgradeButtons[selectedUpgrade].GetComponent<Image>().color = defaultColor;
                if (selectedUpgrade == 0)
                {
                    selectedUpgrade = upgradeButtons.Length;
                }

                selectedUpgrade -= 1;
                upgradeButtons[selectedUpgrade].GetComponent<Image>().color = selectedColor;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                upgradeButtons[selectedUpgrade].GetComponent<Image>().color = defaultColor;
                if (selectedUpgrade == upgradeButtons.Length - 1)
                {
                    selectedUpgrade = -1;
                }

                selectedUpgrade += 1;
                upgradeButtons[selectedUpgrade].GetComponent<Image>().color = selectedColor;
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                upgradeButtons[selectedUpgrade].onClick.Invoke();
            }
        }
    }
}