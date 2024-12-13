using UnityEngine;

public class MaterialSwitcher : MonoBehaviour
{
    public Material material; // The material you want to change
    public Color transparentColor = new Color(1f, 1f, 1f, 0f); // Transparent color (fully transparent)
    public Color solidColor = new Color(1f, 1f, 1f, 1f); // Solid color (fully opaque)
    public Color clearColor = new Color(1f, 1f, 1f, 0.5f); // Clear color (50% transparency)

    public void SetTransparent()
    {
        // Set the material color to transparent (alpha = 0)
        material.SetColor("_Color", transparentColor);
    }

    public void SetSolid()
    {
        // Set the material color to solid (alpha = 1)
        material.SetColor("_Color", solidColor);
    }

    public void SetClear()
    {
        // Set the material color to clear (alpha = 0.5)
        material.SetColor("_Color", clearColor);
    }
}
