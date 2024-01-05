using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionController : MonoBehaviour
{

    public GameObject window_button;

    public GameObject fullscreen_button;
    // Start is called before the first frame update
    void Start()
    {

        Debug.Log("Screen Mode: " + Screen.fullScreenMode);
        if (Screen.fullScreenMode == FullScreenMode.Windowed)
        {
            fullscreen_button.SetActive(true);
            window_button.SetActive(false);
        }
        else
        {
            fullscreen_button.SetActive(false);
            window_button.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ScreenMode(string input_screen_mode)
    {
        Debug.Log("Screen Mode: " + input_screen_mode);

        if (input_screen_mode == "window")
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;

        }
        if (input_screen_mode == "full")
        {
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;


        }

    }
}
