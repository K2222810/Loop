using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
using Random = UnityEngine.Random;

public class PlayerMovement : MonoBehaviour
{   //this script contains all our movement funcionality
    private Animator animator;
    private CharacterController Controler;
    private Vector3 PlayerVelocity;
    private bool isGrounded;
    bool crouching = false;
    bool lerpCrouch = false;
    bool sprinting = false;

    float crouchTimer = 1;

    [SerializeField]
    public float Velocity = 5f;
    public float gravity = -9.8f;
    public float jumpHeight = 3f;



    // Start is called before the first frame update
    void Start()
    {
        Controler = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Controler.isGrounded;
        if (isGrounded && animator.GetBool("isJumping"))
        {
            animator.SetBool("isJumping", false);
        }

        if (lerpCrouch)
        {
            crouchTimer += Time.deltaTime;
            float p = crouchTimer / 1;
            p *= p;

            if (crouching)
            {
                Controler.height = Mathf.Lerp(Controler.height, 1, p);
            }
            else
            {
                Controler.height = Mathf.Lerp(Controler.height, 2, p);
            }
            if (p > 1)
            {
                lerpCrouch = false;
                crouchTimer = -0f;
            }
        }
    }


    public void ProcessMove(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;

        bool isWalking = input.magnitude > 0.1f;
        animator.SetBool("isWalking", isWalking);

        Controler.Move(transform.TransformDirection(moveDirection) * Velocity * Time.deltaTime);
        PlayerVelocity.y += gravity * Time.deltaTime;
        if (isGrounded && PlayerVelocity.y < 0)
        {
            PlayerVelocity.y = -2f;
            animator.SetBool("isJumping", false);
        }
        else
        {
            animator.SetBool("isJumping", true);
        }



            Controler.Move(PlayerVelocity * Time.deltaTime);
    }

    public void jump()
    {
        //makes the jump 
        if (isGrounded)
        { 
            PlayerVelocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravity);
        }
    }

    public void Sprint()
    {
        sprinting = !sprinting;
        if (sprinting)
            Velocity = 15;
        else
            Velocity = 5;
    }

    public void Crouch()
    {
        crouching = !crouching;
        crouchTimer = 0;
        lerpCrouch = true;
    }


}
