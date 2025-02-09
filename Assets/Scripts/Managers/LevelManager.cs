using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelManager : MonoBehaviour
{
    public void Load10x10()
    {
        SceneManager.LoadScene("10x10");
    }

    public void Load5x8()
    {
        SceneManager.LoadScene("5x8");
    }
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
