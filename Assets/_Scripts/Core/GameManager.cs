using UnityEngine; 
using System; 

public class GameManager : MonoBehaviour 
{ 
    public static GameManager Instance; 

    [Header("Game State")] 
    public bool gameStarted = false; 
    public float sessionTime = 0f; 
    public bool[] realmsCompleted = new bool[3]; 

    [Header("References")] 
    public EmotionManager emotionManager; 

    void Awake() 
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

    void Update() 
    { 
        if (gameStarted) 
        { 
            sessionTime += Time.deltaTime; 
        } 
    } 

    public void StartGame() 
    { 
        gameStarted = true; 
        sessionTime = 0f; 
        Debug.Log("Game Started - Doors of the Ancestral Plane"); 
    } 

    public void CompleteRealm(int realmIndex) 
    { 
        realmsCompleted[realmIndex] = true; 
        Debug.Log($"Realm {realmIndex} completed!"); 
    } 
}
