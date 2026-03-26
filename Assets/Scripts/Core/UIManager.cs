using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] internal MenuPanel menuPanel;
    [SerializeField] internal GamePlayPanel gamePlayPanel;
    private void Awake()
    {
        if (Instance == null) { Instance = this; }
    }

    private void Start()
    {
        menuPanel.ShowView();
        gamePlayPanel.HideView();
    }
}
