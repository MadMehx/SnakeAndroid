using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Resume : MonoBehaviour
{
    // PlayGame
    // public void function. This function allows the user to press on the start
    // button for the application and it will load the game. The function 
    // uses the scenemanager in Unity to change between different scenes within
    // the application.
    public void PauseGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void ResumeGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
    //-----------------------------------------------------------------------------
    // QuitGame
    // public void function. This function allows the user to press on the quit
    // button for the application and it will quit the game. After the user quits
    // the application, the application should close and return the user back to
    // their home screen or their list of applications.
    public void QuitGame()
    {
        Application.Quit();
    }
}
