using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpColor : MonoBehaviour
{
    public SpriteRenderer spriteSquare;

    public Color[] myColors;
    public float lerpTime;

    int colorIndex = 0;
    public float t = 0f;

    // Start is called before the first frame update
    void Start()
    {
        spriteSquare = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        spriteSquare.color = Color.Lerp(spriteSquare.color, myColors[colorIndex], lerpTime * Time.deltaTime);

        t = Mathf.Lerp(t, 1f, lerpTime * Time.deltaTime);

        if (t > .9f)
        {
            t = 0f;
            colorIndex++;

            if (colorIndex >= myColors.Length)
            {
                colorIndex = 0;
            }
        }
    }
}
