using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Apple;
using UnityEngine.SceneManagement;

public class KeyPadMoveChangeScene : Interactable
{
    public int Number;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    protected override void Interact()
    {
        respawn();
    }

    void respawn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + Number);

    }

} 