using UnityEngine;
using TMPro;

public class PlayerBehaviour : MonoBehaviour
{
    int score = 0;
    bool canInteract = false;
    bool hasKey = false;
    CoinBehaviour currentCoin;
    DoorBehaviour currentDoor;
    KeyBehaviour currentKey;

    [SerializeField]
    GameObject projectile;

    [SerializeField]
    Transform spawnPoint;

    [SerializeField]
    float interactionDistance = 5f;

    [SerializeField]
    TextMeshProUGUI scoreText;

    [SerializeField]
    TextMeshProUGUI interactionText;

    [SerializeField]
    TextMeshProUGUI hintText;

    void Start()
    {
        scoreText.text = "Score: " + score.ToString();
        interactionText.text = "";
        hintText.text = "";
    }

    void Update()
    {
        RaycastHit hitInfo;
        Debug.DrawRay(spawnPoint.position, spawnPoint.forward * interactionDistance, Color.green);

        // Reset previous interaction states
        canInteract = false;
        interactionText.text = ""; // Clear interaction text

        if (currentCoin != null)
        {
            currentCoin.Unhighlight();
            // currentCoin = null;
        }

        currentKey = null;
        currentDoor = null;

        if (Physics.Raycast(spawnPoint.position, spawnPoint.forward, out hitInfo, interactionDistance))
        {
            GameObject hitObject = hitInfo.collider.gameObject;

            if (hitObject.CompareTag("Collectable"))
            {
                CoinBehaviour coin = hitObject.GetComponent<CoinBehaviour>();
                if (coin != null)
                {
                    currentCoin = hitInfo.collider.gameObject.GetComponent<CoinBehaviour>();
                    currentCoin.Highlight();
                    canInteract = true;
                }

                KeyBehaviour key = hitObject.GetComponent<KeyBehaviour>();
                if (key != null)
                {
                    currentKey = hitInfo.collider.gameObject.GetComponent<KeyBehaviour>();
                    canInteract = true;
                    interactionText.text = "Press 'E' to collect the key"; // Show interaction text for key
                }
            }
            else if (hitObject.CompareTag("Door"))
            {
                currentDoor = hitObject.GetComponent<DoorBehaviour>();
                if (currentDoor != null)
                {
                    canInteract = true;
                    interactionText.text = "Press 'E' to open the door"; // Show interaction text for door
                }
            }
        }
    }


    void OnInteract()
    {
        if (currentCoin != null)
        {
            currentCoin.Collect(this);
        }
        else if (currentKey != null)
        {
            currentKey.Collect(this);
            hasKey = true;
        }
        else if (currentDoor != null)
        {
            // DEBUG: see in Console what door we're hitting and what its requirement is
            Debug.Log($"Door \"{currentDoor.gameObject.name}\" requires {currentDoor.requiredScore} points; you have {score}");

            // If this door is score-gated...
            if (currentDoor.requiredScore > 0)
            {
                if (score < currentDoor.requiredScore)
                {
                    StartCoroutine(DisplayHint(
                        $"You need {currentDoor.requiredScore} scores to open the door"));
                    return;    // <— important, stops here if you don't have enough
                }
            }
            // Otherwise it's key-gated
            else
            {
                if (!hasKey)
                {
                    StartCoroutine(DisplayHint("You need a key to open this door."));
                    return;    // <— important, stops here if you don't have the key
                }
            }

            // If we got this far, either (score ≥ requiredScore) or (hasKey)
            currentDoor.Interact();
        }
    }




    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if (other.CompareTag("Collectable"))
        {
            currentCoin = other.GetComponent<CoinBehaviour>();
            if (currentCoin != null)
            {
                currentCoin.Collect(this); // Auto collect the coin
            }
        }
        else if (other.CompareTag("Door"))
        {
            canInteract = true;
            currentDoor = other.GetComponent<DoorBehaviour>();
        }
    }


    public void ModifyScore(int amount)
    {
        score += amount;
        Debug.Log("Score: " + score);
        scoreText.text = "Score: " + score.ToString();
    }

    void OnTriggerExit(Collider other)
    {
        if (currentCoin != null)
        {
            if (other.gameObject == currentCoin.gameObject)
            {
                canInteract = false;
                currentCoin.Unhighlight(); // Unhighlight the coin when player exits trigger
                currentCoin = null;
            }
        }
    }

    System.Collections.IEnumerator DisplayHint(string message)
    {
        hintText.text = message;
        yield return new WaitForSeconds(2f); // Display the hint for 2 seconds
        hintText.text = ""; // Clear the hint text
    }
}
