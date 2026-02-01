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

        // Only change material if the material fields are assigned
        // Otherwise, keep the existing material color
        if (targetRealmIndex == 0 && GameManager.Instance != null && GameManager.Instance.balanceMeter >= 75f)
        {
            // White door for ending
            if (whiteDoorMaterial != null)
            {
                doorRenderer.material = whiteDoorMaterial;
            }
            isLocked = false;
        }
        else if (isLocked)
        {
            if (lockedMaterial != null)
            {
                doorRenderer.material = lockedMaterial;
            }
        }
        else
        {
            if (unlockedMaterial != null)
            {
                doorRenderer.material = unlockedMaterial;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter: " + other.gameObject.name + " - Tag: " + other.tag);
        
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player detected! Trying to enter door...");
            TryEnterDoor();
        }
    }

    private void TryEnterDoor()
    {
        if (isLocked)
        {
            Debug.Log(doorName + " is locked. Need balance: " + unlockBalanceThreshold + ". Current balance: " + (GameManager.Instance != null ? GameManager.Instance.balanceMeter.ToString() : "N/A"));
            
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

        // Set emotion based on target realm
        if (EmotionManager.Instance != null)
        {
            switch (targetRealmIndex)
            {
                case 1: // Anger Realm
                    EmotionManager.Instance.SetEmotion(EmotionalState.Anger);
                    break;
                case 2: // Calm Realm
                    EmotionManager.Instance.SetEmotion(EmotionalState.Calm);
                    break;
                case 3: // Joy Realm
                    EmotionManager.Instance.SetEmotion(EmotionalState.Joy);
                    break;
                default: // Ancestral Plane (0) or other
                    EmotionManager.Instance.SetEmotion(EmotionalState.Neutral);
                    break;
            }
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
