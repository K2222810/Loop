using TMPro;
using UnityEngine;

public class Oxigenoff : Interactable
{
    public bool oxygen = false;
    public TextMeshProUGUI notifaction;
    
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
        notifaction.text = "NEW MESSAGE " +
      "IN THE MAIN ROOM ";
        oxygen = true;
    }
    public void OxygenOff()
    {
        notifaction.text = "Reduce unnecessary oxygen consumption";
        oxygen = false;
    }
    void Update()
    {
        
    }
}
