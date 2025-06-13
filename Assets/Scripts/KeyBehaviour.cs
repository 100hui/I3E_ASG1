using UnityEngine;

public class KeyBehaviour : MonoBehaviour
{
    [SerializeField]
    private AudioClip collectSound;
    public void Collect(PlayerBehaviour player)
    {
        Debug.Log("Key Collected!");
        AudioSource.PlayClipAtPoint(collectSound, transform.position, 1f);
        Destroy(gameObject);
    }
}
