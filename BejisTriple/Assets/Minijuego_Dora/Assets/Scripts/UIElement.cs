using UnityEngine;

public class UIElement : MonoBehaviour
{
    public RectTransform panel;  // Reference to the RectTransform (UI object)
    public float speed = 5;
    public float maxDistance = 32;
    public bool isOpposite = false;
    Vector3 originalPos;  // Original position of the panel

    private void Start()
    {
        originalPos = panel.position;
    }

    // Method to move the panel based on mouse position and direction
    public void MoveWithCursor(Vector3 direction, float distance)
    {
        // Calculate the scaled distance based on the mouse distance, clamped to maxDistance
        float scaledDistance = Mathf.Min(distance, maxDistance);
        if (isOpposite)
        {
            panel.position = Vector3.Lerp(panel.position, originalPos + direction * scaledDistance, Time.deltaTime * speed);
        }
        else
        {
            panel.position = Vector3.Lerp(panel.position, originalPos - direction * scaledDistance, Time.deltaTime * speed);
        }

    }
}
