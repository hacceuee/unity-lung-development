using UnityEngine;
using UnityEngine.EventSystems;  // Add the EventSystems namespace

public class CircleClickManager : MonoBehaviour, IPointerClickHandler // Implement IPointerClickHandler
{
    [Header("UI Circles")]
    public GameObject[] uiCircles; // 12 UI circles (GameObjects with BoxCollider2D components)
    public Sprite activeSprite; // The sprite to show when the object is "active"
    public Sprite inactiveSprite; // The sprite to show when the object is inactive

    [Header("3D Objects")]
    public GameObject[] objectsInScene; // 12 3D objects to toggle between

    private int currentIndex = -1; // Index of the currently active object

    void Start()
    {
        // Ensure all 3D objects are initially deactivated
        foreach (GameObject obj in objectsInScene)
        {
            obj.SetActive(false);
        }

        // Set all circles to the inactive sprite at the start
        foreach (GameObject circle in uiCircles)
        {
            SpriteRenderer spriteRenderer = circle.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = inactiveSprite; // Set the inactive sprite to all circles
            }
        }
    }

    void Update()
    {
        // Optional: Handle mouse click or touch input manually if needed
    }

    // Implement the IPointerClickHandler interface method
    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject clickedObject = eventData.pointerCurrentRaycast.gameObject;

        // Check if the clicked object is one of the UI circles
        for (int i = 0; i < uiCircles.Length; i++)
        {
            if (clickedObject == uiCircles[i])
            {
                Debug.Log("Clicked on circle: " + uiCircles[i].name);
                OnCircleClick(i);
                break; // Stop once we find the clicked circle
            }
        }
    }

    // This function is called when a circle is clicked
    public void OnCircleClick(int index)
    {
        if (index == currentIndex) return; // Ignore click if the same object is already active

        // Deactivate the previous object, if any
        if (currentIndex >= 0)
        {
            objectsInScene[currentIndex].SetActive(false);
            // Reset the previous circle's sprite to inactive
            SpriteRenderer previousCircleRenderer = uiCircles[currentIndex].GetComponent<SpriteRenderer>();
            if (previousCircleRenderer != null)
            {
                previousCircleRenderer.sprite = inactiveSprite;
            }
        }

        // Activate the new object and set the sprite of the corresponding circle to "active"
        objectsInScene[index].SetActive(true);

        // Set the sprite of the clicked circle to the "active" sprite
        SpriteRenderer clickedCircleRenderer = uiCircles[index].GetComponent<SpriteRenderer>();
        if (clickedCircleRenderer != null)
        {
            clickedCircleRenderer.sprite = activeSprite;
        }

        // Update the current index to the new selection
        currentIndex = index;
    }
}
