using UnityEngine;
using System.Collections.Generic;

public class CollapseManager : MonoBehaviour
{
    public static int conditionA = 4, conditionB = 7, conditionC = 9;

    public BoardManager boardManager;
    public MatchFinder matchFinder;

    // reference to the audio source
    private AudioSource popSound; 

    private void Awake()
    {
        boardManager = FindFirstObjectByType<BoardManager>();
        matchFinder = FindFirstObjectByType<MatchFinder>();
        popSound = GetComponent<AudioSource>(); 
    }

    public void HandleGemClick(Gem clickedGem)
    {
        List<Gem> connectedGems = matchFinder.FindMatchesFromGem(clickedGem);

        if (connectedGems.Count >= 2)
        {
            foreach (Gem gem in connectedGems)
            {
                StartCoroutine(gem.AnimateGemDestruction());
                RemoveGemFromBoard(gem);
            }

            // play pop sound when a gem is removed
            if (popSound != null)
            {
                popSound.Play();
            }

            // update board after removals
            boardManager.UpdateBoard();

            // reset box obstacle damage flags at the end of the move
            boardManager.ResetBoxDamageFlags();
        }
    }

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
