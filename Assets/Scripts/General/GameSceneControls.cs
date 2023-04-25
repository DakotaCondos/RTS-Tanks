using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneControls : MonoBehaviour
{
    public void Quit()
    {
        print("Exiting Application");
        Application.Quit();
    }

    public void MainMenu()
    {
        RTSNetworkManager.singleton.StopClient();
        SceneManager.LoadScene("MainMenu");
    }
}
