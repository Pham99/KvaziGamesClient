using UnityEngine;

public class RandomColour
{
    public Color GetRandomHueColor()
    {
        float hue = Random.Range(0f, 1f); // Random hue (0-1)
        float saturation = 1f; // Full saturation
        float value = 1f; // Full brightness

        return Color.HSVToRGB(hue, saturation, value);
    }
}
