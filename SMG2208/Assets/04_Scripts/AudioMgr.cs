using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMgr : SingletonMono<AudioMgr>
{
    public List<AudioClip> clipList = new List<AudioClip>();

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayOneShot(int clipIndex)
    {
        audioSource.PlayOneShot(clipList[clipIndex]);
    }

    public void Play(int clipIndex)
    {
        audioSource.Stop();
        audioSource.clip = clipList[clipIndex];
        audioSource.Play();
    }

    public void StopPlay()
    {
        audioSource.Stop();
    }
}
