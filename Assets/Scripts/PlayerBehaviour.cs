/*
* Author: Geng Baihui
* Date: 2025-06-14
* Description: Controls player movement, interaction (coins, doors, gas, etc.),
*              shooting, and respawn mechanics across the three rooms.
*/

using UnityEngine;
using TMPro;
using System.Collections;

/// <summary>
/// Manages player input, UI, hints, and physics-based interactions. 
/// </summary>
public class PlayerBehaviour : MonoBehaviour
{
    /// <summary>
    /// Player's current score, increases by 10 per coin collected. 
    /// </summary>
    int score = 0;

    /// <summary>
    /// Whether theplayer is currently able to interact with objects.
    /// </summary>
    bool canInteract = false;

    /// <summary>
    ///  Whether the player has collected a key, gun, or mask.
    /// </summary>
    bool hasKey = false;
    bool hasGun = false;
    bool hasMask = false;

    /// <summary>
    /// Reference to the current interactable objects in the scene.
    /// </summary>
    CoinBehaviour currentCoin;
    DoorBehaviour currentDoor;
    KeyBehaviour currentKey;
    GunBehaviour currentGun;
    CrystalBehaviour currentCrystal;
    MaskBehaviour currentMask;

    /// <summary>
    /// Projectile prefab to instantiate when shooting. 
    /// </summary>
    [SerializeField]
    GameObject projectile;

    /// <summary>
    ///  Transform representing the spawn point for projectiles and raycasts.
    /// </summary>
    [SerializeField]
    Transform spawnPoint;

    /// <summary>
    /// Transform for the respawn location in Room 2.
    /// </summary>
    [SerializeField]
    Transform room2StartPoint;

    /// <summary>
    /// Transform for the respawn location in Room 3.
    /// </summary>
    [SerializeField]
    Transform room3StartPoint;

    /// <summary>
    /// Maximum distance for interaction raycasts.
    /// </summary>
    [SerializeField]
    float interactionDistance = 5f;

    /// <summary>
    /// UI text element displaying the player's score.
    /// </summary>
    [SerializeField]
    TextMeshProUGUI scoreText;

    /// <summary>
    /// UI text element displaying interaction instructions.
    /// </summary>
    [SerializeField]
    TextMeshProUGUI interactionText;

    /// <summary>
    /// UI text element for temporary hints and messages.
    /// </summary>
    [SerializeField]
    TextMeshProUGUI hintText;

    /// <summary>
    /// Initialization: set initial UI and show starting hints.
    /// </summary>
    void Start()
    {
        scoreText.text = "Score: " + score.ToString();
        interactionText.text = "";
        hintText.text = "";

        StartCoroutine(InitialHint());
    }

    /// <summary>
    /// Main update loop: handle raycast interactions and shooting input. 
    /// </summary>
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
        currentMask = null;

        if (Physics.Raycast(spawnPoint.position, spawnPoint.forward, out hitInfo, interactionDistance))
        {
            GameObject hitObject = hitInfo.collider.gameObject;

            if (hitObject.CompareTag("Gas") && !hasMask)
            {
                StartCoroutine(DisplayHint("Dangerous! You need a mask to go through the toxic gas area"));
            }

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

                MaskBehaviour mask = hitObject.GetComponentInParent<MaskBehaviour>();
                if (mask != null)
                {
                    currentMask = mask;
                    canInteract = true;
                    interactionText.text = "Press 'E' to collect the mask"; // Show interaction text for mask
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

    /// <summary>
    ///  Handles player interaction with objects when the 'E' key is pressed.
    /// </summary>
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
        else if (currentMask != null)
        {
            currentMask.Collect(this);
            hasMask = true;
            StartCoroutine(DisplayHint("You can now go through the toxic gas area"));
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

    /// <summary>
    /// Trigger enter logic for gas, water death, Room2 hints, and collisions.
    /// </summary>
    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);

        if (other.CompareTag("Gas") && !hasMask)
        {
            StartCoroutine(GasDeathAndRespawn());
            return;  // skip the rest this frame
        }

        // 1) Water death → start delayed respawn
        if (other.CompareTag("Water"))
        {
            StartCoroutine(DeathAndRespawn());
            return;  // skip the rest this frame
        }
        if (other.CompareTag("Room2Start"))
        {
            StartCoroutine(DisplayHint("Be careful — if you fall into the water, you'll die"));
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

    /// <summary>
    ///  Coroutine to handle player death and respawn in Room 2.
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    ///  Coroutine to handle player death in the gas area and respawn in Room 3.
    /// </summary>
    /// <returns></returns>
    private IEnumerator GasDeathAndRespawn()
    {
        // Show “You died” for 2 seconds
        yield return DisplayHint("You died");

        // Teleport back to Room 3 start
        var cc = GetComponent<CharacterController>();
        if (cc != null)
        {
            cc.enabled = false;
            transform.position = room3StartPoint.position;
            cc.enabled = true;
        }
        else
        {
            var rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.position = room3StartPoint.position;
            }
            else
            {
                transform.position = room3StartPoint.position;
            }
        }
    }

    /// <summary>
    /// Coroutine to display initial hints at the start of the game.
    /// </summary>
    /// <returns></returns>
    private IEnumerator InitialHint()
    {
        hintText.text = "Use WASD to move and mouse to look around\n" +
                        "Collect coins to increase your score\n" +
                        "Find the crystal to win the game";
        yield return new WaitForSeconds(5f); // Display the hint for 5 seconds
        hintText.text = ""; // Clear the hint text
    }

    /// <summary>
    /// Updates the player's score by the given amount.
    /// </summary>
    /// <param name="amount"></param>
    public void ModifyScore(int amount)
    {
        score += amount;
        Debug.Log("Score: " + score);
        scoreText.text = "Score: " + score.ToString();
    }

    /// <summary>
    /// Clears coin highlight when player exits the trigger. 
    /// </summary>
    /// <param name="other"></param>
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

    /// <summary>
    /// Displays a hint message onscreen for 2 seconds. 
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    System.Collections.IEnumerator DisplayHint(string message)
    {
        hintText.text = message;
        yield return new WaitForSeconds(2f); // Display the hint for 2 seconds
        hintText.text = ""; // Clear the hint text
    }

    /// <summary>
    /// Displays the win message when player collects the crystal. 
    /// </summary>
    public void Win()
    {
        StartCoroutine(DisplayHint("You win!"));
    }
}
