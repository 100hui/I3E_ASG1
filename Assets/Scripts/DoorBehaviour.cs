using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{
    [Tooltip("If > 0, player needs this many points instead of a key to open the door.")]
    public int requiredScore = 0; // If > 0, player needs this many points instead of a key to open the door.

    bool isOpen = false;
    float closedAngleY;

    void Start()
    {
        closedAngleY = transform.eulerAngles.y; // Store the initial Y rotation angle
    }

    public void Interact()
    {
        Vector3 doorRotation = transform.eulerAngles;
        if (isOpen)
        {
            doorRotation.y = closedAngleY; // Reset to closed angle
        }
        else
        {
            doorRotation.y = closedAngleY - 90f; // Open the door by rotating 90 degrees
        }
        transform.eulerAngles = doorRotation;
        isOpen = !isOpen;
    }
}
