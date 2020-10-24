using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject camvasMainMenu;
    public GameObject camvasCredits;
    public GameObject camvasControls;
    public GameObject gameplay;
    public Music m;
    private void Start()
    {
        Time.timeScale = 0;
        m.StartMusicMenu();
    }
    private void OnEnable()
    {
        Time.timeScale = 0;
    }
    public void ActivateMainMenu()
    {
        camvasMainMenu.SetActive(true);
        camvasControls.SetActive(false);
        camvasCredits.SetActive(false);
    }
    public void ActivateCredits()
    {
        camvasCredits.SetActive(true);
        camvasControls.SetActive(false);
        camvasMainMenu.SetActive(false);
    }
    public void ActivateControls()
    {
        camvasControls.SetActive(true);
        camvasCredits.SetActive(false);
        camvasMainMenu.SetActive(false);
    }
    public void DisableMenu()
    {
        m.StartMusicGameplay();
        Time.timeScale = 1;
        gameObject.SetActive(false);
        gameplay.SetActive(true);
    }
}
