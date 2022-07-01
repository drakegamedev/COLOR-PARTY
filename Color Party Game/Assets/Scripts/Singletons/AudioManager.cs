using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    
    [System.Serializable]
    public struct AudioData
    {
        public string Id;
        public AudioClip Clip;

        [Range(0, 256)] public int Priority;
        [Range(0f, 1f)] public float Volume;
        [Range(0f, 3f)] public float Pitch;
        public bool Loop;
        [Range(0f, 3f)] public float SpatialBlend;

        [HideInInspector]
        public AudioSource Source;
    }

    [Header("References")]
    public AudioData[] Audio;

    #region Singleton
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }

        for (int i = 0; i < Audio.Length; i++)
        {
            Audio[i].Source = gameObject.AddComponent<AudioSource>();
            Audio[i].Source.clip = Audio[i].Clip;
            
            Audio[i].Source.priority = Audio[i].Priority;
            Audio[i].Source.volume = Audio[i].Volume;
            Audio[i].Source.pitch = Audio[i].Pitch;
            Audio[i].Source.loop = Audio[i].Loop;
            Audio[i].Source.spatialBlend = Audio[i].SpatialBlend;
        }
    }
    #endregion

    // Play Audio
    public void Play(string id)
    {
        for (int i = 0; i < Audio.Length; i++)
        {
            if (id == Audio[i].Id)
            {
                Audio[i].Source.Play();
                break;
            }
        }
    }

    // Stop Audio
    public void Stop(string id)
    {
        for (int i = 0; i < Audio.Length; i++)
        {
            if (id == Audio[i].Id)
            {
                Audio[i].Source.Stop();
                break;
            }
        }
    }

    // Modify Audio Pitch
    public void ModifyPitch(string id, float amount)
    {
        for (int i = 0; i < Audio.Length; i++)
        {
            if (id == Audio[i].Id)
            {
                Audio[i].Pitch = amount;
                Audio[i].Source.pitch = Audio[i].Pitch;
                break;
            }
        }
    }
}
