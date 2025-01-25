using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void SceneChange(string name)
    {
        if (GameManager.instance != null)
        {
            Destroy(GameManager.instance.gameObject);
        }
        SceneManager.LoadScene(name);
        Time.timeScale = 1f;
    }
}
