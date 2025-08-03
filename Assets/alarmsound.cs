using UnityEngine;

public class alarmsound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip loopClip;
    public AudioClip loopClip2;


    void Start()
    {
        audioSource.clip = loopClip;
        audioSource.loop = true;
        audioSource.Play();
    }
}
