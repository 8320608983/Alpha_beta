using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;
    [SerializeField] GridLayoutGroup grid;
    [SerializeField] CardScript cardScript;
    [SerializeField] CardDatabase database;

    [SerializeField] int rows;
    [SerializeField] int cols;

    [Header("Card Details")]
    [SerializeField] float heightCard;
    [SerializeField] float widthCard;

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
    }
    public void SetGrid()
    {
        ClearGrid();
        GenerateGrid();
    }
    public void StartGame(int rowCount, int colCount)
    { 
        rows = rowCount;
        cols = colCount;
        StartCoroutine(InitGrid()); 
        ScoreManager.Instance.ResetScore();
        UIManager.Instance.menuPanel.HideView();
        UIManager.Instance.gamePlayPanel.ShowView();
    }
    IEnumerator InitGrid()
    {
        yield return null;  

        LayoutRebuilder.ForceRebuildLayoutImmediate(grid.GetComponent<RectTransform>());

        SetGrid();
    }
    void ClearGrid()
    {
        foreach (Transform child in grid.transform)
        {
            Destroy(child.gameObject);
        }
    }
    void GenerateGrid()
    {
        RectTransform rect = grid.GetComponent<RectTransform>();
         
        LayoutRebuilder.ForceRebuildLayoutImmediate(rect);

        float width = rect.rect.width;
        float height = rect.rect.height;

        if (width <= 0 || height <= 0)
        {
            Debug.LogError("Invalid grid size!");
            return;
        }

        float spacingX = grid.spacing.x;
        float spacingY = grid.spacing.y;

        float totalSpacingX = spacingX * (cols - 1);
        float totalSpacingY = spacingY * (rows - 1);

        float cellWidth = (width - totalSpacingX) / cols;
        float cellHeight = (height - totalSpacingY) / rows;
         
        float aspect = widthCard / heightCard;
         
        float cardHeight = cellHeight;
         
        float cardWidth = cardHeight * aspect;

        if (cardWidth > cardHeight)
        { 
            cardHeight = cardWidth / aspect;
            cardWidth = cardHeight /aspect  ;
        } 
        grid.cellSize = new Vector2(cardWidth, cardHeight);
         

        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = cols;

        int totalCells = rows * cols;
        int pairCount = totalCells / 2;
        if (database.cards.Count < pairCount)
        {
            Debug.LogError("Not enough CardData in database!");
            return;
        }

        MatchManager.Instance.SetTotalPairs(pairCount);

        List<CardData> temp = new List<CardData>(database.cards);
        Shuffle(temp);

        List<CardData> finalList = new List<CardData>();

        for (int i = 0; i < pairCount; i++)
        {
            finalList.Add(temp[i]);
            finalList.Add(temp[i]);
        }

        Shuffle(finalList);
        
        foreach (var data in finalList)
        {
            CardScript obj = Instantiate(cardScript, grid.transform);
            obj.Setup(data);
        }

    }
    void Shuffle(List<CardData> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rand = Random.Range(i, list.Count);
            (list[i], list[rand]) = (list[rand], list[i]);
        }
    }
}
