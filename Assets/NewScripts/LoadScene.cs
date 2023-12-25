using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using MyBox;

public class LoadScene : MonoBehaviour
{
    /*
    public void ButtonPress(string name)
    {
        SceneManager.LoadScene(name);
    }
    */

    [Scene]
    public string scene;

    Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(NextScene);
    }

    public void NextScene()
    {
        SceneManager.LoadScene(scene);
    }

}
