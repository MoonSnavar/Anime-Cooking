using InstantGamesBridge;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] private UITranslator uiTranslator;
    [SerializeField] private int maxScenes;
    [SerializeField] private TMP_Text buttonFMText;
    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private TMP_Text possibilityText;
    [SerializeField] private GameObject textblock;
    [SerializeField] private GameObject levelPrefab;
    [SerializeField] private GameObject levelPanel;
    [SerializeField] private GameObject levelsGO, shopGO;
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
        
        possibilityText.text = uiTranslator.GetTextByName("PossibilityCount") + PlayerPrefs.GetInt("Possibility");
        
        bool funMode = PlayerPrefs.GetInt("FunMode", 0) == 1;
        buttonFMText.text = funMode ? uiTranslator.GetTextByName("FMbuttonText") : uiTranslator.GetTextByName("FMbuttonTextLock");
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
        var money = PlayerPrefs.GetInt("Money");
        bool funMode = PlayerPrefs.GetInt("FunMode", 0) == 1;
        if (money >= 100 && !funMode)
        {
            PlayerPrefs.SetInt("Money", money - 100);
            PlayerPrefs.SetInt("FunMode", 1);
            UpdateText();
        }
        else if (funMode)
        {
            Bridge.advertisement.ShowInterstitial();
            PlayerPrefs.SetInt("Currentlevel", -1);
            SceneManager.LoadScene(2);
        }   
    }

    public void ShowAndHide()
    {
        levelsGO.SetActive(!levelsGO.activeSelf);
        shopGO.SetActive(!shopGO.activeSelf);
    }
}
