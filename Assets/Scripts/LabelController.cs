using UnityEngine;
using UnityEngine.UI;

public class LabelController : MonoBehaviour
{
    // References to the buttons
    public Button buttonOn;
    public Button buttonOff;

    // Sprite renderers for the "active" indicators
    public GameObject spriteOnActive;
    public GameObject spriteOffActive;

    // Array of GameObjects to toggle
    public GameObject[] gameObjects;

    // Internal state for the currently active button
    private int activeButtonIndex = 0; // Default to the "on" state

    void Start()
    {
        // Set the default state to "on"
        ToggleState(0);

        // Assign listeners to the buttons
        buttonOn.onClick.AddListener(() => ToggleState(0));
        buttonOff.onClick.AddListener(() => ToggleState(1));
    }

    // Method to toggle state based on button index
    void ToggleState(int buttonIndex)
    {
        if (activeButtonIndex == buttonIndex)
        {
            // If the same button is clicked, do nothing
            return;
        }

        // Update the active button index
        activeButtonIndex = buttonIndex;

        // Toggle game objects and update the "active" indicators
        SetGameObjectsActive(buttonIndex == 0);

        if (buttonIndex == 0)
        {
            spriteOnActive.SetActive(true);
            spriteOffActive.SetActive(false);
        }
        else if (buttonIndex == 1)
        {
            spriteOnActive.SetActive(false);
            spriteOffActive.SetActive(true);
        }
    }

    // Helper method to toggle the game objects
    void SetGameObjectsActive(bool isActive)
    {
        foreach (GameObject obj in gameObjects)
        {
            obj.SetActive(isActive);
        }
    }
}
