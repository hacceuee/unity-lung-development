using UnityEngine;
using UnityEngine.UI;

public class ControlsController : MonoBehaviour
{
    public GameObject rotatableObject; // The object to rotate
    public GameObject toggleableObject; // The object to show/hide

    private bool isToggled = false; // Track the toggle state
    private Vector3 defaultRotation = new Vector3(0f, 0f, 0f); // Default rotation
    private Vector3 toggledRotation = new Vector3(180f, 0f, 0f); // Toggled rotation

    // Method to toggle rotation and visibility
    public void Toggle()
    {
        Debug.Log("clicked");
        if (rotatableObject != null && toggleableObject != null)
        {
            // Toggle state
            isToggled = !isToggled;

            // Rotate the object and toggle visibility
            rotatableObject.transform.eulerAngles = isToggled ? toggledRotation : defaultRotation;
            toggleableObject.SetActive(isToggled);
        }
    }
}
