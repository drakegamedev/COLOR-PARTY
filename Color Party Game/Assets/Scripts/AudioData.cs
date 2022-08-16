using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Audio", menuName = "Audio")]
public class AudioData : ScriptableObject
{
    // Indicates Type of Audio
    public enum AudioType
    {
        BGM,
        SFX
    };

    public string Id { get; set; }
    public AudioClip Clip;
    public AudioType Type;

    [Range(0, 256)] public int Priority;
    [Range(0f, 1f)] public float Volume;
    [Range(0f, 3f)] public float Pitch;
    public bool Loop;
    [Range(0f, 3f)] public float SpatialBlend;

    // Audio Source
    public AudioSource Source { get; private set; }

    public bool IsInitialized => Source;

    // Initialize Audio Data Properties
    public void Initialize(GameObject audioSourceContainer)
    {
        Source = audioSourceContainer.AddComponent<AudioSource>();
        Source.clip = Clip;
        Source.priority = Priority;
        Source.volume = Volume;
        Source.pitch = Pitch;
        Source.loop = Loop;
        Source.spatialBlend = SpatialBlend;
    }
}
