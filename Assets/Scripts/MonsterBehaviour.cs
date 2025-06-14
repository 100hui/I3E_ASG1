/*
* Author: Geng Baihui
* Date: 2025-06-14
* Description: Handles monster behavior when hit by the player's projectile, including sound effect, crystal spawning, and destruction.
*/

using UnityEngine;

/// <summary>
/// Listens for collisions with the player's projectile. Upon being hit, plays a damage sound, spawns a crystal, and destroys itself and the projectile.
/// </summary>
public class MonsterBehaviour : MonoBehaviour
{
    /// <summary>
    /// Sound clip to play when the monster takes damage from being shot.
    /// </summary>
    [SerializeField]
    private AudioClip damageSound;

    /// <summary>
    /// Prefab of the crystal to spawn when the monster is shot.
    /// </summary>
    [Tooltip("The crystal prefab to spawn when monster is shot")]
    public GameObject crystalPrefab;

    /// <summary>
    /// Called by Unity when another Collider makes contact with this object's collider.
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionEnter(Collision collision)
    {
        // only react to our projectile
        if (collision.gameObject.CompareTag("Projectile"))
        {
            // play damage sound
            AudioSource.PlayClipAtPoint(damageSound, transform.position, 1f);
            // spawn crystal at monster's position & rotation
            Instantiate(crystalPrefab, transform.position, transform.rotation);

            // destroy monster and the projectile
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }
    }
}
