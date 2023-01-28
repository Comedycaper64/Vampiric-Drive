using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    int currentSceneIndex;
    [SerializeField] GameObject endText;


    // Use this for initialization
    void Start()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    public int GetCurrentSceneIndex()
    {
        return currentSceneIndex;
    }

    public IEnumerator WaitForRestart(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        RestartScene();
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadOptionsScreen()
    {
        SceneManager.LoadScene("Options");
    }

    public void LoadLevelSelect()
    {
        SceneManager.LoadScene("Level Select");
    }

    public void LoadNextScene()
    {
        if (currentSceneIndex == 7)
        {
            endText.SetActive(true);
        }
        else
        {
            SceneManager.LoadScene(currentSceneIndex + 1);
        }
    }

    public void LoadLevel(int levelNumber)
    {
        SceneManager.LoadScene(levelNumber);
    }

    public void LoadYouLose()
    {
        SceneManager.LoadScene("Lose Screen");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
