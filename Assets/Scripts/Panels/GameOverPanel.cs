using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverPanel : BaseView
{
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
