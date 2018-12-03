using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;

public class LevelSelectButton : MonoBehaviour {

    private static int progressData = 0;
    public static int FirstLevel = 2;

    public static void LoadProgress()
    {
        progressData = PlayerPrefs.GetInt("levelCompletion");
    }

    public static void SaveProgress()
    {
        PlayerPrefs.SetInt("levelCompletion", progressData);
    }

    public static bool HasCompleted(int levelIndex)
    {
        return 0 != (progressData & (1 << levelIndex));
    }

    public static bool HasCompleted(string level)
    {
        return HasCompleted(SceneManager.GetSceneByName(level).buildIndex);
    }

    public static void SetCompletion(int levelIndex, bool completed)
    {
        if (completed)
        {
            progressData = progressData ^ (1 << levelIndex);
        }
        else
        {
            progressData = progressData & ~(1 << levelIndex);
        }

        SaveProgress();
    }

    public static void SetCompletion(string level, bool completed)
    {
        SetCompletion(SceneManager.GetSceneByName(level).buildIndex, completed);
    }

    public static void SetThisLevelCompleted(bool completed)
    {
        SetCompletion(SceneManager.GetActiveScene().buildIndex, completed);
    }

    public static bool HasCompletedPreviousLevel(int levelIndex)
    {
        if (levelIndex == FirstLevel) return true;
        else return HasCompleted(levelIndex - 1);
    }

    public static bool HasCompletedPreviousLevel(string level)
    {
        return HasCompletedPreviousLevel(SceneManager.GetSceneByName(level).buildIndex);
    }

    public int levelLoadIndex = 0;

    public void LoadLevel()
    {
        if(HasCompletedPreviousLevel(levelLoadIndex)) SceneManager.LoadScene(levelLoadIndex);
    }

    private void Start()
    {
        if (!HasCompletedPreviousLevel(levelLoadIndex))
        {
            GetComponent<Button>().interactable = false;
        }
    }
}
