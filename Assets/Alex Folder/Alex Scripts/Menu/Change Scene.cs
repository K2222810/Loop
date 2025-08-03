using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;

    public void ActivateMainMenu(bool state)
    {
        mainMenu.SetActive(state);

    }

    public void play()
    {
        SceneManager.LoadScene("Map");
    }
    public void Quit()
    {
        Application.Quit();
    }

}
