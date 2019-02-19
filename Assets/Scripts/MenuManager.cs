using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour {

    [SerializeField]
    private GameObject pauseMenu;
    [SerializeField]
    private GameObject gameUI, victoryLoseMenu;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Escape) && GameManager.Instance.inGame)
        {
            Pause();            
        }
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        gameUI.SetActive(false);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

        Time.timeScale = 0;      
        if (Camera.main.GetComponent<FPSCamera>()) Camera.main.GetComponent<FPSCamera>().enabled = false;
        if (Camera.main.GetComponent<TPSCamera>()) Camera.main.GetComponent<TPSCamera>().enabled = false;
    }

    public void MainMenu()
    {
        GameManager.Instance.NewGame();
        pauseMenu.SetActive(false);
        victoryLoseMenu.SetActive(false);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
