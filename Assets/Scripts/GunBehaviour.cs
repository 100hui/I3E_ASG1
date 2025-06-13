using UnityEngine;

public class GunBehaviour : MonoBehaviour
{
    public void Collect(PlayerBehaviour player)
    {
        Debug.Log("Gun Collected!");
        Destroy(gameObject);
    }
}
