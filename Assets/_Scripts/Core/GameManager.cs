using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game State")]
    public int currentRealm = 0; // 0 = Ancestral Plane, 1 = Anger, 2 = Calm, 3 = Joy
    public float balanceMeter = 50f; // 0-100, starts at 50
    public float sessionTime = 0f;
    public bool isPlayerInRealm = false;

    [Header("Realm Completion")]
    public bool angerRealmCompleted = false;
    public bool calmRealmCompleted = false;
    public bool joyRealmCompleted = false;

    [Header("Current Emotion")]
    public EmotionalState currentEmotion = EmotionalState.Neutral;

    // Events
    public delegate void BalanceChanged(float newBalance);
    public static event BalanceChanged OnBalanceChanged;

    [Header("Scene References")]
    public string[] realmSceneNames = { "AncestralPlane", "AngerRealm", "CalmRealm", "JoyRealm" };

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("GameManager initialized. Balance: " + balanceMeter);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        sessionTime += Time.deltaTime;
    }

    public void EnterRealm(int realmIndex)
    {
        Debug.Log("EnterRealm called with index: " + realmIndex);
        
        if (realmIndex < 0 || realmIndex >= realmSceneNames.Length)
        {
            Debug.LogError("Invalid realm index: " + realmIndex + ". Available scenes: " + string.Join(", ", realmSceneNames));
            return;
        }

        string sceneName = realmSceneNames[realmIndex];
        Debug.Log("Attempting to load scene: " + sceneName);
        
        // Check if scene exists
        if (Application.CanStreamedLevelBeLoaded(sceneName))
        {
            Debug.Log("Scene " + sceneName + " can be loaded");
        }
        else
        {
            Debug.LogWarning("Scene " + sceneName + " might not exist or not be added to build settings");
        }

        currentRealm = realmIndex;
        isPlayerInRealm = (realmIndex > 0); // 0 = Ancestral Plane, >0 = emotion realm
        SceneManager.LoadScene(sceneName);
        Debug.Log("Loading scene: " + sceneName);
    }

    public void EnterRealm(EmotionalState emotion)
    {
        int realmIndex = 0;
        switch (emotion)
        {
            case EmotionalState.Anger:
                realmIndex = 1;
                break;
            case EmotionalState.Calm:
                realmIndex = 2;
                break;
            case EmotionalState.Joy:
                realmIndex = 3;
                break;
            default:
                realmIndex = 0;
                break;
        }
        EnterRealm(realmIndex);
    }

    public void UpdateBalance(float change)
    {
        balanceMeter += change;
        balanceMeter = Mathf.Clamp(balanceMeter, 0f, 100f);

        // Notify listeners
        OnBalanceChanged?.Invoke(balanceMeter);

        Debug.Log("Balance updated: " + balanceMeter + " (Change: " + change + ")");
    }

    public void CompleteRealm(bool success = true)
    {
        if (currentRealm == 0)
        {
            // In Ancestral Plane, check if we should unlock the white door
            if (balanceMeter >= 75f)
            {
                Debug.Log("White Door Unlocked! Balance is high enough for clarity.");
                // Trigger ending sequence
            }
            else
            {
                Debug.Log("Balance too low for white door. Explore more realms.");
            }
        }
        else
        {
            // Mark realm as completed
            if (success)
            {
                switch (currentRealm)
                {
                    case 1:
                        angerRealmCompleted = true;
                        break;
                    case 2:
                        calmRealmCompleted = true;
                        break;
                    case 3:
                        joyRealmCompleted = true;
                        break;
                }
                Debug.Log("Completed realm " + currentRealm + ". Returning to Ancestral Plane.");
            }
            else
            {
                Debug.Log("Failed realm " + currentRealm + ". Returning to Ancestral Plane.");
            }
            
            // Return to Ancestral Plane
            EnterRealm(0);
        }
    }

    public bool CanEnterWhiteDoor()
    {
        return balanceMeter >= 75f;
    }

    public void RecordMindfulAction()
    {
        Debug.Log("RecordMindfulAction called");
        UpdateBalance(5f); // Increase balance for mindful actions
    }

    public void RecordChaoticAction()
    {
        Debug.Log("RecordChaoticAction called");
        UpdateBalance(-5f); // Decrease balance for chaotic actions
    }

    public string GetCurrentRealmName()
    {
        if (currentRealm >= 0 && currentRealm < realmSceneNames.Length)
        {
            return realmSceneNames[currentRealm];
        }
        return "Unknown";
    }

    public float GetSessionTimeMinutes()
    {
        return sessionTime / 60f;
    }

    public bool AreAllRealmsCompleted()
    {
        return angerRealmCompleted && calmRealmCompleted && joyRealmCompleted;
    }

    public IEnumerator TriggerEnding(bool isWhiteDoorEnding)
    {
        // Placeholder for ending sequence
        Debug.Log("=== ENDING SEQUENCE ===");
        yield return new WaitForSeconds(2f);
        Debug.Log("Game Complete!");
    }
}
