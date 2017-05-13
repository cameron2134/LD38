using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

    public GameObject controlsPanel, menuPanel;

	public void StartGame() {
        menuPanel.SetActive(false);
        controlsPanel.SetActive(true);
    }

    public void Play() {
        SceneManager.LoadScene("Main");
    }

    


    public void QuitGame() {
        Application.Quit();
    }
}
