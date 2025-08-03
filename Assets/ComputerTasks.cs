using TMPro;
using UnityEngine;

public class ComputerTasks : Interactable
{
    [Header("COMPUTER")]
    public GameObject ComputerCanvas;
    public GameObject firstTask;
    public GameObject secondTask;
    public GameObject thirdTask;

    [Header("ROOM 1")]
    //ROOM 1
    public GameObject Room11;
    public GameObject Room12;
    public GameObject Room13;

    [Header("ROOM 2")]
    // ROOM 2
    public GameObject Room21;
    public GameObject Room22;

    [Header("MENU")]
    // TASK MENU
    public GameObject TaskCanvas;

    public TextMeshProUGUI messageText;

    public bool ComputerOn = false;
    public LightsOff lightsoff;
    public Oxigenoff oxigenoff;

    public void Start()
    {
        lightsoff = FindAnyObjectByType<LightsOff>();
    }
    protected override void Interact()
    {
        if (!ComputerOn)
        {
            turnoncomputer();
        }
        else
        {
            turnoffcomputer();
        }
    }
    public void turnoncomputer()
    {
        if(!lightsoff.firstaskdone)
        {
            ComputerCanvas.SetActive(true);
            firstTask.SetActive(true);
            TaskCanvas.SetActive(true);
            messageText.text = "Turn Off Power Supplies";
            ComputerOn = true;
        }
        if (lightsoff.firstaskdone)
        {
            ComputerCanvas.SetActive(true);
            secondTask.SetActive(true);
            //TASK TEXT
            messageText.text = "Reduce unnecessary oxygen consumption";

            //ROOM 1
            Room11.SetActive(false);
            Room12.SetActive(true);
            // COMPUTER ON 
            ComputerOn = true;
        }
        if (lightsoff.firstaskdone && oxigenoff.oxygen)
        {
            ComputerCanvas.SetActive(true);
            secondTask.SetActive(false);
            thirdTask.SetActive(true);
            //TASK TEXT
            messageText.text = "Reduce unnecessary water consumption";

            //ROOM 1 
            Room12.SetActive(false);
            Room13.SetActive(true);
            //ROOM 2
            Room21.SetActive(false);
            Room22.SetActive(true);
            // COMPUTER ON 
            ComputerOn = true;
        }
    }
    public void turnoffcomputer()
    {
        if (!lightsoff.firstaskdone)
        {
            ComputerCanvas.SetActive(false);
            firstTask.SetActive(false);
            ComputerOn = false;
        }
        if (lightsoff.firstaskdone)
        {
            ComputerCanvas.SetActive(false);
            secondTask.SetActive(false);
            ComputerOn = false;
        }
/*        if(lightsoff.firstaskdone && oxigenoff.oxygen) 
        {
            ComputerCanvas.SetActive(false);
            thirdTask.SetActive(false);
            ComputerOn = false;
        }*/
    }

}
