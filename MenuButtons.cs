using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public GameObject NewGameButton;
    public GameObject DifficultyButtons;
    public static string difficulty = "";

    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name); // need to include Unity Engine Scene Management
    }

    public void AssignDifficulty(string chosen_difficulty)
    {
        difficulty = chosen_difficulty;
    }

    public void NewGame()
    {
        NewGameButton.SetActive(false);
        DifficultyButtons.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
