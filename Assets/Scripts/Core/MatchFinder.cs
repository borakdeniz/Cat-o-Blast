using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MatchFinder : MonoBehaviour
{
    private BoardManager boardManager;

    public List<Gem> currentMatches = new List<Gem>();

    public List<Gem> autoMatches = new List<Gem>();

    private void Awake()
    {
        boardManager = FindFirstObjectByType<BoardManager>();
    }

    public List<Gem> FindMatchesFromGem(Gem startGem)
    {
        currentMatches.Clear();

        Queue<Gem> gemsToCheck = new Queue<Gem>();
        HashSet<Gem> checkedGems = new HashSet<Gem>();

        gemsToCheck.Enqueue(startGem);
        checkedGems.Add(startGem);

        while (gemsToCheck.Count > 0)
        {
            Gem currentGem = gemsToCheck.Dequeue();
            currentMatches.Add(currentGem);

            foreach (Gem neighborGem in GetNeighbors(currentGem))
            {
                if (neighborGem != null && neighborGem.gemColorId == startGem.gemColorId && !checkedGems.Contains(neighborGem))
                {
                    gemsToCheck.Enqueue(neighborGem);
                    checkedGems.Add(neighborGem);
                }
            }
        }

        return currentMatches.Count > 1 ? currentMatches : new List<Gem>(); // Return empty list if no match found
    }

    private List<Gem> GetNeighbors(Gem gem)
    {
        List<Gem> neighbors = new List<Gem>();
        Vector2Int pos = boardManager.GetGemPosition(gem);

        neighbors.Add(boardManager.GetGemAtPosition(new Vector2Int(pos.x - 1, pos.y))); // Left
        neighbors.Add(boardManager.GetGemAtPosition(new Vector2Int(pos.x + 1, pos.y))); // Right
        neighbors.Add(boardManager.GetGemAtPosition(new Vector2Int(pos.x, pos.y + 1))); // Up
        neighbors.Add(boardManager.GetGemAtPosition(new Vector2Int(pos.x, pos.y - 1))); // Down

        return neighbors.Where(n => n != null).ToList();
    }

    public void FindAutoMatches()
    {
        autoMatches.Clear();

        for (int col = 0; col < boardManager.columns; col++) // loop columns
        {
            for (int row = 0; row < boardManager.rows; row++) // loop rows
            {
                Gem currentGem = boardManager.boardGems[row, col];

                if (currentGem != null)
                {
                    List<Gem> connectedGems = FindConnectedMatches(currentGem);

                    // Track the matches
                    foreach (Gem gem in connectedGems)
                    {
                        if (connectedGems.Count >= 2)
                        {
                            autoMatches.Add(gem);
                        }
                        gem.UpdateGemSprite(connectedGems.Count);
                    }
                }
            }
        }
    }

    private List<Gem> FindConnectedMatches(Gem startGem)
    {
        List<Gem> connectedGems = new List<Gem>();
        Queue<Gem> gemsToCheck = new Queue<Gem>();
        gemsToCheck.Enqueue(startGem);
        connectedGems.Add(startGem);

        while (gemsToCheck.Count > 0)
        {
            Gem currentGem = gemsToCheck.Dequeue();

            foreach (Gem neighbor in GetNeighbors(currentGem))
            {
                if (neighbor != null && !connectedGems.Contains(neighbor) && neighbor.gemColorId == startGem.gemColorId)
                {
                    connectedGems.Add(neighbor);
                    gemsToCheck.Enqueue(neighbor);
                }
            }
        }

        return connectedGems;
    }
}
