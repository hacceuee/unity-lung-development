using UnityEngine;
using UnityEngine.UI;

public class MaterialSwitcher : MonoBehaviour
{
    public Material material; // The material you want to change
    public Color transparentColor = new Color(1f, 1f, 1f, 0f); // Transparent color (fully transparent)
    public Color translucentColor = new Color(1f, 1f, 1f, 0.5f); // Clear color (50% transparency)
    public Color opaqueColor = new Color(1f, 1f, 1f, 1f); // Solid color (fully opaque)

    // GameObjects associated with buttons under the same parent
    public GameObject[] relatedObjects; // List of objects to toggle visibility
    public Button[] buttons; // List of buttons to listen for clicks

    private int currentSelection = 1; // Start with index 1 active

    private void Start()
    {
        // Attach listeners to buttons
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i; // Local copy to avoid closure issues in the lambda
            buttons[i].onClick.AddListener(() => OnButtonClick(index));
        }

        // Initialize: make all related objects invisible
        HideAllObjects();
        
        // Initialize: start with translucent (index 1) active
        HideAllObjects();
        if (relatedObjects[currentSelection] != null)
        {
            relatedObjects[currentSelection].SetActive(true);
        }
        SetTranslucent(); // Set material to translucent (index 1)
    }

    public void OnButtonClick(int index)
    {
        // Hide all objects first
        HideAllObjects();

        // Show the associated object
        if (relatedObjects[index] != null)
        {
            relatedObjects[index].SetActive(true);
        }

        // Update material's color when a button is clicked
        /*switch (index)
        {
            case 0:
                SetTransparent();
                break;
            case 1:
                SetTranslucent();
                break;
            case 2:
                SetOpaque();
                break;
                
        }*/
    }

    private void HideAllObjects()
    {
        foreach (GameObject obj in relatedObjects)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
    }

    public void SetTransparent()
    {
        // Set the material color to transparent (alpha = 0)
        material.SetColor("_Color", transparentColor);
    }

    public void SetTranslucent()
    {
        // Set the material color to translucent (alpha = 0.5)
        material.SetColor("_Color", translucentColor);
    }

    public void SetOpaque()
    {
        // Set the material color to solid (alpha = 1)
        material.SetColor("_Color", opaqueColor);
    }

   
}
