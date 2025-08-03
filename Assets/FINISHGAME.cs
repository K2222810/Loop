using TMPro;
using UnityEngine;

public class FINISHGAME : Interactable
{
    public GameObject finishGameText;
    public TextMeshProUGUI messageText;


    protected override void Interact()
    {
        EndGame();
    }
    public void EndGame()
    {
        finishGameText.SetActive(true);
        messageText.text = "WELL DONE YOU SAVED THE NUCLEAR " +
            "POWER PLANT ";
        Time.timeScale = 0f;

    }
}
