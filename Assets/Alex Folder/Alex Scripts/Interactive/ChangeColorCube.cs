using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColorCube : Interactable
{
    MeshRenderer mesh;
    public Color[] colors;
    private int coloutIndex;

    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<MeshRenderer>();
        mesh.material.color = Color.red;  
    }

    // Update is called once per frame
    protected override void Interact()
    {
        coloutIndex++;
        if (coloutIndex > colors.Length - 1)
        { 
            coloutIndex = 0;    
        
        }

        mesh.material.color = colors[coloutIndex];


        Debug.Log("Hello World");


    }
}
