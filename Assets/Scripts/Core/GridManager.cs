using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;
    [SerializeField] GridLayoutGroup grid;
    [SerializeField] CardScript cardScript;

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
        UIManager.Instance.menuPanel.HideView();
        UIManager.Instance.gamePlayPanel.ShowView();
    }
    IEnumerator InitGrid()
    {
        yield return null; // wait for UI layout

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
        print("cardWidth : " + cardWidth + " > cardHeight : " + cardHeight);
            cardHeight = cardWidth / aspect;
            cardWidth = cardHeight /aspect  ;
        }
        print("cardWidth : " + cardWidth + " : cardWidth : " + cardHeight);
        grid.cellSize = new Vector2(cardWidth, cardHeight);
         

        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = cols;
         
        for (int i = 0; i < rows * cols; i++)
        {
            CardScript obj = Instantiate(cardScript, grid.transform);
            obj.Setup();
        }


    }
}
