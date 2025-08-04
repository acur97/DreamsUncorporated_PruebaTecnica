using System;
using UnityEngine;

[Serializable]
public struct LevelData
{
    public int currentLevel;
    public int currentLives;
    public int currentScore;
}

public class PersistanceData : MonoBehaviour
{
    public static PersistanceData instance;
    public static LevelData levelData;

    [SerializeField] private GameplaySettings settings;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
            instance = this;

            levelData = new LevelData
            {
                currentLevel = 0,
                currentLives = settings.lifes,
                currentScore = 0
            };
        }
    }

    public static void UpLevel()
    {
        levelData.currentLevel++;
        levelData.currentLives++;
    }

    public static float GetLevelMultiplier()
    {
        return 1 + (levelData.currentLevel * 0.75f);
    }
}