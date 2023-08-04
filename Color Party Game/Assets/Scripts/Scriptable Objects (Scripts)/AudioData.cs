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

    [SerializeField] private string id;                                                 // Audio ID
    [SerializeField] private AudioClip clip;                                            // Audio Clip
    [SerializeField] private AudioType type;                                            // Type of Audio

    [Range(0, 256)] [SerializeField] private int priority;                              // Audio Priority
    [Range(0f, 1f)] [SerializeField] private float volume;                              // Audio Volume
    [Range(0f, 3f)] [SerializeField] private float pitch;                               // Audio Pitch
    [SerializeField] private bool playOnAwake;                                          // Checks if Audio Should Play On Start-up
    [SerializeField] private bool loop;                                                 // Check if Audio Should Play On Repeat
    [Range(0f, 3f)] [SerializeField] private float spatialBlend;                        // Audio Spatial Blend


    public AudioSource Source { get; private set; }                                     // Audio Source Component

    public bool IsInitialized => Source;

    /// <summary>
    /// Initialize Audio Data Properties
    /// </summary>
    /// <param name="audioSourceContainer"></param>
    public void Initialize(GameObject audioSourceContainer)
    {
        Source = audioSourceContainer.AddComponent<AudioSource>();
        Source.clip = clip;
        Source.priority = priority;
        Source.volume = volume;
        Source.pitch = pitch;
        Source.playOnAwake = playOnAwake;
        Source.loop = loop;
        Source.spatialBlend = spatialBlend;
    }

    /// <summary>
    /// Audio ID Getter
    /// </summary>
    /// <returns></returns>
    public string GetId() { return id; }

    /// <summary>
    /// Audio Type Getter
    /// </summary>
    /// <returns></returns>
    public AudioType GetAudioType() { return type; }
}
