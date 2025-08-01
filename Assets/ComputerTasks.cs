using UnityEngine;

public class ComputerTasks : Interactable
{
    public GameObject ComputerCanvas;
    public GameObject firstTask;
    public GameObject secondTask;
    public bool ComputerOn = false;
    public LightsOff lightsoff;
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
    }

}
