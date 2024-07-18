using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager: MonoBehaviour 
{
    public AudioSource audioSource;

    public float pitchIncrement = 1f;
    public void PlayPop()
    {
            if (audioSource != null)
            {
                audioSource.pitch = pitchIncrement;
                audioSource.Play();
            }
    }
}