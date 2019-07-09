
// ----------------------------------------------------------------------------
// The MIT License
// CropScale https://github.com/mopsicus/unity-crop-scale-texture
// Copyright (c) 2019 Mopsicus <mail@mopsicus.ru>
// ----------------------------------------------------------------------------

using UnityEngine;

/// <summary>
/// Crop options to select zone to crop
/// </summary>
public enum CropOptions {
    CENTER,
    BOTTOM_RIGHT,
    TOP_RIGHT,
    BOTTOM_LEFT,
    TOP_LEFT,
    CUSTOM
}

public static class CropScale {

    /// <summary>
    /// Scale teture
    /// ACHTUNG! Load CPU a lot
    /// </summary>
    /// <param name="texture">Source</param>
    /// <param name="width">New width</param>
    /// <param name="height">New height</param>
    /// <returns>Scaled texture</returns>
    public static Texture2D ScaleTexture (Texture2D texture, int width, int height) {
        Color[] sourceColors = texture.GetPixels ();
        Color[] newColors = new Color[width * height];
        float ratioX = 1f / ((float) width / (texture.width - 1));
        float ratioY = 1f / ((float) height / (texture.height - 1));
        for (int i = 0; i < height; i++) {
            int yFloor = (int) Mathf.Floor (i * ratioY);
            int colorY1 = yFloor * texture.width;
            int colorY2 = (yFloor + 1) * texture.width;
            int offset = i * width;
            for (int j = 0; j < width; j++) {
                int xFloor = (int) Mathf.Floor (j * ratioX);
                float xLerp = j * ratioX - xFloor;
                newColors[offset + j] = ColorLerpUnclamped (ColorLerpUnclamped (sourceColors[colorY1 + xFloor], sourceColors[colorY1 + xFloor + 1], xLerp), ColorLerpUnclamped (sourceColors[colorY2 + xFloor], sourceColors[colorY2 + xFloor + 1], xLerp), i * ratioY - yFloor);
            }
        }
        texture.Resize (width, height);
        texture.SetPixels (newColors);
        texture.Apply ();
        sourceColors = null;
        newColors = null;
        return texture;
    }

    /// <summary>
    /// Color lerp
    /// </summary>
    static Color ColorLerpUnclamped (Color color1, Color color2, float value) {
        return new Color (color1.r + (color2.r - color1.r) * value, color1.g + (color2.g - color1.g) * value, color1.b + (color2.b - color1.b) * value, color1.a + (color2.a - color1.a) * value);
    }

    /// <summary>
    /// Crop texture with params
    /// </summary>
    /// <param name="texture">Source texture</param>
    /// <param name="rect">Size and bounds</param>
    /// <param name="options">Crop option</param>
    /// <param name="x">Offset x for custom crop (begin from top right)</param>
    /// <param name="y">Offset y for custom crop (begin from top right)</param>
    /// <returns>Cropped textute</returns>
    public static Texture2D CropTexture (Texture2D texture, Vector2 crop, CropOptions options = CropOptions.CENTER, int x = 0, int y = 0) {
        if (crop.x < 0f || crop.y < 0f) {
            return texture;
        }
        Rect sizes = new Rect ();
        Texture2D result = new Texture2D ((int) crop.x, (int) crop.y);
        if (crop.x != 0f && crop.y != 0f) {
            sizes.x = 0;
            sizes.y = 0;
            sizes.width = crop.x;
            sizes.height = crop.y;
            switch (options) {
                case CropOptions.CENTER:
                    sizes.x = (texture.width - crop.x) / 2f;
                    sizes.y = (texture.height - crop.y) / 2f;
                    break;
                case CropOptions.BOTTOM_RIGHT:
                    sizes.x = texture.width - crop.x;
                    break;
                case CropOptions.BOTTOM_LEFT:
                    break;
                case CropOptions.TOP_LEFT:
                    sizes.y = texture.height - crop.y;
                    break;
                case CropOptions.TOP_RIGHT:
                    sizes.x = texture.width - crop.x;
                    sizes.y = texture.height - crop.y;
                    break;
                case CropOptions.CUSTOM:
                    float width = texture.width - crop.x - x;
                    float height = texture.height - crop.y - y;
                    sizes.x = (width > texture.width) ? 0f : width;
                    sizes.y = (height > texture.height) ? 0f : height;
                    break;
            }
            if ((texture.width < sizes.x + crop.x) || (texture.height < sizes.y + crop.y) || (sizes.x > texture.width) || (sizes.y > texture.height) || (sizes.x < 0) || (sizes.y < 0) || (crop.x < 0) || (crop.y < 0)) {
                return texture;
            }
            result.SetPixels (texture.GetPixels (Mathf.FloorToInt (sizes.x), Mathf.FloorToInt (sizes.y), Mathf.FloorToInt (sizes.width), Mathf.FloorToInt (sizes.height)));
            result.Apply ();
        }
        return result;
    }

}