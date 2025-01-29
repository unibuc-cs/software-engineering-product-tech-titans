
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject PauseScreen;
    public GameObject EscapedScreen;
    public GameObject DiedScreen;
    public GameObject ui_inventory = null;
    private bool isPaused = false;
    public bool isInSettings = false;

    void Start()
    {
        PauseScreen.SetActive(false);
        Time.timeScale = 1f; 
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !EscapedScreen.activeSelf && !DiedScreen.activeSelf && !isInSettings) 
        {
            if (isPaused)
            {
                ResumeGame(); 
            }
            else
            {
                PauseGame(); 
            }
        }
    }

    public void PauseGame()
    {
        PauseScreen.SetActive(true); 
        Time.timeScale = 0f;        
        isPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        ui_inventory.gameObject.SetActive(false);
    }

    public void ResumeGame()
    {
        PauseScreen.SetActive(false); 
        Time.timeScale = 1f;         
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        ui_inventory.gameObject.SetActive(true);
    }

    public void QuitGame() {
        PauseScreen.SetActive(false);
        SceneManager.LoadSceneAsync(0);
    }

    public void Settings()
    {
        isInSettings = !isInSettings;
    }



}
