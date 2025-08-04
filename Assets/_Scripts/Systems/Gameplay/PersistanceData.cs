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

    /// <summary>
    /// Singleton, sets the initial LevelData values like the lifes
    /// </summary>
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

    /// <summary>
    /// Increases the level and the lifes for the next level
    /// </summary>
    public static void UpLevel()
    {
        levelData.currentLevel++;
        levelData.currentLives++;
    }

    /// <summary>
    /// A multiplier used to increase the speed or difficulty of the game
    /// </summary>
    /// <returns></returns>
    public static float GetLevelMultiplier()
    {
        return 1 + (levelData.currentLevel * 0.75f);
    }
}