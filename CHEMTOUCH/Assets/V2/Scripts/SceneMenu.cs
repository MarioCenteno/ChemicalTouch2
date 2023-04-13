using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMenu : MonoBehaviour
{
    public void LoadMainGame()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadSandBox()
    {
        SceneManager.LoadScene(3);
    }

    public void LoadOptions()
    {

    }
}
