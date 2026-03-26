using UnityEngine;

public class MenuPanel : BaseView
{
    public void Start_2x2()
    {
        GridManager.Instance.StartGame(2, 2);
    }
    public void Start_4x4()
    {
        GridManager.Instance.StartGame(4, 4);
    }

    public void Start_2x3()
    {
        GridManager.Instance.StartGame(2, 3);
    }

    public void Start_5x6()
    {
        GridManager.Instance.StartGame(5, 6);
    }
}
