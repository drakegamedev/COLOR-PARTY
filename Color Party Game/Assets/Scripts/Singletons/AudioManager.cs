using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    
    [System.Serializable]
    // Collects AudioData
    public struct AudioDataCollection
    {
        public string Id;
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
        for (int i = 0; i < AudioCollections.Length; i++)
        {
            AudioData audioData = AudioCollections[i].Audios[i];

            // Set Audio Data ID
            audioData.Id = AudioCollections[i].Id;

            // Check if Audio is BGM or SFX,
            // then put audio source in designated container
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
    #endregion

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
                    break;
                }
                // Audio Not Found
                else
                {
                    Debug.LogWarning("Audio " + id + " cannot be found!");
                    return;
                }
            }
        }
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
                    break;
                }
                // Audio Not Found
                else
                {
                    Debug.LogWarning("Audio " + id + " cannot be found!");
                    return;
                }
            }
        }
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
                    break;
                }
                // Audio Not Found
                else
                {
                    Debug.LogWarning("Audio " + id + " cannot be found!");
                    return;
                }
            }
        }
    }
}
