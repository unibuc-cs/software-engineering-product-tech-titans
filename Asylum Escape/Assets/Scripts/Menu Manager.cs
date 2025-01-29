using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{

    public GameObject mainMenu = null;
    public GameObject setingsMenu = null;


    public void setingsMenuStart()
    {
        mainMenu.gameObject.SetActive(false);
        setingsMenu.gameObject.SetActive(true);
    }

    public void back()
    {
        setingsMenu.gameObject.SetActive(false);
        mainMenu.gameObject.SetActive(true);
    }

    public void Close()
    {
        Application.Quit();
    }

    public void Game()
    {
        mainMenu.SetActive(false);
    }
}
