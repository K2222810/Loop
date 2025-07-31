using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectedCube : Interactable
{
    public GameObject particle;

    // Start is called before the first frame update
    protected override void Interact()
    {
        Destroy(gameObject);
        Instantiate(particle, transform.position, Quaternion.identity);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
