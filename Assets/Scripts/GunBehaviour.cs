using UnityEngine;

public class GunBehaviour : MonoBehaviour
{
    [SerializeField]
    private AudioClip collectSound;
    public void Collect(PlayerBehaviour player)
    {
        Debug.Log("Gun Collected!");
        AudioSource.PlayClipAtPoint(collectSound, transform.position, 1f);
        Destroy(gameObject);
    }
}
