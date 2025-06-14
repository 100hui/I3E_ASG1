using UnityEngine;

public class MaskBehaviour : MonoBehaviour
{
    [SerializeField]
    private AudioClip collectSound;
    public void Collect(PlayerBehaviour player)
    {
        Debug.Log("Mask Collected!");
        AudioSource.PlayClipAtPoint(collectSound, transform.position);
        Destroy(gameObject);
    }
}
