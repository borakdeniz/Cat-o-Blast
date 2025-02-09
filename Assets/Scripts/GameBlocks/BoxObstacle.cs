using UnityEngine;

public class BoxObstacle : Gem
{
    public int health = 2; 
    private Transform damageIndicator; 

    public bool hasTakenDamageThisAction = false;

    private BoardManager boardManager;

    private void Awake()
    {
        isObstacle = true; 
        damageIndicator = transform.GetChild(0); 
        damageIndicator.gameObject.SetActive(false);

        // get the board manager and properly remove the box from the board
        CollapseManager collapseManager = FindFirstObjectByType<CollapseManager>();
    }

    public void TakeDamage()
    {
        if (hasTakenDamageThisAction) return;

        health--;
        hasTakenDamageThisAction = true;

        if (health == 1)
        {
            if (damageIndicator != null)
                damageIndicator.gameObject.SetActive(true);
        }
        else if (health <= 0)
        {
            BoardManager boardManager = FindFirstObjectByType<BoardManager>();

            if (boardManager != null)
            {
                boardManager.RemoveGemFromBoard(this); // remove from board first
            }

            Destroy(gameObject); // fully remove the box
        }
    }


}
