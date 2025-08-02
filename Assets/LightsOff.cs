using UnityEngine;
using UnityEngine.InputSystem;

public class LightsOff : Interactable
{
    public GameObject RedLights;
    public GameObject GeneralLights;

    public bool redlight1 = false;
    public GameObject notification;

    public bool firstaskdone = false;

    public Color lightAmbientColor =new 
    Color(255f/255f, 255f/255f,255f/255f);

    public Color darkAmbientColor = new
    Color(0f / 255f, 0f / 255f, 0f / 255f);
    void Update()
    {
    }
    protected override void Interact()
    {
        if (!redlight1)
        {
            Redlights();
        }
        else
        {
            NormalLights();

        }

    }

    public void Redlights()
    {
            RenderSettings.ambientLight = darkAmbientColor;
            GeneralLights.SetActive(false);
            RedLights.SetActive(true);
            firstaskdone = true;
            redlight1 = true;
    }
    public void NormalLights()
    {
            RenderSettings.ambientLight = lightAmbientColor;
            GeneralLights.SetActive(true);
            RedLights.SetActive(false);
            firstaskdone = false;
            redlight1 = false;
    }
}
