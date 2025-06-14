/*
* Author: Geng Baihui
* Date: 2025-06-14
* Description: Handles crystal behavior when collected by the player, including sound playback and object destruction.
*/

using UnityEngine;

/// <summary>
/// Represents the collectible crystal in the scene, playing a sound on collection and notifying the player.
/// </summary>
public class CrystalBehaviour : MonoBehaviour
{
    /// <summary>
    /// Sound clip played when the crystal is collected.
    /// </summary>
    [SerializeField]
    private AudioClip collectSound;

    /// <summary>
    /// Collect the crystal: play sound and notify the player.
    /// </summary>
    /// <param name="player"></param>
    public void Collect(PlayerBehaviour player)
    {
        Debug.Log("Crystal Collected! You win!");
        player.Win();
        AudioSource.PlayClipAtPoint(collectSound, transform.position, 1f);
        Destroy(gameObject);
    }
}
