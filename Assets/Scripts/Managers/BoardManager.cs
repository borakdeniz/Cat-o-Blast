using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour
{
    [Header("Level Data")]
    public LevelData currentLevelData;

    [HideInInspector]
    public int rows, columns;

    public Gem[,] boardGems;
    public GemPool gemPool;
    private MatchFinder matchFinder;

    public GameObject boxObstaclePrefab; // assign in unity inspector
    public float boxSpawnChance = 0.1f; // 10% chance to spawn an obstacle



    [HideInInspector]
    public int totalColors;
    [HideInInspector]
    public float spawnHeight;
    [HideInInspector]
    public float dropDuration;
    [HideInInspector]
    public float neighborMatchProbability;

    private void Start()
    {
        matchFinder = FindFirstObjectByType<MatchFinder>();
        ApplyLevelData();
        SetupBoard();
    }


    private void Update()
    {
        UpdateBoard();
    }
    

    private void ApplyLevelData()
    {
        if (currentLevelData != null)
        {
            rows = currentLevelData.rows;
            columns = currentLevelData.columns;
            totalColors = currentLevelData.totalColors;
            spawnHeight = currentLevelData.spawnHeight;
            dropDuration = currentLevelData.dropDuration;
            neighborMatchProbability = currentLevelData.neighborMatchProbability;
        }
        else
        {
            Debug.LogError("LevelData is not assigned!");
        }
    }
    public void SetupBoard()
    {
        boardGems = new Gem[rows, columns];
        InitializeBoard();
    }

    void InitializeBoard()
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                // 10% chance to spawn a box obstacle
                if (Random.value < boxSpawnChance)
                {
                    SpawnBoxObstacle(row, col);
                }
                else
                {
                    SpawnGem(row, col);
                }
            }
        }
    }


    public void SpawnGem(int row, int col)
    {
        GameObject gemObj = gemPool.GetGem();

        // find or create the column parent in the hierarchy
        Transform columnParent = GetOrCreateColumnParent(col);

        // assign gem to its column group
        gemObj.transform.SetParent(columnParent); 

        Vector3 startPosition = new Vector3(col, spawnHeight, 0);
        gemObj.transform.localPosition = startPosition;

        Gem gem = gemObj.GetComponent<Gem>();

        int chosenColor;
        if (row < rows - 1 && boardGems[row + 1, col] != null && Random.value < neighborMatchProbability)
        {
            chosenColor = boardGems[row + 1, col].gemColorId;
        }
        else
        {
            chosenColor = Random.Range(0, totalColors);
        }
        gem.gemColorId = chosenColor;
        boardGems[row, col] = gem;

        // set the name correctly in Unity Hierarchy (Column first, then Row)
        gemObj.name = $"Gem ({col}, {row})";

        StartCoroutine(DropGem(gem, row, col));
    }

    public void SpawnBoxObstacle(int row, int col)
    {
        // ensure the position is within bounds and not already occupied
        if (row < 0 || row >= rows || col < 0 || col >= columns || boardGems[row, col] != null)
            return;

        GameObject boxObj = Instantiate(boxObstaclePrefab);

        // find or create the column parent in the hierarchy
        Transform columnParent = GetOrCreateColumnParent(col);

        // assign box to its column group
        boxObj.transform.SetParent(columnParent);
        boxObj.transform.localPosition = new Vector3(col, -row, 0);

        BoxObstacle box = boxObj.GetComponent<BoxObstacle>();
        boardGems[row, col] = box;

        // set the name correctly in unity hierarchy (column first, then row)
        boxObj.name = $"BoxObstacle ({col}, {row})";
    }

    // creates or finds a parent object for each column
    private Transform GetOrCreateColumnParent(int col)
    {
        string columnName = $"Column {col}";
        GameObject columnObj = GameObject.Find(columnName);

        // if it doesn't exist, create it
        if (columnObj == null) 
        {
            columnObj = new GameObject(columnName);

            // parent to BoardManager
            columnObj.transform.SetParent(transform); 
        }

        return columnObj.transform;
    }

    private IEnumerator DropGem(Gem gem, int row, int col)
    {
        Vector3 startPos = gem.transform.localPosition;
        Vector3 targetPos = new Vector3(col, -row, 0);
        float elapsedTime = 0f;

        while (elapsedTime < dropDuration)
        {
            gem.transform.localPosition = Vector3.Lerp(startPos, targetPos, elapsedTime / dropDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        gem.transform.localPosition = targetPos;
    }

    public Vector2Int GetGemPosition(Gem gem)
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                if (boardGems[row, col] == gem)
                    return new Vector2Int(col, row);
            }
        }
        return Vector2Int.one * -1;
    }

    public Gem GetGemAtPosition(Vector2Int position)
    {
        if (position.x >= 0 && position.x < columns && position.y >= 0 && position.y < rows)
        {
            return boardGems[position.y, position.x];
        }
        return null;
    }

    public void UpdateBoard()
    {
        for (int col = 0; col < columns; col++)
        {
            int emptySlots = 0;

            for (int row = rows - 1; row >= 0; row--)
            {
                if (boardGems[row, col] != null && boardGems[row, col].isObstacle)
                {
                    emptySlots = 0;
                    continue;
                }

                if (boardGems[row, col] == null)
                {
                    emptySlots++;
                }
                else if (emptySlots > 0)
                {
                    boardGems[row + emptySlots, col] = boardGems[row, col];
                    boardGems[row, col] = null;

                    StartCoroutine(MoveGemDown(boardGems[row + emptySlots, col], row + emptySlots, col));
                }
            }

            for (int i = 0; i < emptySlots; i++)
            {
                SpawnGem(i, col);
            }
        }

        // find and update gem icons to match the match count
        matchFinder.FindAutoMatches();

        // rename all gems in the hierarchy
        RenameAllGems();
    }



    // function to rename all gems with correct positions
    private void RenameAllGems()
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                if (boardGems[row, col] != null)
                {
                    boardGems[row, col].gameObject.name = $"Gem ({col}, {row})"; // Column first, then Row
                }
            }
        }
    }

    private IEnumerator MoveGemDown(Gem gem, int newRow, int col)
    {
        Vector3 startPosition = gem.transform.localPosition;
        Vector3 targetPosition = new Vector3(col, -newRow, 0);
        float elapsedTime = 0f;

        while (elapsedTime < dropDuration)
        {
            gem.transform.localPosition = Vector3.Lerp(startPosition, targetPosition, elapsedTime / dropDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        gem.transform.localPosition = targetPosition;
    }
    public void ResetBoxDamageFlags()
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                Gem gem = boardGems[row, col];

                if (gem is BoxObstacle box)
                {
                    // reset flag so the box can take damage next turn
                    box.hasTakenDamageThisAction = false;
                }
            }
        }
    }


}
