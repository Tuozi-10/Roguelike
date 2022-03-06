using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    [SerializeField] private List<AudioClip> m_clips;
    [SerializeField] private AudioSource m_source;
    
    public static AudioManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            DestroyImmediate(gameObject);
            return;
        }

        instance = this;
    }


    public enum sounds
    {
        play = 0,
        hit = 1,
        BossEntrance = 2,
        goreWet = 3,
        shoot = 4
    }

    public void PlaySound(sounds sound, float volume = 0.35f)
    {
        m_source.PlayOneShot(m_clips[(int)sound], volume);
    }
    
}