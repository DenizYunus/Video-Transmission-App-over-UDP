using UnityEngine;
using UnityEngine.UI;

public static class ImageProcesses
{
    public static void FlipTextureVertically(Texture2D original)
    {
        var originalPixels = original.GetPixels();

        var newPixels = new Color[originalPixels.Length];

        var width = original.width;
        var rows = original.height;

        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < rows; y++)
            {
                newPixels[x + y * width] = originalPixels[x + (rows - y - 1) * width];
            }
        }

        original.SetPixels(newPixels);
        original.Apply();
    }

    public static void FlipTextureHorizontally(Texture2D original)
    {
        var originalPixels = original.GetPixels();

        var newPixels = new Color[originalPixels.Length];

        var width = original.width;
        var rows = original.height;

        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < rows; y++)
            {
                newPixels[x + y * width] = originalPixels[(width - x - 1) + y * width];
            }
        }

        original.SetPixels(newPixels);
        original.Apply();
    }

    public static Texture2D FitToRawImage(this Texture2D texture, RawImage rawImage)
    {
        if ((texture.width / rawImage.rectTransform.rect.width) < (texture.height / rawImage.rectTransform.rect.height))
        {
            texture.Reinitialize((int)rawImage.rectTransform.rect.width, (int)(texture.height * (rawImage.rectTransform.rect.width / texture.width)));
        } else
        {
            texture.Reinitialize((int)(texture.width * (rawImage.rectTransform.rect.height / texture.height)), (int)rawImage.rectTransform.rect.height);
        }
        return texture;
    }
}