using UnityEngine;
using System.Collections.Generic;

public class CollapseManager : MonoBehaviour
{
    public static int conditionA = 4, conditionB = 7, conditionC = 9;
    public BoardManager boardManager;

    public MatchFinder matchFinder;

    private void Awake()
    {
        boardManager = FindFirstObjectByType<BoardManager>();
        matchFinder = FindFirstObjectByType<MatchFinder>();
    }

    public void HandleGemClick(Gem clickedGem)
    {
        List<Gem> connectedGems = matchFinder.FindMatchesFromGem(clickedGem);

        if (connectedGems.Count >= 2) // Pop only if there’s a match
        {
            foreach (Gem gem in connectedGems)
            {
                StartCoroutine(gem.AnimateGemDestruction());
                RemoveGemFromBoard(gem);
            }

            boardManager.UpdateBoard(); // ✅ Updates board and renames gems
        }
    }

    // ✅ Function to remove the gem from the board
    private void RemoveGemFromBoard(Gem gem)
    {
        for (int row = 0; row < boardManager.rows; row++)
        {
            for (int col = 0; col < boardManager.columns; col++)
            {
                if (boardManager.boardGems[row, col] == gem)
                {
                    boardManager.boardGems[row, col] = null;
                    return;
                }
            }
        }
    }


    private List<Gem> GetConnectedGems(Gem startGem)
    {
        List<Gem> connectedGems = new List<Gem>();
        bool[,] visited = new bool[boardManager.rows, boardManager.columns];

        Vector2Int startPos = GetGemPosition(startGem);
        FindConnectedGemsRecursive(startPos.x, startPos.y, startGem.gemColorId, connectedGems, visited);

        return connectedGems;
    }

    private Vector2Int GetGemPosition(Gem gem)
    {
        for (int row = 0; row < boardManager.rows; row++)
        {
            for (int col = 0; col < boardManager.columns; col++)
            {
                if (boardManager.boardGems[row, col] == gem)
                    return new Vector2Int(row, col);
            }
        }
        return -Vector2Int.one;
    }

    private void FindConnectedGemsRecursive(int row, int col, int gemColorId, List<Gem> connectedGems, bool[,] visited)
    {
        if (row < 0 || row >= boardManager.rows || col < 0 || col >= boardManager.columns || visited[row, col]) return;

        Gem currentGem = boardManager.boardGems[row, col];
        if (currentGem != null && currentGem.gemColorId == gemColorId)
        {
            visited[row, col] = true;
            connectedGems.Add(currentGem);

            FindConnectedGemsRecursive(row + 1, col, gemColorId, connectedGems, visited);
            FindConnectedGemsRecursive(row - 1, col, gemColorId, connectedGems, visited);
            FindConnectedGemsRecursive(row, col + 1, gemColorId, connectedGems, visited);
            FindConnectedGemsRecursive(row, col - 1, gemColorId, connectedGems, visited);
        }
    }

}
