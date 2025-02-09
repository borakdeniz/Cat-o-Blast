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
            // activate the child to show damage state
            if (damageIndicator != null)
                damageIndicator.gameObject.SetActive(true);
        }
        else if (health <= 0)
        {
            if (boardManager != null)
            {
                collapseManager.RemoveGemFromBoard(this);
            }
            Destroy(gameObject); // fully destroy the obstacle
        }
    }

}
