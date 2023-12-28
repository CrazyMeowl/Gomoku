using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class MainMenu : MonoBehaviour
{
    public Slider size_slider;
    public TMP_Text size_text;

    void Start()
    {
        if ((int)PlayerPrefs.GetInt("BoardSize") == 0)
        {
            // Set the default size of the board to 15.
            PlayerPrefs.SetInt("BoardSize", 15);
        }
        // Set the slider value to the saved value
        size_slider.value = (int)PlayerPrefs.GetInt("BoardSize");
        // Set the text of the size setting to the slider value.
        size_text.text = $"Board Size: {size_slider.value.ToString()}";
        // Add the listener to the size slider to change the text when the size slider changes.
        size_slider.onValueChanged.AddListener((v) =>
        {
            size_text.text = $"Board Size: {v.ToString()}";
        });
    }
    public void PlayGame()
    {
        // Get the value of the slider and apply it to setting
        PlayerPrefs.SetInt("BoardSize", (int)size_slider.value);
        // Load up the game scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void Quit()
    {
        Debug.Log("Quit");
        // Close the application.
        Application.Quit();
    }
}
