using System;
using UnityEngine;

public class QuitOnEscapeOrBack : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}

