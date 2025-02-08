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

        // pop only if there’s a match
        if (connectedGems.Count >= 2) 
        {
            foreach (Gem gem in connectedGems)
            {
                StartCoroutine(gem.AnimateGemDestruction());
                RemoveGemFromBoard(gem);
            }
        }
    }

    // function to remove the gem from the board
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
}
