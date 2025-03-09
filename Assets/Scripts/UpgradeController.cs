using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class UpgradeController : MonoBehaviour
{
    public class Upgrades
    {
        public int UpPlayerSpeed { get; set; }
        public int UpBulletSpeed { get; set; }
        public int UpRange { get; set; }
        public int UpDamage { get; set; }
        public int UpRoF { get; set; }
        public int UpEnemySpeed { get; set; }
        public int UpPickUpRange { get; set; }
        public int UpMaxHealth { get; set; }
        public int UpHealthRegen { get; set; }
        public int UpArmour { get; set; }
        public int UpXpGain { get; set; }
        public int UpBulletPierce { get; set; }
        public int UpBulletCount { get; set; }
    }

    [SerializeField] private Canvas upgradeCanvas;
    [SerializeField] private TextMeshProUGUI up1txt;
    [SerializeField] private TextMeshProUGUI up2txt;
    [SerializeField] private TextMeshProUGUI up3txt;
    [SerializeField] private TextMeshProUGUI up1txt2;
    [SerializeField] private TextMeshProUGUI up2txt2;
    [SerializeField] private TextMeshProUGUI up3txt2;
    [SerializeField] private Color PlayerTxtColour;
    [SerializeField] private Color SpecialTxtColour;
    [SerializeField] private Color WeaponTxtColour;
    [SerializeField] private GameObject UpgradeDisplay;
    [SerializeField] private GameObject UpgradeDisplayHolder;
    private UpgradeNav upgradeNav;
    public Upgrades upgrades;
    private List<int> upgradeIndices = new List<int>();
    private int randomUpgrade;
    private int randomUpgrade2;
    private int randomUpgrade3;

    public delegate void UpgradeEventHandler(Upgrades upgrades);

    public static event UpgradeEventHandler OnUpgradeChosen;
    private List<GameObject> displays = new List<GameObject>();
    private bool firstDisplay;

    private void Start()
    {
        upgradeNav = GetComponent<UpgradeNav>();
        upgradeCanvas.enabled = false;
        upgrades = new Upgrades();
        var upgradeType = typeof(Upgrades);
        var properties = upgradeType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.PropertyType == typeof(int));
        foreach (var property in properties)
        {
            int index = (int)property.GetValue(upgrades, null);
            upgradeIndices.Add(index);
        }
    }

    public void UpdateChoice(int level)
    {
        Time.timeScale = 0;
        upgradeCanvas.enabled = true;
        if (level % 5 == 0)
        {
            randomUpgrade = 10;
            randomUpgrade2 = 11;
            randomUpgrade3 = 12;
        }
        else
        {
            randomUpgrade = Random.Range(0, upgradeIndices.Count - 3);
            while (true)
            {
                randomUpgrade2 = Random.Range(0, upgradeIndices.Count - 3);
                if (randomUpgrade2 != randomUpgrade)
                {
                    break;
                }
            }

            while (true)
            {
                randomUpgrade3 = Random.Range(0, upgradeIndices.Count - 3);
                if (randomUpgrade3 != randomUpgrade && randomUpgrade3 != randomUpgrade2)
                {
                    break;
                }
            }
        }

        string[] up1 = UpgradeName(randomUpgrade);
        string[] up2 = UpgradeName(randomUpgrade2);
        string[] up3 = UpgradeName(randomUpgrade3);
        up1txt.text = up1[0];
        up2txt.text = up2[0];
        up3txt.text = up3[0];
        up1txt2.text = up1[1];
        up2txt2.text = up2[1];
        up3txt2.text = up3[1];
        ColourTextByType(up1[2], up1txt, up1txt2);
        ColourTextByType(up2[2], up2txt, up2txt2);
        ColourTextByType(up3[2], up3txt, up3txt2);
        DisplayUpgrades();
        firstDisplay = true;
    }

    private void ColourTextByType(string code, TextMeshProUGUI text1, TextMeshProUGUI text2)
    {
        if (code.Equals("P"))
        {
            text1.color = PlayerTxtColour;
            text2.color = PlayerTxtColour;
        }
        else if (code.Equals("G"))
        {
            text1.color = WeaponTxtColour;
            text2.color = WeaponTxtColour;
        }
        else
        {
            text1.color = SpecialTxtColour;
            text2.color = SpecialTxtColour;
        }
    }

    private string[] UpgradeName(int up)
    {
        switch (up)
        {
            case 0:
                return new[] { "Move Speed", "+10%", "P" };
            case 1:
                return new[] { "Bullet Speed", "+15%", "G" };
            case 2:
                return new[] { "Range", "+15%", "G" };
            case 3:
                return new[] { "Damage", "+10%", "G" };
            case 4:
                return new[] { "Rate of Fire", "+10%", "G" };
            case 5:
                return new[] { "Enemy Speed", "-15%", "P" };
            case 6:
                return new[] { "Pickup Range", "+50%", "P" };
            case 7:
                return new[] { "Max Health", "+25%", "P" };
            case 8:
                return new[] { "Health Regen", "+50%", "P" };
            case 9:
                return new[] { "Armour", "+10%", "P" };
            case 10:
                return new[] { "XP Gain", "+25%", "S" };
            case 11:
                return new[] { "Bullet Pierce", "+1", "S" };
            case 12:
                return new[] { "Bullet Count", "+1", "S" };
        }

        return new[] { "", "" };
    }

    private void DisplayUpgrades()
    {
        var upgradeType = typeof(Upgrades);
        var properties = upgradeType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.PropertyType == typeof(int));
        int i = 0;
        int playerCount = 0;
        int gunCount = 0;
        int specialCount = 0;
        foreach (var property in properties)
        {
            string[] upName = UpgradeName(i);
            bool percent = false;
            char[] up1 = upName[1].ToCharArray();
            int num;
            if (up1.Length == 4)
            {
                string numString = up1[1].ToString();
                numString += up1[2].ToString();
                num = Int32.Parse(numString);
                percent = true;
            }
            else
            {
                num = Int32.Parse(up1[1].ToString());
            }

            int currentValue = (int)property.GetValue(upgrades);
            currentValue *= num;
            string value = up1[0].ToString();
            value += currentValue.ToString();
            if (percent)
            {
                value += "%";
            }

            if (!firstDisplay)
            {
                displays.Add(Instantiate(UpgradeDisplay, new Vector3(0, 0, 0), Quaternion.identity,
                    UpgradeDisplayHolder.transform));
                displays[i].SetActive(true);
            }

            if (upName[2].Equals("P"))
            {
                if (playerCount < 3)
                {
                    displays[i].transform.localPosition = new Vector3(-1685 + (playerCount * 470), -550, 0);
                }
                else
                {
                    displays[i].transform.localPosition = new Vector3(-1685 + ((playerCount - 3) * 470), -800, 0);
                }

                playerCount++;
                foreach (TextMeshProUGUI txt in displays[i].GetComponentsInChildren<TextMeshProUGUI>())
                {
                    txt.color = PlayerTxtColour;
                }
            }
            else if (upName[2].Equals("G"))
            {
                if (gunCount < 2)
                {
                    displays[i].transform.localPosition = new Vector3(930 + (gunCount * 570), -550, 0);
                }
                else
                {
                    displays[i].transform.localPosition = new Vector3(930 + ((gunCount - 2) * 570), -800, 0);
                }

                gunCount++;
                foreach (TextMeshProUGUI txt in displays[i].GetComponentsInChildren<TextMeshProUGUI>())
                {
                    txt.color = WeaponTxtColour;
                }
            }
            else
            {
                displays[i].transform.localPosition = new Vector3(0, -525 + (specialCount * -190), 0);
                specialCount++;
                foreach (TextMeshProUGUI txt in displays[i].GetComponentsInChildren<TextMeshProUGUI>())
                {
                    txt.color = SpecialTxtColour;
                }
            }

            foreach (TextMeshProUGUI txt in displays[i].GetComponentsInChildren<TextMeshProUGUI>())
            {
                if (txt.gameObject.CompareTag("UpName"))
                {
                    txt.text = upName[0];
                }
                else
                {
                    txt.text = value;
                }
            }

            i++;
        }
    }

    private void UpdateChoose(int chosen)
    {
        var upgradeType = typeof(Upgrades);
        var properties = upgradeType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.PropertyType == typeof(int));
        int count = 0;
        foreach (var property in properties)
        {
            if (count == chosen)
            {
                int currentValue = (int)property.GetValue(upgrades);
                property.SetValue(upgrades, currentValue + 1);
                OnUpgradeChosen?.Invoke(upgrades);
            }

            count++;
        }

        upgradeNav.isMenuNav = false;
        Time.timeScale = 1;
        upgradeCanvas.enabled = false;
    }

    public void On1Press()
    {
        UpdateChoose(randomUpgrade);
    }

    public void On2Press()
    {
        UpdateChoose(randomUpgrade2);
    }

    public void On3Press()
    {
        UpdateChoose(randomUpgrade3);
    }
}