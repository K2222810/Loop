using UnityEngine;

public class Oxigenoff : Interactable
{
    public bool oxygen = false;


    protected override void Interact()
    {
        if (!oxygen)
        {
            OxygenOn();
        }
        else
        {
            OxygenOff();
        }
    }
    public void OxygenOn()
    {
        oxygen = true;
    }
    public void OxygenOff()
    {
        oxygen = false;
    }
    void Update()
    {
        
    }
}
