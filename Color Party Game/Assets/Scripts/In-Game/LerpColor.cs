using System.Collections;
using UnityEngine;

public class LerpColor : MonoBehaviour
{
    public Color[] MyColors { get; set; }                   // Array of Colors to lerp
    public float LerpTime { get; set; }                     // Time of Lerping

    private SpriteRenderer spriteRenderer;                  // Sprite Renderer Reference
    private int colorIndex = 0;                             // Current Color Index

    void OnEnable()
    {
        EventManager.Instance.Intensify += StartLerping;
    }

    void OnDisable()
    {
        EventManager.Instance.Intensify -= StartLerping;
    }

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Inititate Lerping
    /// </summary>
    public void StartLerping()
    {
        StartCoroutine(ColorLerping(MyColors[colorIndex], LerpTime));
    }

    /// <summary>
    /// Lerping of Colors
    /// </summary>
    /// <param name="endColor"></param>
    /// <param name="duration"></param>
    /// <returns></returns>
    IEnumerator ColorLerping(Color endColor, float duration)
    {
        float time = 0;
        Color startColor = spriteRenderer.color;

        // Keep on lerping until end color has been reached
        while (time < duration)
        {
            spriteRenderer.color = Color.Lerp(startColor, endColor, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        colorIndex++;
        spriteRenderer.color = endColor;

        // Reset Color Index when color array reached last index
        if (colorIndex >= MyColors.Length)
        {
            colorIndex = 0;
        }

        yield return new WaitForSeconds(1f);

        // Repeat Lerp Cycle
        StartCoroutine(ColorLerping(MyColors[colorIndex], LerpTime));
    }
}
