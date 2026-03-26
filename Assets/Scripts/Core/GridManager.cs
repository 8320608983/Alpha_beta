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
        SetGrid();
        UIManager.Instance.menuPanel.HideView();
        UIManager.Instance.gamePlayPanel.ShowView();
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

        float width = rect.rect.width;
        float height = rect.rect.height;

        float spacingX = grid.spacing.x;
        float spacingY = grid.spacing.y;

        float totalSpacingX = spacingX * (cols - 1);
        float totalSpacingY = spacingY * (rows - 1);

        float cellWidth = (width - totalSpacingX) / cols;
        float cellHeight = (height - totalSpacingY) / rows;

        float size = Mathf.Min(cellWidth, cellHeight);
        grid.cellSize = new Vector2(size, size);

        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = cols;

        for (int i = 0; i < cols * rows; i++)
        {
            CardScript obj = Instantiate(cardScript, grid.transform);

        }



    }
}
