using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static bool GameStarted = false;

    [SerializeField] private GameplaySettings settings;
    private int lifes = 3;
    private int highScore = 0;
    private int trys = 1;

    [Space]
    [SerializeField] private PlayerInput playerInput;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI scoresText;
    [SerializeField] private TextMeshProUGUI counterText;
    [SerializeField] private TextMeshProUGUI lifesText;
    [SerializeField] private TextMeshProUGUI trysText;

    [Space]
    [SerializeField] private GameObject playerLife;
    [SerializeField] private Transform playerLifeRoot;

    private void Awake()
    {
        instance = this;

        for (int i = 0; i < playerLifeRoot.childCount; i++)
        {
            Destroy(playerLifeRoot.GetChild(i).gameObject);
        }
        for (int i = 0; i < lifes; i++)
        {
            Instantiate(playerLife, playerLifeRoot);
        }

        highScore = PlayerPrefs.GetInt("HighScore", 0);
        trys = PlayerPrefs.GetInt("Trys", 1);
        SetTrys();

        UpScores(0);
    }

    private void OnDestroy()
    {
        GameStarted = false;
    }

    private void Start()
    {
        _ = InitCounter();
    }

    public void UpScores(int amount)
    {
        PersistanceData.levelData.currentScore += amount;
        scoresText.SetText($"{PersistanceData.levelData.currentScore:D4}           {highScore:D4}           0000");
    }

    private void SetTrys()
    {
        trysText.SetText($"CREDIT {trys:D2}");
    }

    private async Task InitCounter()
    {
        scoresText.SetText(string.Empty);
        counterText.SetText(string.Empty);
        await Task.Delay(1000);
        UpScores(0);
        counterText.SetText($"LEVEL {PersistanceData.levelData.currentLevel + 1}");
        await Task.Delay(100);
        scoresText.SetText(string.Empty);
        counterText.SetText(string.Empty);
        await Task.Delay(100);
        UpScores(0);
        counterText.SetText($"LEVEL {PersistanceData.levelData.currentLevel + 1}");
        await Task.Delay(100);
        scoresText.SetText(string.Empty);
        counterText.SetText(string.Empty);
        await Task.Delay(100);
        UpScores(0);
        counterText.SetText($"LEVEL {PersistanceData.levelData.currentLevel + 1}");
        await Task.Delay(100);
        scoresText.SetText(string.Empty);
        counterText.SetText(string.Empty);
        await Task.Delay(100);
        UpScores(0);
        counterText.SetText($"LEVEL {PersistanceData.levelData.currentLevel + 1}");
        await Task.Delay(100);
        scoresText.SetText(string.Empty);
        counterText.SetText(string.Empty);
        await Task.Delay(100);
        UpScores(0);
        counterText.SetText($"LEVEL {PersistanceData.levelData.currentLevel + 1}");
        await Task.Delay(100);
        scoresText.SetText(string.Empty);
        counterText.SetText(string.Empty);

        await Task.Delay(250);
        UpScores(0);
        counterText.SetText("5");
        await Task.Delay(1000);
        counterText.SetText("4");
        await Task.Delay(1000);
        counterText.SetText("3");
        await Task.Delay(1000);
        counterText.SetText("2");
        await Task.Delay(1000);
        counterText.SetText("1");
        await Task.Delay(1000);
        counterText.SetText(string.Empty);

        GameStarted = true;
    }

    public void RemoveLife()
    {
        StepsTimer.PauseTimer = true;
        StepsTimer.Timer -= 2.5f;

        lifes--;
        lifesText.SetText(lifes.ToString());
        Destroy(playerLifeRoot.GetChild(playerLifeRoot.childCount - 1).gameObject);

        if (lifes <= 0)
        {
            counterText.SetText("GAME OVER");
            EndGame();
        }
    }

    private void ResetScene(InputAction.CallbackContext context)
    {
        playerInput.actions.FindAction("Submit").performed -= ResetScene;
        SceneManager.LoadScene(0);
    }

    public void WinRound()
    {
        if (PersistanceData.levelData.currentLevel == 9)
        {
            counterText.SetText("VICTORY");
            EndGame();
            return;
        }

        PersistanceData.UpLevel();
        ResetScene(default);
    }

    private void EndGame()
    {
        GameStarted = false;
        PlayerPrefs.SetInt("HighScore", PersistanceData.levelData.currentScore);
        UpScores(0);
        Destroy(PersistanceData.instance.gameObject);

        trys++;
        PlayerPrefs.SetInt("Trys", trys);
        SetTrys();

        playerInput.actions.FindAction("Submit").performed += ResetScene;
    }
}