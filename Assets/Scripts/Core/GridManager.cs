using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;
    public GridLayoutGroup grid;
    public CardScript cardScript; 

    public int rows;
    public int cols;
    private void Awake()
    {
        if (Instance == null) { Instance = this; }
    }
    private void Start()
    {
        ClearGrid();
        GenerateGrid();
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

        for(int i = 0; i < cols * rows; i++)
        {
            CardScript obj = Instantiate(cardScript, grid.transform);

        }



    }
}
