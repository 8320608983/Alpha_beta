using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : BaseView
{
    [SerializeField] Text finalScore;
    private void OnEnable()
    {
        ShowScore();
    }
    void ShowScore()
    {
        int FinalScore = ScoreManager.Instance.GetScore();
        finalScore.text = "Final Score: " + FinalScore;
    }
    
    public void On_Restart_BtnClick()
    {
        UIManager.Instance.RestartGame();
    }

    public void On_MainMenu_BtnClick()
    {
        UIManager.Instance.CloseAllPanels();
        UIManager.Instance.menuPanel.ShowView();
    }
}
