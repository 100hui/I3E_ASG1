using UnityEngine;

public class CoinBehaviour : MonoBehaviour
{
    MeshRenderer myMeshRenderer;

    [SerializeField]
    float rotationSpeed = 100f; 

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
        Destroy(gameObject);
    }
    void Update()
    {
        // Rotate the coin around its Z axis
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }
}
