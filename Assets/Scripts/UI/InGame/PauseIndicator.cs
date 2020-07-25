using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class PauseIndicator : MonoBehaviour
{
    FengGameManagerMKII gameManager;
    private bool unpausing = false;

    /// <summary>
    /// Displays the pause indicator with the text "Game Paused"
    /// </summary>
    public void Pause()
    {
        gameObject.GetComponentInChildren<Text>().text = "Game Paused";
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Sets the process to remove the pause indicator
    /// </summary>
    public void UnPause()
    {
        unpausing = true;
    }

    // Use this for initialization
    void Start()
    {
        gameManager = GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>();
    }

    // Update is called once per frame
    void Update()
    {
        // Only need constant updates when unpausing
        if (unpausing)
        {
            float timeRemaining = gameManager.pauseWaitTime;
            gameObject.GetComponentInChildren<Text>().text = $"Unpausing in:\n{timeRemaining:0.00}";
            if (timeRemaining == 0)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
