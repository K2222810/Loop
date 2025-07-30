using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{

    //Reference the playerInput Script
    private InputSystem_Actions playerInput;
    //A reference to the input action map,so the program reads the input values
    public InputSystem_Actions.OnFootActions onFoot;

    private PlayerLook Look;
    private PlayerMovement Move;
    private Vector2 movementInput;



    // Start is called before the first frame update
    void Awake()
    {   //A new instance player class

        playerInput = new InputSystem_Actions();
        onFoot = playerInput.OnFoot;

        Look = GetComponent<PlayerLook>();
        Move = GetComponent<PlayerMovement>();
        //This part of the code We're using is a callback context
        //or ctx to call our motor.jump function and Shoot
        onFoot.Jump.performed += ctx => Move.jump();
        onFoot.Crouch.performed += ctx => Move.Crouch();
        onFoot.Sprint.performed += ctx => Move.Sprint();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!PauseMenu.isPaused)
        {
            //tells the player movement to move,using the movement value from our movement action
            Move.ProcessMove(onFoot.Movement.ReadValue<Vector2>());
        }
    }

    private void LateUpdate()
    {
        if (!PauseMenu.isPaused)
        {
            //tells the player movement to move,using the movement value from our movement action
            Look.ProcessLook(onFoot.Look.ReadValue<Vector2>());
        }
    }

    //enable and disable our action map
    private void OnEnable()
    {

        onFoot.Enable();

    }
    private void OnDisable()
    {
        onFoot.Disable();
    }

}
