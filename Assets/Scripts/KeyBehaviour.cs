using UnityEngine;

public class KeyBehaviour : MonoBehaviour
{
    public void Collect(PlayerBehaviour player)
    {
        Debug.Log("Key Collected!");
        Destroy(gameObject);
    }
}
