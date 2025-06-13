using UnityEngine;

public class CrystalBehaviour : MonoBehaviour
{
    public void Collect(PlayerBehaviour player)
    {
        Debug.Log("Crystal Collected! You win!");
        player.Win();
        Destroy(gameObject);
    }
}
