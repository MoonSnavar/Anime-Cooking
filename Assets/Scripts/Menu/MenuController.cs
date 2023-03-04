using InstantGamesBridge;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] private int maxScenes;
    [SerializeField] private TMP_Text moneyText;
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
        }

        moneyText.text = PlayerPrefs.GetInt("Money").ToString();

        for (int i = 1; i <= maxScenes; i++)
        {
            var lvl = Instantiate(levelPrefab, levelPanel.transform);
            lvl.GetComponent<Level>().levelID = i;
            lvl.GetComponent<Level>().OnClick += LoadLevel;
        }
    }

    public void LoadLevel(int levelIndex)
    {
        Bridge.advertisement.ShowInterstitial();
        SceneManager.LoadScene(levelIndex);        
    }
    private void OnDestroy()
    {
        for (int i = 0; i < levelPanel.transform.childCount; i++)
        {
            levelPanel.transform.GetChild(i).GetComponent<Level>().OnClick -= LoadLevel;
        }
    }
}
