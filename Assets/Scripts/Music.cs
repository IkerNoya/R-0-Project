using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    [SerializeField] AudioClip songMenu;
    [SerializeField] AudioClip songFight;
    [SerializeField] AudioSource source;

    public void StartMusicMenu() {
        source.Stop();
        source.clip = songMenu;
        source.Play();
    }
    public void StartMusicGameplay() {
        source.Stop();
        source.clip = songFight;
        source.Play();
    }

}
