using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//-----------------------------------------------------------------------------
// Creates functionalility for the resume and quit button for the application 
// for the pause menu. It uses Unity's SceneManagement library to help build the
// function needed to shift to a different scene.
//
// Assumptions, implementation details
//   -- There are two buttons 
//   -- The buttons properly reference the functions
public class pause : MonoBehaviour
{
    //-----------------------------------------------------------------------------
    // PauseGame
    // public void function. This function allows the user to press on the pause
    // button for the application and change into a different scene. The function 
    // uses the scenemanager in Unity to change between different scenes within
    // the application.
    public void PauseGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    //-----------------------------------------------------------------------------
    // ResumeGame
    // public void function. This function allows the user to press on the start
    // resume for the application and it will reload the game. The function 
    // uses the scenemanager in Unity to change between different scenes within
    // the application.
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
