using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseListener : MonoBehaviour {

    public bool paused = false;
    public GameObject pauseCanvas;
    public GameObject winCanvas;

    private GameObject[] crew;

    private void Start()
    {
        crew = GameObject.FindGameObjectsWithTag("Crew");
    }

    void Update () {
		if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused)
                ContinueGame();
            else
                PauseGame();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            winCanvas.GetComponent<WinCanvas>().ReloadLevel();
        }
	}

    public void ContinueGame()
    {
        Time.timeScale = 1;
        pauseCanvas.SetActive(false);
        foreach(GameObject c in crew)
        {
            c.GetComponent<CrewMember>().enabled = true;
        }
    }
    public void PauseGame()
    {
        Time.timeScale = 0;
        pauseCanvas.SetActive(true);
        foreach (GameObject c in crew)
        {
            c.GetComponent<CrewMember>().enabled = false;
        }
    }
}
