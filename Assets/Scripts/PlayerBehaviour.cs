using UnityEngine;
using TMPro;
using System.Collections;

public class PlayerBehaviour : MonoBehaviour
{
    int score = 0;
    bool canInteract = false;
    bool hasKey = false;
    bool hasGun = false;
    CoinBehaviour currentCoin;
    DoorBehaviour currentDoor;
    KeyBehaviour currentKey;
    GunBehaviour currentGun;
    CrystalBehaviour currentCrystal;

    [SerializeField]
    GameObject projectile;

    [SerializeField]
    Transform spawnPoint;

    [SerializeField]
    Transform room2StartPoint;

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
        currentGun = null;
        currentCrystal = null;

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

                GunBehaviour gun = hitObject.GetComponentInParent<GunBehaviour>();
                if (gun != null)
                {
                    currentGun = gun;
                    canInteract = true;
                    interactionText.text = "Press 'E' to collect the gun"; // Show interaction text for gun
                }

                CrystalBehaviour crystal = hitObject.GetComponent<CrystalBehaviour>();
                if (crystal != null)
                {
                    canInteract = true;
                    interactionText.text = "Press 'E' to collect the crystal"; // Show interaction text for crystal
                    currentCrystal = crystal;
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
        if (hasGun && Input.GetKeyDown(KeyCode.F))
        {
            var shot = Instantiate(projectile, spawnPoint.position, spawnPoint.rotation);
            var rb = shot.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.useGravity = true;
                float shootSpeed = 20f;
                rb.linearVelocity = spawnPoint.forward * shootSpeed;
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
        else if (currentGun != null)
        {
            currentGun.Collect(this);
            hasGun = true;
            StartCoroutine(DisplayHint("Press 'F' to shoot the monster"));
        }
        else if (currentCrystal != null)
        {
            currentCrystal.Collect(this);
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

        // 1) Water death → start delayed respawn
        if (other.CompareTag("Water"))
        {
            StartCoroutine(DeathAndRespawn());
            return;  // skip the rest this frame
        }
        if (other.CompareTag("Room2Start"))
        {
            StartCoroutine(DisplayHint("Be careful—if you fall into the water, you'll die"));
        }
        // 2) Auto‐collect coins
        if (other.CompareTag("Collectable"))
        {
            currentCoin = other.GetComponent<CoinBehaviour>();
            if (currentCoin != null)
                currentCoin.Collect(this);
        }
        // 3) Door interaction
        else if (other.CompareTag("Door"))
        {
            canInteract = true;
            currentDoor = other.GetComponent<DoorBehaviour>();
        }
    }

    private IEnumerator DeathAndRespawn()
    {
        // Show “You died” for 2 seconds
        yield return DisplayHint("You died");

        // Then teleport back to Room 2 start
        var cc = GetComponent<CharacterController>();
        if (cc != null)
        {
            cc.enabled = false;
            transform.position = room2StartPoint.position;
            cc.enabled = true;
        }
        else
        {
            var rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.position = room2StartPoint.position;
            }
            else
            {
                transform.position = room2StartPoint.position;
            }
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
    
    public void Win()
    {
        StartCoroutine(DisplayHint("You win!"));
    }
}
