using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GhostReplayer : MonoBehaviour
{
    public List<GhostFrame> replayData;
    public float playbackSpeed = 1f;

    private void Start()
    {
        StartCoroutine(ReplayMovement());
    }

    IEnumerator ReplayMovement()
    {
        foreach(GhostFrame frame in replayData) {
            transform.position = frame.position;
            transform.rotation = frame.rotation;
            yield return 0;//new WaitForSeconds(Time.deltaTime / playbackSpeed);
        }

    }

}
