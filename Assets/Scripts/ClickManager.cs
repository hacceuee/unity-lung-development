using UnityEngine;

public class ClickManager : MonoBehaviour
{
    // Array of 2D sprites to be checked for clicks
    public GameObject[] clickableObjects;

    void Update()
    {
        // Check if the left mouse button is clicked
        if (Input.GetMouseButtonDown(0)) // 0 is for left-click
        {
            HandleClick();
        }
    }

    // Method to handle the click detection
    private void HandleClick()
    {
        // Convert the mouse position to world space (accounting for the camera)
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Create a ray at the mouse position
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

        // Check if the Raycast hit something
        if (hit.collider != null)
        {
            // If the hit collider is one of the objects in the array
            foreach (GameObject obj in clickableObjects)
            {
                if (hit.collider.gameObject == obj)
                {
                    // Output the name of the clicked object
                    Debug.Log("Clicked on: " + obj.name);
                    break;
                }
            }
        }
    }
}
