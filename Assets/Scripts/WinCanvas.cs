using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinCanvas : MonoBehaviour
{
    public string nextLevel;

    public void NextLevel()
    {
        SceneManager.LoadScene(nextLevel);

        Time.timeScale = 1;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
