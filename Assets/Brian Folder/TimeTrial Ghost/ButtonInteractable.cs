using UnityEngine;

public class ButtonInteractable : Interactable
{
    public bool isPressed { get; private set; }
    public float pressedYOffset = 0.1f;
    private Vector3 originalPosition;
    private Vector3 pressedPosition;

    private void Start()
    {
        base.Awake ();
        originalPosition = transform.position;
        pressedPosition = originalPosition - new Vector3(0, pressedYOffset, 0);
        promptMessage = "Press E to activate button";
    }
    
    public override void BaseInteract()
    {
        if (!hasBeenInteracted) {
            PressButton();
        }
    }

    public void PressButton()
    {
        isPressed = true;
        transform.position = pressedPosition;
        hasBeenInteracted = true;

        Debug.Log("Button Pressed");
    }

    public override void ResetToOriginalPosition()
    {
        base.ResetToOriginalPosition();
        isPressed = false;
        transform.position = originalPosition;
    }
}
