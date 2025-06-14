/*
* Author: Geng Baihui
* Date: 2025-06-14
* Description: Handles mask behavior when collected by the player, including sound playback and object destruction.
*/

using UnityEngine;

public class MaskBehaviour : MonoBehaviour
{
    /// <summary>
    /// Sound clip played when the crystal is collected.
    /// </summary>
    [SerializeField]
    private AudioClip collectSound;

    /// <summary>
    /// Collect the mask: play sound and remove the mask object.
    /// </summary>
    /// <param name="player"></param>
    public void Collect(PlayerBehaviour player)
    {
        Debug.Log("Mask Collected!");
        AudioSource.PlayClipAtPoint(collectSound, transform.position);
        Destroy(gameObject);
    }
}
