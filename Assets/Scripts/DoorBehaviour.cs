using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{
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
