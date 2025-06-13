using UnityEngine;

public class MonsterBehaviour : MonoBehaviour
{
    [SerializeField]
    private AudioClip damageSound;

    [Tooltip("The crystal prefab to spawn when monster is shot")]
    public GameObject crystalPrefab;

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
