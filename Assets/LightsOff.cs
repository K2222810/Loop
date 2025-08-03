using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class LightsOff : Interactable
{
    public GameObject RedLights;
    public GameObject GeneralLights;

    public bool redlight1 = false;
    public TextMeshProUGUI notifaction;

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
        notifaction.text = "NEW MESSAGE " +
            "IN THE MAIN ROOM ";

        RenderSettings.ambientLight = darkAmbientColor;
            GeneralLights.SetActive(false);
/*            RedLights.SetActive(true);
*/            firstaskdone = true;
            redlight1 = true;
    }
    public void NormalLights()
    {
        notifaction.text = "Turn Off Power Supplies";
        RenderSettings.ambientLight = lightAmbientColor;
            GeneralLights.SetActive(true);
/*            RedLights.SetActive(false);
*/            firstaskdone = false;
            redlight1 = false;
    }
}
