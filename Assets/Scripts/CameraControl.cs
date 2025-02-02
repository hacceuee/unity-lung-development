﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // For handling UI elements like buttons

public class CameraControl : MonoBehaviour
{
    // Variables for rotation, panning, and zooming
    private Vector3 previousMousePosition;
    private Vector3 mouseDelta;
    private Vector3 screenPoint;
    private Vector3 offset;

    // Rotation settings
    public float rotationSpeed = 10f;
    public float rotationAcceleration = 2f;
    private float currentRotationSpeed = 0f;

    // Gentle spin cooldown settings
    public float rotationCooldownDuration = 0.5f; // Duration of the pause after mouse release (in seconds)
    private float rotationCooldown = 0f; // Timer for the cooldown

    // Zoom settings
    public float zoomSpeed = 0.5f;
    public float minZoom = 20f;
    public float maxZoom = 100f;
    private Camera cam;

    // Track mouse button press state
    private bool isMouseButtonHeld = false;
    private float mouseButtonHoldTime = 0f;

    // Auto-rotation state
    private bool isAutoRotationActive = true; // Default to on

    // Buttons to control auto-rotation
    public Button toggleOnButton;  // Button to turn auto-rotation on
    public Button toggleOffButton; // Button to turn auto-rotation off

    // Sprites or GameObjects to show when each button is active
    public GameObject activeOnSprite;  // Sprite for the "on" button
    public GameObject activeOffSprite; // Sprite for the "off" button

    private void Start()
    {
        cam = Camera.main;

        // Set up the button listeners
        toggleOnButton.onClick.AddListener(TurnAutoRotationOn);
        toggleOffButton.onClick.AddListener(TurnAutoRotationOff);

        // Set the initial state based on `isAutoRotationActive`
        UpdateButtonStates();
    }

    private void Update()
    {
        // If gentle rotation is active, allow it, else just let regular mouse movement handle the rotation
        if (isAutoRotationActive)
        {
            // Apply gentle rotation if it's active
            ApplyGentleRotation();
        }

        // Handle camera orbit rotation with Left Mouse Button
        if (Input.GetMouseButton(0) && !IsAltPressed())
        {
            HandleOrbitRotation();
            isMouseButtonHeld = true; // Start tracking button hold time
        }

        if (Input.GetMouseButtonDown(0) && !IsAltPressed())
        {
            mouseButtonHoldTime = 0f; // Reset the hold time when the button is first pressed
        }

        if (Input.GetMouseButtonUp(0) && !IsAltPressed())
        {
            isMouseButtonHeld = false; // Stop tracking button hold time when released
        }

        // Handle panning with Right Mouse Button
        if (Input.GetMouseButtonDown(1))
        {
            InitializePanning();
        }
        else if (Input.GetMouseButton(1))
        {
            Pan();
        }

        // Handle zoom with Alt + Left or Right Mouse Button
        if ((Input.GetMouseButton(0) || Input.GetMouseButton(1)) && IsAltPressed())
        {
            Zoom();
        }

        Debug.Log(mouseButtonHoldTime);

        // Only reset the cooldown if the button has been held long enough
        if (isMouseButtonHeld)
        {
            mouseButtonHoldTime += Time.deltaTime;
            rotationCooldown = 0f; // Don't apply the cooldown until the button is released.
            currentRotationSpeed = 0f; // Reset rotation speed while the button is held
        }
        else if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
        {
            rotationCooldown = rotationCooldownDuration; // Reset the cooldown if the button is pressed again
        }
    }

    // Handles orbiting the camera around the object
    private void HandleOrbitRotation()
    {
        if (Input.GetMouseButtonDown(0))
        {
            previousMousePosition = Input.mousePosition;
        }

        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            mouseDelta = Input.mousePosition - previousMousePosition;

            float horizontalRotation = Vector3.Dot(mouseDelta, Camera.main.transform.right);
            float verticalRotation = Vector3.Dot(mouseDelta, Camera.main.transform.up);

            if (Vector3.Dot(transform.up, Vector3.up) >= 0)
            {
                transform.Rotate(transform.up, -horizontalRotation, Space.World);
            }
            else
            {
                transform.Rotate(transform.up, horizontalRotation, Space.World);
            }

            transform.Rotate(Camera.main.transform.right, verticalRotation, Space.World);
            previousMousePosition = Input.mousePosition;
        }
    }

    // Initializes the panning with Right Mouse Button
    private void InitializePanning()
    {
        screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    }

    // Handles panning the camera
    private void Pan()
    {
        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            Vector3 currentScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            Vector3 newPosition = Camera.main.ScreenToWorldPoint(currentScreenPoint) + offset;
            transform.position = newPosition;
        }
    }

    // Handles zooming with Alt + Left or Right Mouse Button
    private void Zoom()
    {
        float mouseY = Input.GetAxis("Mouse Y");
        float mouseX = Input.GetAxis("Mouse X");

        float scrollAmount = (mouseY - mouseX) * zoomSpeed;
        cam.fieldOfView = Mathf.Clamp(cam.fieldOfView - scrollAmount, minZoom, maxZoom);
    }

    // Applies a gentle automatic rotation with a cooldown delay after mouse release
    private void ApplyGentleRotation()
    {
        // Decrease the cooldown timer if it's greater than 0
        if (rotationCooldown > 0)
        {
            rotationCooldown -= Time.deltaTime;
            return; // Pause gentle rotation until cooldown is over
        }

        // Gradually increase rotation speed after cooldown
        if (currentRotationSpeed < rotationSpeed)
        {
            currentRotationSpeed = Mathf.Lerp(currentRotationSpeed, rotationSpeed, rotationAcceleration * Time.deltaTime);
        }

        // Apply the gentle rotation
        transform.Rotate(Vector3.up, currentRotationSpeed * Time.deltaTime);
    }

    // Helper method to check if either Left Alt or Right Alt is pressed
    private bool IsAltPressed()
    {
        return Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt);
    }

    // Toggle auto-rotation on
    private void TurnAutoRotationOn()
    {
        isAutoRotationActive = true;
        UpdateButtonStates();
    }

    // Toggle auto-rotation off
    private void TurnAutoRotationOff()
    {
        isAutoRotationActive = false;
        currentRotationSpeed = 0f; // Immediately stop any ongoing gentle rotation
        UpdateButtonStates();
    }

    // Update button states (toggle sprites for "active" state)
    private void UpdateButtonStates()
    {
        activeOnSprite.SetActive(isAutoRotationActive);   // Show "active" sprite for on button
        activeOffSprite.SetActive(!isAutoRotationActive); // Show "active" sprite for off button

        // Optionally, you can also disable/enable the buttons themselves if needed
        toggleOnButton.interactable = !isAutoRotationActive;  // Disable button when it's on
        toggleOffButton.interactable = isAutoRotationActive; // Disable button when it's off
    }
}
