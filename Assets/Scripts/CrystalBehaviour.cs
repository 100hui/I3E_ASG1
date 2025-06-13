using UnityEngine;

public class CrystalBehaviour : MonoBehaviour
{
    [SerializeField]
    private AudioClip collectSound;
    public void Collect(PlayerBehaviour player)
    {
        Debug.Log("Crystal Collected! You win!");
        player.Win();
        AudioSource.PlayClipAtPoint(collectSound, transform.position, 1f);
        Destroy(gameObject);
    }
}
