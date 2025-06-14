/*
* Author: Geng Baihui
* Date: 2025-06-14
* Description: Handles player interaction for collecting the key, including sound playback and object destruction. 
*/

using UnityEngine;

/// <summary>
/// Represents the collectible key in the scene, playing a sound on collection and notifying the player.
/// </summary>
public class KeyBehaviour : MonoBehaviour
{
    /// <summary>
    /// Sound clip played when the key is collected.
    /// </summary>
    [SerializeField]
    private AudioClip collectSound;

    /// <summary>
    /// Collect the key: play sound and remove the key object.
    /// </summary>
    /// <param name="player"></param>
    public void Collect(PlayerBehaviour player)
    {
        Debug.Log("Key Collected!");
        AudioSource.PlayClipAtPoint(collectSound, transform.position, 1f);
        Destroy(gameObject);
    }
}
