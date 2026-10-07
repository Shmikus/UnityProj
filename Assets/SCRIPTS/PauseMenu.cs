using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(isPaused)
            {
                Resume();
            }

            else
            {
                Pause();
            }
        }
    }

    void Resume()
    {
        Time.timeScale = 1;
        isPaused = false;
    }
    void Pause()
    {
        Time.timeScale = 0;
        isPaused=true;
    }
}
