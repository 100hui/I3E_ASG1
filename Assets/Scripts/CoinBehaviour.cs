using UnityEngine;

public class CoinBehaviour : MonoBehaviour
{
    MeshRenderer myMeshRenderer;

    [SerializeField]
    float rotationSpeed = 100f; 

    [SerializeField]
    private AudioClip collectSound;

    [SerializeField]
    Material highlightMat;
    Material originalMat;
    public int coinValue = 10;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myMeshRenderer = GetComponent<MeshRenderer>();
        originalMat = myMeshRenderer.material; // Store the original material
    }

    public void Highlight()
    {
        myMeshRenderer.material = highlightMat; // Change to highlight material
    }
    public void Unhighlight()
    {
        myMeshRenderer.material = originalMat; // Reset to original material
    }

    public void Collect(PlayerBehaviour player)
    {
        player.ModifyScore(coinValue);
        AudioSource.PlayClipAtPoint(collectSound, transform.position, 1f);
        Destroy(gameObject);
    }
    void Update()
    {
        // Rotate the coin around its Z axis
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }
}
