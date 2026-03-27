using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] internal MenuPanel menuPanel;
    [SerializeField] internal GamePlayPanel gamePlayPanel;
    [SerializeField] internal GameOverPanel gameOverPanel;

    [SerializeField] GameObject[] panels; 
    private void Awake()
    {
        if (Instance == null) { Instance = this; }
    } 
    public void CloseAllPanels()
    {
        foreach (var panel in panels) { panel.SetActive(false); }
    }

    public void RestartGame()
    {
        SaveSystem.Clear();
        CloseAllPanels(); 
        gamePlayPanel.ShowView();
        MatchManager.Instance.ResetSystem();
        ScoreManager.Instance.ResetScore();
        GridManager.Instance.SetGrid();
    }
}
