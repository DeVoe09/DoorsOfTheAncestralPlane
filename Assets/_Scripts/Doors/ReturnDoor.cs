using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnDoor : MonoBehaviour
{
    [Header("Return Door Settings")]
    public string targetScene = "AncestralPlane";
    public bool isLocked = false;
    
    [Header("Visual Settings")]
    public Material lockedMaterial;
    public Material unlockedMaterial;
    
    private Renderer doorRenderer;
    private AudioSource audioSource;
    
    private void Awake()
    {
        doorRenderer = GetComponent<Renderer>();
        audioSource = GetComponent<AudioSource>();
        
        if (doorRenderer == null)
        {
            Debug.LogError("ReturnDoor: No Renderer found on " + gameObject.name);
        }
        
        UpdateDoorAppearance();
    }
    
    private void UpdateDoorAppearance()
    {
        if (doorRenderer == null) return;
        
        if (isLocked)
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
        Debug.Log("ReturnDoor OnTriggerEnter: " + other.gameObject.name + " - Tag: " + other.tag);
        
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player detected at return door!");
            TryEnterDoor();
        }
    }
    
    private void TryEnterDoor()
    {
        if (isLocked)
        {
            Debug.Log("Return door is locked!");
            return;
        }
        
        // Return to Ancestral Plane
        Debug.Log("Returning to Ancestral Plane...");
        
        // Reset emotion to Neutral
        if (EmotionManager.Instance != null)
        {
            EmotionManager.Instance.SetEmotion(EmotionalState.Neutral);
        }
        
        if (audioSource != null)
        {
            // Play sound if available
        }
        
        // Load Ancestral Plane scene
        SceneManager.LoadScene(targetScene);
    }
    
    public void UnlockDoor()
    {
        isLocked = false;
        UpdateDoorAppearance();
        Debug.Log("Return door unlocked!");
    }
}
