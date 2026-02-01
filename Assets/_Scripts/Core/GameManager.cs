using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game State")]
    public int currentRealm = 0; // 0 = Ancestral Plane, 1 = Anger, 2 = Calm, 3 = Joy
    public float balanceMeter = 50f; // 0-100, starts at 50
    public float sessionTime = 0f;

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
        if (realmIndex < 0 || realmIndex >= realmSceneNames.Length)
        {
            Debug.LogError("Invalid realm index: " + realmIndex);
            return;
        }

        currentRealm = realmIndex;
        SceneManager.LoadScene(realmSceneNames[realmIndex]);
        Debug.Log("Entering realm: " + realmSceneNames[realmIndex]);
    }

    public void UpdateBalance(float change)
    {
        balanceMeter += change;
        balanceMeter = Mathf.Clamp(balanceMeter, 0f, 100f);

        // Notify listeners
        OnBalanceChanged?.Invoke(balanceMeter);

        Debug.Log("Balance updated: " + balanceMeter + " (Change: " + change + ")");
    }

    public void CompleteRealm()
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
            // Completed an emotion realm
            Debug.Log("Completed realm " + currentRealm + ". Returning to Ancestral Plane.");
            EnterRealm(0);
        }
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
}
