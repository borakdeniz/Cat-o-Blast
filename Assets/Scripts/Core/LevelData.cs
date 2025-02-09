using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Game/LevelData", order = 1)]
public class LevelData : ScriptableObject
{
    [Header("Board Settings")]
    public int rows = 10;
    public int columns = 12;
    public int totalColors = 6;

    [Header("Block Spawn & Drop")]
    public float spawnHeight = 2.0f;

    // drop time
    public float dropDuration = 0.3f;  

    [Range(0f, 1f)]
    // probability for possible matches while creating board
    public float neighborMatchProbability = 0.8f; 
}
