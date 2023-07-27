using UnityEngine;

// Game Audio System
// Manages Music and Sound Effects
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    // Collects AudioData
    [System.Serializable]
    public struct AudioDataCollection
    {
        public AudioData[] Audios;
    }

    [Header("References")]
    [SerializeField] private AudioDataCollection[] audioCollections;                            // Audio Collection Array

    [Space]
    [SerializeField] private GameObject musicHolder;                                            // BGM Holder Reference
    [SerializeField] private GameObject soundHolder;                                            // SFX Holder Reference

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
            return;
        }

        // Initialize Audio Collections
        foreach (AudioDataCollection collectionData in audioCollections)
        {
            foreach (AudioData audioData in collectionData.Audios)
            {
                switch (audioData.GetAudioType())
                {
                    case AudioData.AudioType.BGM:
                        audioData.Initialize(musicHolder);
                        break;

                    case AudioData.AudioType.SFX:
                        audioData.Initialize(soundHolder);
                        break;
                };
            }
        }
    }
    #endregion

    #region Audio Methods
    /// <summary>
    /// Play Audio
    /// </summary>
    /// <param name="id"></param>
    public void Play(string id)
    {
        // Find Audio in Audio Collections
        foreach (AudioDataCollection collectionData in audioCollections)
        {
            // Check within AudioData Types
            foreach (AudioData audioData in collectionData.Audios)
            {
                // Audio Found
                if (id == audioData.GetId())
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

    /// <summary>
    /// Stop Audio
    /// </summary>
    /// <param name="id"></param>
    public void Stop(string id)
    {
        // Find Audio in Audio Collections
        foreach (AudioDataCollection collectionData in audioCollections)
        {
            // Check within AudioData Types
            foreach (AudioData audioData in collectionData.Audios)
            {
                // Audio Found
                if (id == audioData.GetId())
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

    /// <summary>
    /// Modify Audio Pitch
    /// </summary>
    /// <param name="id"></param>
    /// <param name="amount"></param>
    public void ModifyPitch(string id, float amount)
    {
        // Find Audio in Audio Collections
        foreach (AudioDataCollection collectionData in audioCollections)
        {
            // Check within AudioData Types
            foreach (AudioData audioData in collectionData.Audios)
            {
                // Audio Found
                if (id == audioData.GetId())
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
