using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Kasumbal/ColorPaletteGenerator")]
public class ColorPaletteGenerator : ScriptableObject
{
    //  LIST of Colors to make a palette
    public ColorPalette[] colorPalettes;

    public Color[] GetColor(int i)
    {
        var proceduralColors = colorPalettes[i].inputColors;
        var length = proceduralColors.Length;
        Color[] colors = new Color[length];
        for (int j = 0; j < length; j++)
        {
            colors[j] = proceduralColors[j].color;
        }
        return colors;
    }
}

[System.Serializable]
public class ColorPalette
{
    public ProceduralColor[] inputColors;
}

[System.Serializable]
public class ProceduralColor
{
    public Color color;
    //public float minSaturation = 0f;
    //public float maxSaturation = 1f;
    //public float minBrightness = 0f;
    //public float maxBrightness = 1f;
}
