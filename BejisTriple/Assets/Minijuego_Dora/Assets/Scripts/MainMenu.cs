using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] UIElement[] uiElements;  // Array of UIElement objects

    void Update()
    {
        // Get the mouse position in screen space
        Vector3 mousePos = Input.mousePosition;

        // Calculate the screen center
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);

        // Calculate the direction vector from the screen center to the mouse position
        Vector3 oppositeDirection = screenCenter - mousePos;

        // Get the normalized direction vector
        Vector3 normDir = oppositeDirection.normalized;

        // Calculate the distance from the screen center to the mouse position
        float distance = oppositeDirection.magnitude;

        // Move each UI element based on its settings
        foreach (var uiElement in uiElements)
        {
            // Move the panel with the appropriate speed and direction
            uiElement.MoveWithCursor(normDir, distance);
        }
    }
}
