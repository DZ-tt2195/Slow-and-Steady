using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugScript : MonoBehaviour
{
    void Start()
    {
        //Debug.Log($"Framerate: {Application.targetFrameRate}");
        Application.targetFrameRate = 60;
        Debug.Log($"Screensize: {Screen.width}, {Screen.height}");
    }

}
