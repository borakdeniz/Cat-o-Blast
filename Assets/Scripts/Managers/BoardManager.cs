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
        ApplyLevelData();
        SetupBoard();
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

            CollapseManager.conditionA = currentLevelData.conditionA;
            CollapseManager.conditionB = currentLevelData.conditionB;
            CollapseManager.conditionC = currentLevelData.conditionC;
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
        RefreshGemIcons();
    }

    void InitializeBoard()
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                SpawnGem(row, col);
            }
        }
    }

    public void SpawnGem(int row, int col)
    {
        GameObject gemObj = gemPool.GetGem();
        gemObj.transform.SetParent(transform);

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
        gem.UpdateGemSprite(1);

        StartCoroutine(DropGem(gem, row, col));
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

    public void UpdateBoard()
    {
        for (int col = 0; col < columns; col++)
        {
            int emptySlots = 0;
            for (int row = rows - 1; row >= 0; row--)
            {
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
        StartCoroutine(DelayedRefreshIcons(dropDuration));
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

    private void RefreshGemIcons()
    {
        bool[,] visited = new bool[rows, columns];
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                if (visited[row, col] || boardGems[row, col] == null)
                    continue;
                List<Gem> connectedGroup = new List<Gem>();
                FindConnectedGems(row, col, boardGems[row, col].gemColorId, connectedGroup, visited);
                foreach (Gem gem in connectedGroup)
                {
                    gem.UpdateGemSprite(connectedGroup.Count);
                }
            }
        }
    }

    private void FindConnectedGems(int row, int col, int gemColorId, List<Gem> group, bool[,] visited)
    {
        if (row < 0 || row >= rows || col < 0 || col >= columns)
            return;
        if (visited[row, col])
            return;
        Gem gem = boardGems[row, col];
        if (gem == null || gem.gemColorId != gemColorId)
            return;
        visited[row, col] = true;
        group.Add(gem);
        FindConnectedGems(row + 1, col, gemColorId, group, visited);
        FindConnectedGems(row - 1, col, gemColorId, group, visited);
        FindConnectedGems(row, col + 1, gemColorId, group, visited);
        FindConnectedGems(row, col - 1, gemColorId, group, visited);
    }

    private IEnumerator DelayedRefreshIcons(float delay)
    {
        yield return new WaitForSeconds(delay);
        RefreshGemIcons();
    }
}
