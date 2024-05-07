using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    public GameObject cellPrefab;
    private int rows = 50;
    private int columns = 50;
    public float cellSize = 1.2f;
    private int gridWidth;

    private GameObject[] gridCells;
    private bool[,] currentCellStates;
    private bool[,] nextCellStates;

    void Start()
    {
        gridWidth = columns;

        InstantiateGrid();
        LoadGridCellsToArray();


        SetCellStatus(26, 25, true);
        SetCellStatus(25, 26, true);
        SetCellStatus(26, 26, true);
        SetCellStatus(26, 27, true);
        SetCellStatus(27, 27, true);
    }

    void Update()
    {
        CalculateNextCellStates();
        ApplyNextCellStates();
    }

    void InstantiateGrid()
    {
        for(int row = -rows/2; row < rows/2; row++)
        {
            for(int col = -columns/2; col < columns/2; col++)
            {
                Vector3 position = new Vector3(col * cellSize, 0f, row * cellSize);
                Instantiate(cellPrefab, position, Quaternion.identity, transform);
            }
        }
    }

    void LoadGridCellsToArray()
    {
        int childCount = transform.childCount;
        gridCells = new GameObject[childCount];
        currentCellStates = new bool[rows, columns];
        nextCellStates = new bool[rows, columns];

        for (int i = 0; i < childCount; i++)
        {
            gridCells[i] = transform.GetChild(i).gameObject;
            gridCells[i].SetActive(false);

            int x = i % gridWidth;
            int y = i / gridWidth;
            currentCellStates[y, x] = false;
        }
    }

    public void SetCellStatus(int x, int y, bool isActive)
    {
        int index = y * gridWidth + x;

        if(index >= 0 && index < gridCells.Length)
        {
            gridCells[index].SetActive(isActive);
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
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                int index = y * gridWidth + x;
                gridCells[index].SetActive(nextCellStates[y, x]);
                currentCellStates[y, x] = nextCellStates[y, x];
            }
        }
    }
}