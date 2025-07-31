using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Apple;
using UnityEngine.SceneManagement;

public class KeyPadMove: Interactable
{
    public Transform destination, player;
    public GameObject Player;

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
            Player.SetActive(false);
            player.position = destination.position;
            Player.SetActive(true);
    }

} 