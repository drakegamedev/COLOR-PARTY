using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [System.Serializable]
    public struct Sound
    {
        public string name;

        public AudioClip clip;

        [Range(0f, 1f)] public float volume;
        [Range(.1f, 3f)] public float pitch;

        public bool loop;

        [HideInInspector]
        public AudioSource source;
    }

    public Sound[] Sounds;

    #region Singleton
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        for (int i = 0; i < Sounds.Length; i++)
        {
            Sounds[i].source = gameObject.AddComponent<AudioSource>();
            Sounds[i].source.clip = Sounds[i].clip;

            Sounds[i].source.volume = Sounds[i].volume;
            Sounds[i].source.pitch = Sounds[i].pitch;
            Sounds[i].source.loop = Sounds[i].loop;
        }
    }
    #endregion

    // Play Audio
    public void Play(string name)
    {
        for (int i = 0; i < Sounds.Length; i++)
        {
            Sounds[i] = Array.Find(Sounds, sound => sound.name == name);

            if (Sounds[i].name == null)
            {
                Debug.LogWarning("Sound: " + name + " not found");
                return;
            }

            Sounds[i].source.Play();

            break;
        }
    }

    // Stop Audio
    public void Stop(string name)
    {
        for (int i = 0; i < Sounds.Length; i++)
        {
            Sounds[i] = Array.Find(Sounds, sound => sound.name == name);

            if (Sounds[i].name == null)
            {
                Debug.LogWarning("Sound: " + name + " not found");
                return;
            }

            Sounds[i].source.Stop();

            break;
        }
    }

    // Modify Audio Pitch
    public void ModifyPitch(string name, float amount)
    {
        for (int i = 0; i < Sounds.Length; i++)
        {
            Sounds[i] = Array.Find(Sounds, sound => sound.name == name);

            if (Sounds[i].name == null)
            {
                Debug.LogWarning("Sound: " + name + " not found");
                return;
            }

            Sounds[i].pitch = amount;

            Sounds[i].source.pitch = Sounds[i].pitch;

            break;
        }
    }
}
