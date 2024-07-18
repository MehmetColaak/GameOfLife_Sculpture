using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    public GameObject cellPrefab;
    private int rows = 50;
    private int columns = 50;
    public float cellSize = 1.2f;

    private GameObject[,] gridCells;
    private bool[,] currentCellStates;
    private bool[,] nextCellStates;
    private float heightOffset = 0f;

    private int maxLayer = 50;
    private int currentLayer = 0;

    public float generationDelay = 0.5f;


    void Start()
    {
        InitializeGrid();
    }

    public void InitializeGrid()
    {
        InstantiateGrid();

        SetCellStatus(26, 25, true);
        SetCellStatus(25, 26, true);
        SetCellStatus(26, 26, true);
        SetCellStatus(26, 27, true);
        SetCellStatus(27, 27, true);

        StartCoroutine(GenerateLayers());
    }

    IEnumerator GenerateLayers()
    {
        while (true)
        {
            CalculateNextCellStates();
            ApplyNextCellStates();
            currentLayer++; 

            if (currentLayer >= maxLayer)
            {
                yield break;
            }

            yield return new WaitForSeconds(generationDelay);
        }
    }

    void InstantiateGrid()
    {
        gridCells = new GameObject[rows, columns];
        currentCellStates = new bool[rows, columns];
        nextCellStates = new bool[rows, columns];

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                Vector3 position = new Vector3(col * cellSize, heightOffset, row * cellSize);
                gridCells[row, col] = Instantiate(cellPrefab, position, Quaternion.identity, transform);
                gridCells[row, col].SetActive(false);
            }
        }
    }

    public void SetCellStatus(int x, int y, bool isActive)
    {
        if (x >= 0 && x < columns && y >= 0 && y < rows)
        {
            gridCells[y, x].SetActive(isActive);
            currentCellStates[y, x] = isActive;
        }
        else
        {
            Debug.LogError("Invalid Coordinates.");
        }
    }

    void CalculateNextCellStates()
    {
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                int aliveNeighbors = CountAliveNeighbors(x, y);
                bool currentState = currentCellStates[y, x];

                if (currentState && (aliveNeighbors < 2 || aliveNeighbors > 3))
                {
                    nextCellStates[y, x] = false;
                }
                else if (!currentState && aliveNeighbors == 3)
                {
                    nextCellStates[y, x] = true;
                }
                else
                {
                    nextCellStates[y, x] = currentState;
                }
            }
        }
    }

    int CountAliveNeighbors(int x, int y)
    {
        int count = 0;

        for (int yOffset = -1; yOffset <= 1; yOffset++)
        {
            for (int xOffset = -1; xOffset <= 1; xOffset++)
            {
                int neighborX = x + xOffset;
                int neighborY = y + yOffset;

                if (xOffset == 0 && yOffset == 0)
                    continue;

                if (neighborX >= 0 && neighborX < columns && neighborY >= 0 && neighborY < rows)
                {
                    if (currentCellStates[neighborY, neighborX])
                    {
                        count++;
                    }
                }
            }
        }
        return count;
    }

    void ApplyNextCellStates()
    {
        heightOffset += cellSize;

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                gridCells[y, x] = Instantiate(cellPrefab, new Vector3(x * cellSize, heightOffset, y * cellSize), Quaternion.identity, transform);
                gridCells[y, x].SetActive(nextCellStates[y, x]);
                currentCellStates[y, x] = nextCellStates[y, x];
            }
        }
    }
}
