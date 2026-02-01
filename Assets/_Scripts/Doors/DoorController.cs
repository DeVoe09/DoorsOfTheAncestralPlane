using UnityEngine;

public class DoorController : MonoBehaviour
{
    [Header("Door Settings")]
    public int targetRealmIndex = 0; // 0 = Ancestral Plane, 1 = Anger, 2 = Calm, 3 = Joy
    public string doorName = "Door";
    public bool isLocked = false;
    public float unlockBalanceThreshold = 50f;

    [Header("Visual Feedback")]
    public Material lockedMaterial;
    public Material unlockedMaterial;
    public Material whiteDoorMaterial; // Special for ending

    [Header("Audio")]
    public AudioClip unlockSound;
    public AudioClip enterSound;

    private Renderer doorRenderer;
    private AudioSource audioSource;

    private void Awake()
    {
        doorRenderer = GetComponent<Renderer>();
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void Start()
    {
        UpdateDoorAppearance();
    }

    private void UpdateDoorAppearance()
    {
        if (doorRenderer == null) return;

        if (targetRealmIndex == 0 && GameManager.Instance != null && GameManager.Instance.balanceMeter >= 75f)
        {
            // White door for ending
            doorRenderer.material = whiteDoorMaterial;
            isLocked = false;
        }
        else if (isLocked)
        {
            doorRenderer.material = lockedMaterial;
        }
        else
        {
            doorRenderer.material = unlockedMaterial;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TryEnterDoor();
        }
    }

    private void TryEnterDoor()
    {
        if (isLocked)
        {
            Debug.Log(doorName + " is locked. Need balance: " + unlockBalanceThreshold);
            
            if (GameManager.Instance != null && GameManager.Instance.balanceMeter >= unlockBalanceThreshold)
            {
                UnlockDoor();
            }
            return;
        }

        // Enter the door
        Debug.Log("Entering " + doorName);
        
        if (audioSource != null && enterSound != null)
        {
            audioSource.PlayOneShot(enterSound);
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.EnterRealm(targetRealmIndex);
        }
    }

    public void UnlockDoor()
    {
        isLocked = false;
        UpdateDoorAppearance();
        
        if (audioSource != null && unlockSound != null)
        {
            audioSource.PlayOneShot(unlockSound);
        }

        Debug.Log(doorName + " unlocked!");
    }

    public void SetBalanceThreshold(float threshold)
    {
        unlockBalanceThreshold = threshold;
        UpdateDoorAppearance();
    }

    public void SetTargetRealm(int realmIndex)
    {
        targetRealmIndex = realmIndex;
        UpdateDoorAppearance();
    }
}
