using UnityEngine;
using UnityEngine.UI;

public class TimelineManager : MonoBehaviour
{
    // Arrays of UI Buttons and GameObjects
    public Button[] timelineCheckpoint; // Buttons
    public Sprite unselectedSprite; // The sprite for the unselected state
    public Sprite selectedSprite; // The sprite for the selected state
    public GameObject[] timelineSprite; // GameObjects with Sprite Renderers (that will change sprite)
    public GameObject[] lungModels; // Models corresponding to each checkpoint

    private int currentSelection = -1; // To track the currently selected button/model

    void Start()
    {
        // Ensure all buttons have a listener attached
        for (int i = 0; i < timelineCheckpoint.Length; i++)
        {
            int index = i; // Local copy to avoid closure issue in the lambda
            timelineCheckpoint[i].onClick.AddListener(() => OnButtonClick(index));
        }

        // Initial state: hide all models and set sprites to unselected
        for (int i = 0; i < lungModels.Length; i++)
        {
            lungModels[i].SetActive(false);
            SetSprite(i, false); // Set to unselected sprite initially
        }
    }

    // Method to handle button click
    void OnButtonClick(int index)
    {
        // If there's a current selection, reset it
        if (currentSelection != -1)
        {
            // Hide the previous model
            lungModels[currentSelection].SetActive(false);
            SetSprite(currentSelection, false); // Set previous sprite to unselected
        }

        // Show the selected model and update the sprite
        lungModels[index].SetActive(true);
        SetSprite(index, true); // Set sprite to selected

        // Update the current selection to the new button
        currentSelection = index;
    }

    // Helper method to set the sprite of a timelineSprite object
    void SetSprite(int index, bool isSelected)
    {
        if (timelineSprite[index].GetComponent<SpriteRenderer>() != null)
        {
            SpriteRenderer spriteRenderer = timelineSprite[index].GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = isSelected ? selectedSprite : unselectedSprite;
        }
    }
}
