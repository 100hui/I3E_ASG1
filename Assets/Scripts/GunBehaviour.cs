/*
* Author: Geng Baihui
* Date: 2025-06-14
* Description: Handles player interaction for collecting the gun, including sound playback and object destruction.
*/

using UnityEngine;

/// <summary>
/// Represents the collectible gun in the scene, playing a sound on collection and notifying the player.
/// </summary>
public class GunBehaviour : MonoBehaviour
{
    /// <summary>
    /// Sound clip played when the key is collected.
    /// </summary>
    [SerializeField]
    private AudioClip collectSound;

    /// <summary>
    /// Collect the gun: play sound and remove the gun object.
    /// </summary>
    /// <param name="player"></param>
    public void Collect(PlayerBehaviour player)
    {
        Debug.Log("Gun Collected!");
        AudioSource.PlayClipAtPoint(collectSound, transform.position, 1f);
        Destroy(gameObject);
    }
}
