using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameSession : MonoBehaviour
{
    public static GameSession Instance { get; private set; }

    [SerializeField] int startingLives = 3;
    [SerializeField] float comboWindow = 2f;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text livesText;
    [SerializeField] TMP_Text messageText;

    public int Score { get; private set; }
    public int Lives { get; private set; }
    public bool IsPlaying { get; private set; } = true;

    int enemiesAlive, combo;
    float lastDefeatTime = -99f;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        Lives = startingLives;
        ShowMessage("");
        RefreshUI();
    }

    void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    void Update()
    {
        if (!IsPlaying && Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void RegisterEnemy() => enemiesAlive++;

    public void AddPoints(int amount)
    {
        Score += amount;
        RefreshUI();
    }

    public void EnemyDefeated(int basePoints)
    {
        combo = (Time.time - lastDefeatTime <= comboWindow) ? combo + 1 : 1;
        lastDefeatTime = Time.time;
        AddPoints(basePoints * combo);

        enemiesAlive--;
        if (enemiesAlive <= 0)
        {
            IsPlaying = false;
            ShowMessage("LEVEL COMPLETE!\nPress R to restart");
        }
    }

    public void LoseLife()
    {
        Lives--;
        RefreshUI();
        if (Lives <= 0)
        {
            IsPlaying = false;
            ShowMessage("GAME OVER\nPress R to restart");
        }
    }

    void RefreshUI()
    {
        if (scoreText) scoreText.text = $"SCORE {Score:D6}";
        if (livesText) livesText.text = $"LIVES {Lives}";
    }

    void ShowMessage(string msg)
    {
        if (messageText) messageText.text = msg;
    }
}