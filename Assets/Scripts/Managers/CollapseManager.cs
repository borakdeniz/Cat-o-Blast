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
                // check if the gem has a sprite before removing it
                SpriteRenderer spriteRenderer = gem.GetComponent<SpriteRenderer>();

                if (spriteRenderer == null || spriteRenderer.sprite == null)
                {
                    RemoveGemFromBoard(gem);
                    Destroy(gem.gameObject);
                }
                else
                {
                    StartCoroutine(gem.AnimateGemDestruction());
                    RemoveGemFromBoard(gem);
                }
            }

            // update board after removals
            boardManager.UpdateBoard();

            // reset box obstacle damage flags at the end of the move
            boardManager.ResetBoxDamageFlags();
        }
    }



    // function to remove the gem from the board
    public void RemoveGemFromBoard(Gem gem)
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
