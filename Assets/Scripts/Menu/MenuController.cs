using InstantGamesBridge;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] private int maxScenes;
    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private TMP_Text possibilityText;
    [SerializeField] private GameObject textblock;
    [SerializeField] private GameObject levelPrefab;
    [SerializeField] private GameObject levelPanel;
    private void Start()
    {
        //PlayerPrefs.DeleteAll();
        if (!PlayerPrefs.HasKey("FirstLaunch"))
        {
            textblock.SetActive(true);
            PlayerPrefs.SetInt("FirstLaunch", 1);
            PlayerPrefs.SetInt("1LevelUnblock", 1);

        }

        PlayerPrefs.SetInt("Currentlevel", 0);

        UpdateText();

        for (int i = 1; i <= maxScenes; i++)
        {
            var lvl = Instantiate(levelPrefab, levelPanel.transform);
            lvl.GetComponent<Level>().levelID = i;
            lvl.GetComponent<Level>().OnClick += LoadLevel;
            lvl.GetComponent<Button>().interactable = PlayerPrefs.GetInt(i + "LevelUnblock") == 1;
        }
    }

    private void UpdateText()
    {
        moneyText.text = PlayerPrefs.GetInt("Money").ToString();
        possibilityText.text = "Всего возможностей - " + PlayerPrefs.GetInt("Possibility");
    }

    public void LoadLevel(int levelIndex)
    {
        Bridge.advertisement.ShowInterstitial();
        PlayerPrefs.SetInt("Currentlevel", levelIndex);
        SceneManager.LoadScene(1);        
    }
    private void OnDestroy()
    {
        for (int i = 0; i < levelPanel.transform.childCount; i++)
        {
            levelPanel.transform.GetChild(i).GetComponent<Level>().OnClick -= LoadLevel;
        }
    }
    public void BuyFullHambaga()
    {
        var money = PlayerPrefs.GetInt("Money");
        if (money >= 50)
        {
            PlayerPrefs.SetInt("Money", money - 50);

            PlayerPrefs.SetInt("Possibility", PlayerPrefs.GetInt("Possibility", 0) + 1);
            
            UpdateText();
        }
    }

    public void LoadFunMode()
    {
        Bridge.advertisement.ShowInterstitial();
        PlayerPrefs.SetInt("Currentlevel", -1);
        SceneManager.LoadScene(2);
    }
}
