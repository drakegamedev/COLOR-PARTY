using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpColor : MonoBehaviour
{
    public Color[] MyColors;                                // Array of Colors to lerp
    public float LerpTime { get; set; }                     // Time of Lerping

    private SpriteRenderer spriteRenderer;                  // Sprite Renderer Reference
    private int colorIndex = 0;                             // Current Color Index

    private void OnEnable()
    {
        EventManager.Instance.Intensify += StartLerping;
    }

    private void OnDisable()
    {
        EventManager.Instance.Intensify -= StartLerping;
    }

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Inititate Lerping
    public void StartLerping()
    {
        StartCoroutine(ColorLerping(MyColors[colorIndex], LerpTime));
    }

    // Lerping of Colors
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

        // Repeat Lerp Cycle
        StartCoroutine(ColorLerping(MyColors[colorIndex], LerpTime));
    }
}
