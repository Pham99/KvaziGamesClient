using UnityEngine;

public static class PLayerColours
{
    public static Color GetColour(int id)
    {
        switch (id)
        {
            case 1:
                return Color.red;
            case 2:
                return Color.blue;
            case 3:
                return Color.green;
            case 4:
                return Color.yellow;
            case 5:
                return new Color(0.5f, 0.0f, 1.0f); // Purple
            case 6:
                return new Color(1.0f, 0.0f, 1.0f); // Magenta
            case 7:
                return new Color(0.0f, 1.0f, 1.0f); // Cyan
            case 8:
                return new Color(1.0f, 0.75f, 0.0f); // Orange
            default:
                return Color.white; // Default color for unknown IDs
        }
    }
}