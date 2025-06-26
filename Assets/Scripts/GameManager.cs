using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int currentLevel = 0;

    void Awake()
    {
        // Ensure only one GameManager exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        currentLevel = PlayerPrefs.GetInt("CurrentLevel", 0);
    }

    public void LevelUp()
    {
        currentLevel++;
        Debug.Log("Leveled up to: " + currentLevel);
    }

    public void SaveProgress()
    {
        PlayerPrefs.SetInt("CurrentLevel", currentLevel);
        PlayerPrefs.Save();
        Debug.Log("Game saved at level: " + currentLevel);
    }

    void OnApplicationQuit()
    {
        SaveProgress(); 
    }
}
