using UnityEngine;

public class LevelCharacteristics : MonoBehaviour
{
    public int levelDifficult;

    private void Awake()
    {
        levelDifficult = PlayerPrefs.GetInt("Currentlevel");
    }
}
