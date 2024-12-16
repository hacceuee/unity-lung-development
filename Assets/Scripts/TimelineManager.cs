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
    public GameObject[] informationPanel; // Information panels corresponding to each checkpoint

    public GameObject pleaseSelectLabel; // Label that says "Please Select"
    public Transform movingObject; // Object to move smoothly
    public float moveSpeed = 2f; // Speed of movement

    private bool movingUp = true; // Direction of movement
    private Vector3 initialPosition; // To store the starting position
    private bool firstSelectionMade = false; // To track if the first selection has been made
    private int currentSelection = -1; // To track the currently selected button/model

    void Start()
    {
        // Store the object's initial position
        if (movingObject != null)
        {
            initialPosition = movingObject.localPosition;
        }

        // Ensure all buttons have a listener attached
        for (int i = 0; i < timelineCheckpoint.Length; i++)
        {
            int index = i; // Local copy to avoid closure issue in the lambda
            timelineCheckpoint[i].onClick.AddListener(() => OnButtonClick(index));
        }

        // Initial state: hide all models, information panels, set sprites to unselected, and show the "Please Select" label
        if (pleaseSelectLabel != null)
        {
            pleaseSelectLabel.SetActive(true);
        }

        for (int i = 0; i < lungModels.Length; i++)
        {
            lungModels[i].SetActive(false);
            if (informationPanel.Length > i)
            {
                informationPanel[i].SetActive(false);
            }
            SetSprite(i, false); // Set to unselected sprite initially
        }

        // Initialize with index 7 active
        ActivateDefaultSelection(7);
    }

    void Update()
    {
        // If the label is active, move the object
        if (pleaseSelectLabel != null && pleaseSelectLabel.activeSelf && movingObject != null)
        {
            MoveObject();
        }
    }

    // Method to handle button click
    void OnButtonClick(int index)
    {
        // Hide the "Please Select" label on the first selection
        if (!firstSelectionMade)
        {
            if (pleaseSelectLabel != null)
            {
                pleaseSelectLabel.SetActive(false);
            }
            firstSelectionMade = true;
        }

        // If there's a current selection, reset it
        if (currentSelection != -1)
        {
            // Hide the previous model and information panel
            lungModels[currentSelection].SetActive(false);
            if (informationPanel.Length > currentSelection)
            {
                informationPanel[currentSelection].SetActive(false);
            }
            SetSprite(currentSelection, false); // Set previous sprite to unselected
        }

        // Show the selected model, information panel, and update the sprite
        lungModels[index].SetActive(true);
        if (informationPanel.Length > index)
        {
            informationPanel[index].SetActive(true);
        }
        SetSprite(index, true); // Set sprite to selected

        // Update the current selection to the new button
        currentSelection = index;
    }

    // Helper method to move the object
    void MoveObject()
    {
        // Calculate the movement bounds relative to the initial position
        float newY = movingObject.localPosition.y + (movingUp ? moveSpeed : -moveSpeed) * Time.deltaTime;

        // Check if we reached the bounds
        if (newY >= initialPosition.y + 2.5f)
        {
            newY = initialPosition.y + 2.5f;
            movingUp = false;
        }
        else if (newY <= initialPosition.y - 2.5f)
        {
            newY = initialPosition.y - 2.5f;
            movingUp = true;
        }

        // Apply the new position
        movingObject.localPosition = new Vector3(movingObject.localPosition.x, newY, movingObject.localPosition.z);
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

    // Helper method to activate the default selection (index 10)
    void ActivateDefaultSelection(int index)
    {
        // Validate index
        if (index < 0 || index >= lungModels.Length || index >= timelineSprite.Length || index >= informationPanel.Length)
        {
            Debug.LogWarning("Invalid default index.");
            return;
        }

        // Activate the lung model, timeline sprite, and information panel for the default index
        lungModels[index].SetActive(true);
        
        informationPanel[index].SetActive(false);
      
        SetSprite(index, true);
        currentSelection = index;
    }
}
