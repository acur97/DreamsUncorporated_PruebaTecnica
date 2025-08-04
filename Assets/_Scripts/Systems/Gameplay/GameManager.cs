using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private int score = 0;
    [SerializeField] private int lifes = 3;
    private int highScore = 0;
    private int trys = 1;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI scoresText;
    [SerializeField] private TextMeshProUGUI counterText;
    [SerializeField] private TextMeshProUGUI lifesText;
    [SerializeField] private TextMeshProUGUI trysText;

    private const string scoresFormat = "{0:D4}           {1:D4}           0000";
    private const string trysFormat = "CREDIT {0:D2}";

    private void Awake()
    {
        instance = this;

        highScore = PlayerPrefs.GetInt("HighScore", 0);
        trys = PlayerPrefs.GetInt("Trys", 0);
        UpScores(0);
    }

    public void UpScores(int amount)
    {
        score += amount;
        scoresText.SetText(scoresFormat, score, highScore);
    }

    public void RemoveLife()
    {
        StepsTimer.PauseTimer = true;
        StepsTimer.Timer -= 2;

        lifes--;
        lifesText.SetText(lifes.ToString());

        if (lifes <= 0)
        {
            Time.timeScale = 0;
            Debug.Log("Game Over");

            trys++;
            PlayerPrefs.SetInt("Trys", trys);
            trysText.SetText(trysFormat, trys);
        }
    }
}