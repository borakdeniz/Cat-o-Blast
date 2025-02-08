using UnityEngine;

public class BoxObstacle : Gem
{
    public int health = 2;

    private void Awake()
    {
        // ensures this gem is identified as an obstacle
        isObstacle = true;
    }

    public void TakeDamage()
    {
        //health--;

        //if (health <= 0)
        //{
        //    Destroy(gameObject);
        //}
    }
}
