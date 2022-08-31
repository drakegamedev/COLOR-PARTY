using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    // Collects AudioData
    [System.Serializable]
    public struct AudioDataCollection
    {
        //public string Id;
        public AudioData[] Audios;
    }

    [Header("References")]
    public AudioDataCollection[] AudioCollections;

    [Space]
    public GameObject MusicHolder;
    public GameObject SoundHolder;

    #region Singleton
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        // Initialize Audio Collections
        foreach (AudioDataCollection collectionData in AudioCollections)
        {
            foreach (AudioData audioData in collectionData.Audios)
            {
                switch (audioData.Type)
                {
                    case AudioData.AudioType.BGM:
                        audioData.Initialize(MusicHolder);
                        break;

                    case AudioData.AudioType.SFX:
                        audioData.Initialize(SoundHolder);
                        break;
                };
            }
        }
    }
    #endregion

    #region Audio Methods
    // Play Audio
    public void Play(string id)
    {
        // Find Audio in Audio Collections
        foreach (AudioDataCollection collectionData in AudioCollections)
        {
            // Check within AudioData Types
            foreach (AudioData audioData in collectionData.Audios)
            {
                // Audio Found
                if (id == audioData.Id)
                {
                    audioData.Source.Play();
                    return;
                }
            }
        }

        // Audio Not Found
        Debug.LogWarning("Audio " + id + " cannot be found!");
        return;
    }

    // Stop Audio
    public void Stop(string id)
    {
        // Find Audio in Audio Collections
        foreach (AudioDataCollection collectionData in AudioCollections)
        {
            // Check within AudioData Types
            foreach (AudioData audioData in collectionData.Audios)
            {
                // Audio Found
                if (id == audioData.Id)
                {
                    audioData.Source.Stop();
                    return;
                }
            }
        }

        // Audio Not Found
        Debug.LogWarning("Audio " + id + " cannot be found!");
        return;
    }

    // Modify Audio Pitch
    public void ModifyPitch(string id, float amount)
    {
        // Find Audio in Audio Collections
        foreach (AudioDataCollection collectionData in AudioCollections)
        {
            // Check within AudioData Types
            foreach (AudioData audioData in collectionData.Audios)
            {
                // Audio Found
                if (id == audioData.Id)
                {
                    audioData.Source.pitch = amount;
                    return;
                }
            }
        }

        // Audio Not Found
        Debug.LogWarning("Audio " + id + " cannot be found!");
        return;
    }
    #endregion
}
