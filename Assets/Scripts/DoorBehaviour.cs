/*
* Author: Geng Baihui
* Date: 2025-06-14
* Description: Controls door opening / closing logic. 
*/

using UnityEngine;

/// <summary>
/// Manages door interactions: toggles open/closed state and checks for score requirements.
/// </summary>
public class DoorBehaviour : MonoBehaviour
{
    /// <summary>
    /// If > 0, the player must have at least this many points to open the door (ignores key requirement).
    /// </summary>
    [Tooltip("If > 0, player needs this many points instead of a key to open the door.")]
    public int requiredScore = 0; // If > 0, player needs this many points instead of a key to open the door.

    /// <summary>
    /// Tracks whether the door is currently open.
    /// </summary>
    bool isOpen = false;

    /// <summary>
    /// The Y-axis rotation angle of the door when closed.
    /// </summary>
    float closedAngleY;

    /// <summary>
    /// Initialize the closed angle based on the door's starting rotation.
    /// </summary>
    void Start()
    {
        closedAngleY = transform.eulerAngles.y; // Store the initial Y rotation angle
    }

    /// <summary>
    /// Toggles the door open or closed by rotating around its Y-axis.
    /// </summary>
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
