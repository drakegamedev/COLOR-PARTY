using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpColor : MonoBehaviour
{
    public Color[] MyColors;
    public float LerpTime { get { return lerpTime; } set { lerpTime = value; } }

    private SpriteRenderer spriteRenderer;
    private int colorIndex = 0;
    private float lerpTime;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        StartCoroutine(LerpFunction(MyColors[colorIndex], lerpTime));
    }

    IEnumerator LerpFunction(Color endValue, float duration)
    {
        float time = 0;
        Color startValue = spriteRenderer.color;

        while (time < duration)
        {
            spriteRenderer.color = Color.Lerp(startValue, endValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        colorIndex++;
        spriteRenderer.color = endValue;

        // Reset Color Index
        if (colorIndex >= MyColors.Length)
        {
            colorIndex = 0;
        }

        // Repeat Lerp Cycle
        StartCoroutine(LerpFunction(MyColors[colorIndex], lerpTime));
    }
}
