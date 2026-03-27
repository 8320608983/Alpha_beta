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

    private void Start()
    {
        CloseAllPanels();
        menuPanel.ShowView(); 
    }

    public void CloseAllPanels()
    {
        foreach (var panel in panels) { panel.SetActive(false); }
    }

    public void RestartGame()
    {
        CloseAllPanels(); 
        gamePlayPanel.ShowView();
        MatchManager.Instance.ResetSystem();
        GridManager.Instance.SetGrid();
    }
}
