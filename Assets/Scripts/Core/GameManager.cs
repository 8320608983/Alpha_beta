using UnityEngine;
using UnityEngine.Playables;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static bool isLoadedFromSave = false;
    private void Awake()
    {
        if (Instance == null) { Instance = this; }
    }

    void Start()
    {
        GameData data = SaveSystem.Load();

        if (data != null)
        {
            isLoadedFromSave = true;
            Debug.Log("Loading Data!!!");
            GridManager.Instance.StartGame(data.rows, data.cols); 
        }
        else
        {
            Debug.Log("No Data Saved Yet !!!");
            UIManager.Instance.menuPanel.ShowView();
        }
    }
}

[System.Serializable]
public class GameData
{ 
    public int rows;
    public int cols;
}
public static class SaveSystem
{
    private const string SAVE_KEY = "save_data";

    public static void Save(GameData data)
    {
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(SAVE_KEY, json);
        PlayerPrefs.Save();
        //Debug.Log("Game Saved");
    }

    public static GameData Load()
    {
        if (!PlayerPrefs.HasKey(SAVE_KEY))
            return null;

        string json = PlayerPrefs.GetString(SAVE_KEY);
       // Debug.Log("Game Load in process");
        return JsonUtility.FromJson<GameData>(json);
    }

    public static void Clear()
    {
        PlayerPrefs.DeleteKey(SAVE_KEY);
    }
}