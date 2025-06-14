/*
* Author: Geng Baihui
* Date: 2025-06-14
* Description: Rotates the coin, handles highlight on focus, and manages collection logic including score increment and sound playback.
*/

using UnityEngine;

/// <summary>
/// Manages coin behaviour: spinning, highlighting on player focus, and collection.
/// </summary>
public class CoinBehaviour : MonoBehaviour
{
    /// <summary>
    /// MeshRenderer used to change the coin's material for highlight/unhighlight.
    /// </summary>
    MeshRenderer myMeshRenderer;

    /// <summary>
    /// Speed (degrees per second) at which the coin rotates.
    /// </summary>
    [SerializeField]
    float rotationSpeed = 100f;

    /// <summary>
    /// Sound clip played when the coin is collected.
    /// </summary>
    [SerializeField]
    private AudioClip collectSound;

    /// <summary>
    /// Material used to highlight / unhighlight the coin.
    /// </summary>
    [SerializeField]
    Material highlightMat;
    Material originalMat;

    /// <summary>
    /// Score value awarded to the player upon collecting this coin.
    /// </summary>
    public int coinValue = 10;

    /// <summary>
    /// Initialize references and store the original material.
    /// Called once before the first frame update.
    /// </summary>
    void Start()
    {
        myMeshRenderer = GetComponent<MeshRenderer>();
        originalMat = myMeshRenderer.material; // Store the original material
    }

    /// <summary>
    /// Change the coin's material to the highlight material.
    /// </summary>
    public void Highlight()
    {
        myMeshRenderer.material = highlightMat; // Change to highlight material
    }

    /// <summary>
    /// Revert the coin's material to its original.
    /// </summary>
    public void Unhighlight()
    {
        myMeshRenderer.material = originalMat; // Reset to original material
    }

    /// <summary>
    /// Collect the coin: increment player's score, play sound, and destroy the coin.
    /// </summary>
    /// <param name="player"></param>
    public void Collect(PlayerBehaviour player)
    {
        player.ModifyScore(coinValue);
        AudioSource.PlayClipAtPoint(collectSound, transform.position, 1f);
        Destroy(gameObject);
    }

    /// <summary>
    /// Rotate the coin each frame around its forward axis.
    /// </summary>
    void Update()
    {
        // Rotate the coin around its Z axis
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }
}
