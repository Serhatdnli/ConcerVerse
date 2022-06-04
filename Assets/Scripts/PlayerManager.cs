using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int id;
    public string username;
    private AudioSource danceAudio;

    private void Start()
    {
        danceAudio = GetComponent<AudioSource>();
    }

    public void DanceAudioStart()
    {
        danceAudio.Play();
        print("Ses Çalındı");
    }
}
