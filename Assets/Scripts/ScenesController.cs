using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesController : MonoBehaviour
{

    public void LoadScene(string sceneName)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneName);
    }

    public void Exit()
    {
        Debug.Log("Encerrando aplicação");
        Application.Quit();
    }

    public static string activeScene()
    {
        return SceneManager.GetActiveScene().name;
    }
}
