using UnityEngine;
using UnityEngine.UI;
public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [Header("UI")]
    [SerializeField] Text scoreText;
    [SerializeField] Text comboText;

    private int score;
    private int combo;

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
    }

    private void OnEnable()
    {
        GameEvents.OnMatch += HandleMatch;
        GameEvents.OnMismatch += HandleMismatch;
    }

    private void OnDisable()
    {
        GameEvents.OnMatch -= HandleMatch;
        GameEvents.OnMismatch -= HandleMismatch;
    }
     
    void HandleMatch()
    {
        combo++;

        int points = 10 * combo;  
        score += points;

        UpdateUI();
    }
     
    void HandleMismatch()
    {
        combo = 0;
        UpdateUI();
    }

    void UpdateUI()
    {
        scoreText.text = "Score: " + score;

        if (combo > 1)
            comboText.text = "Combo x" + combo;
        else
            comboText.text = "";
    }

    public void ResetScore()
    {
        score = 0;
        combo = 0;
        UpdateUI();
    }

    public int GetScore()
    {
        return score;
    }
}