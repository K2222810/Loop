using UnityEngine;

public class ComputerTasks : Interactable
{
    public GameObject ComputerCanvas;
    public GameObject firstTask;
    public GameObject secondTask;
    public GameObject thirdTask;
    //ROOM 1
    public GameObject Room11;
    public GameObject Room12;
    public GameObject Room13;

    // ROOM 2
    public GameObject Room21;
    public GameObject Room22;


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
            ComputerOn = true;
        }
        if (lightsoff.firstaskdone)
        {
            ComputerCanvas.SetActive(true);
            secondTask.SetActive(true);
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
