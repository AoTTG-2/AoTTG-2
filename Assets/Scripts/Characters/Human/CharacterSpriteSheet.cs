using System;
using UnityEngine;

//Manages the texture offset of a sprite sheet material to display specific sprites
public class CharacterSpriteSheet : MonoBehaviour
{
    [SerializeField] private Material spriteMaterial;
    [SerializeField] private int sheetColumns;
    [SerializeField] private int sheetRows;
    [SerializeField] private int startColumn;
    [SerializeField] private int startRow;
    [SerializeField] private int endColumn;
    [SerializeField] private int endRow;

    private Material objectMaterial;
    private int numCells;
    private Vector2 cellDimensions = new Vector2();

    private void Awake()
    {
        CalculateNumCells();
        CalculateCellDimensions();
    }

    //Calculate the number of cells in the defined range
    private void CalculateNumCells()
    {
        int startingColumnCells = sheetRows - startRow;
        int middleColumnCells = (endColumn - startColumn - 1) * sheetRows;
        int endingColumnCells = endRow + 1;

        numCells = startingColumnCells + middleColumnCells + endingColumnCells;

        if (numCells < 0)
            numCells = 0;
    }

    private void CalculateCellDimensions()
    {
        cellDimensions.x = spriteMaterial.mainTexture.width / sheetColumns;
        cellDimensions.y = spriteMaterial.mainTexture.height / sheetRows;
    }

    //Calculate and set the offset needed to display the desired sprite
    public void SetSprite(int spriteID)
    {
        if (spriteID >= numCells)
            throw new IndexOutOfRangeException(message: "Invalid sprite ID: " + spriteID);

        int unclampedRow = startRow + spriteID;
        int targetColumn = startColumn + (unclampedRow / sheetRows);
        int targetRow = unclampedRow % sheetRows;

        objectMaterial.mainTextureOffset = new Vector2(cellDimensions.x * targetColumn, cellDimensions.y * targetRow);
    }
}