using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseCanvas : MonoBehaviour {

    public void Quit()
    {
        Application.Quit();
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    private void OnEnable()
    {
       GameObject.Find("LevelText").GetComponent<Text>().text = SceneManager.GetActiveScene().name;
    }
}
