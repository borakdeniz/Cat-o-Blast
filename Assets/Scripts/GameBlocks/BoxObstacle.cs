using UnityEngine;

public class BoxObstacle : Gem
{
    private int health = 2; 
    private Transform damageIndicator; 

    public bool hasTakenDamageThisAction = false;

    private void Awake()
    {
        isObstacle = true; 
        damageIndicator = transform.GetChild(0); 
        damageIndicator.gameObject.SetActive(false); 
    }

    public void TakeDamage()
    {
        health--;

        if(!hasTakenDamageThisAction)
        {
            if (health == 1)
            {
                // activate the child to show damage state
                if (damageIndicator != null)
                    damageIndicator.gameObject.SetActive(true);
            }
            else if (health <= 0)
            {
                // removes the box visually
                Destroy(gameObject);
            }
        }

        // to keep boxes from taking multiple instances of damage same turn
        hasTakenDamageThisAction = true;

    }
}
