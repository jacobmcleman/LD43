using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectCanvas : MonoBehaviour
{
    public int[] DontShowLevels = { 0, 1 };
    public GameObject levelLoadButtonPrefab;
    public RectTransform buttonsFitIn;

    public int gridX = 6;
    public int gridY = 3;
    public float padding = 20;

    private void Start()
    {
        LevelSelectButton.LoadProgress();

        int levelCount = SceneManager.sceneCountInBuildSettings;
        int levelNum = 0;

        RectTransform rt = buttonsFitIn;

        float leftEdge = rt.rect.xMin;
        float topEdge = rt.rect.yMax;

        float width = rt.rect.width;
        float height = rt.rect.height;

        float xSpacing = width / gridX;
        float ySpacing = height / gridY;

        float buttonWidth = xSpacing - 2 * padding;
        float buttonHeight = ySpacing - 2 * padding;

        for (int i = 0; i < levelCount; ++i)
        {
            if(System.Array.IndexOf(DontShowLevels, i) == -1)
            {
                string levelName = SceneUtility.GetScenePathByBuildIndex(i);

                levelName = levelName.Substring(levelName.LastIndexOf('/') + 1);
                levelName = levelName.Substring(0, levelName.LastIndexOf('.'));

                SceneUtility.GetScenePathByBuildIndex(i);

                GameObject button = Instantiate(levelLoadButtonPrefab);
                button.transform.parent = rt;
                button.transform.localPosition = new Vector3(leftEdge + (xSpacing * (levelNum % gridX) + buttonWidth / 2), topEdge - (ySpacing * (levelNum / gridX) + buttonHeight / 2), 0);
                ++levelNum;

                button.GetComponent<LevelSelectButton>().levelLoadIndex = i;
                button.transform.GetChild(0).GetComponent<Text>().text = levelName;
            }
        }
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
