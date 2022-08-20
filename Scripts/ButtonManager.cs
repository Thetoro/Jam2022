using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public void StartButton()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void ControlButton()
    {
        SceneManager.LoadScene("Controls");
    }
    public void CreditsButton()
    {
        SceneManager.LoadScene("Credits");
    }

    public void MenuButton()
    {
        SceneManager.LoadScene("Menu");
    }
}
