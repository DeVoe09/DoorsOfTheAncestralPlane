using UnityEngine;

public class SceneSetup : MonoBehaviour
{
    [Header("Setup Options")]
    public bool setupOnStart = true;
    public bool createPlayer = true;
    public bool createGameManager = true;
    public bool createEmotionManager = true;

    [Header("Player Settings")]
    public GameObject playerPrefab;
    public Vector3 playerSpawnPosition = Vector3.zero;

    [Header("Managers")]
    public GameObject gameManagerPrefab;
    public GameObject emotionManagerPrefab;

    private void Start()
    {
        if (setupOnStart)
        {
            SetupScene();
        }
    }

    [ContextMenu("Setup Scene")]
    public void SetupScene()
    {
        Debug.Log("Setting up scene...");

        // Create GameManager if needed
        if (createGameManager && GameManager.Instance == null)
        {
            if (gameManagerPrefab != null)
            {
                Instantiate(gameManagerPrefab);
            }
            else
            {
                GameObject gm = new GameObject("GameManager");
                gm.AddComponent<GameManager>();
            }
        }

        // Create EmotionManager if needed
        if (createEmotionManager && EmotionManager.Instance == null)
        {
            if (emotionManagerPrefab != null)
            {
                Instantiate(emotionManagerPrefab);
            }
            else
            {
                GameObject em = new GameObject("EmotionManager");
                em.AddComponent<EmotionManager>();
            }
        }

        // Create Player if needed
        if (createPlayer)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player == null)
            {
                if (playerPrefab != null)
                {
                    Instantiate(playerPrefab, playerSpawnPosition, Quaternion.identity);
                }
                else
                {
                    CreateDefaultPlayer();
                }
            }
        }

        Debug.Log("Scene setup complete!");
    }

    private void CreateDefaultPlayer()
    {
        GameObject player = new GameObject("Player");
        player.tag = "Player";
        player.transform.position = playerSpawnPosition;

        // Add CharacterController
        CharacterController controller = player.AddComponent<CharacterController>();
        controller.height = 2f;
        controller.radius = 0.5f;
        controller.center = new Vector3(0, 1, 0);

        // Add FirstPersonController
        player.AddComponent<FirstPersonController>();

        // Add Camera
        GameObject cameraObj = new GameObject("PlayerCamera");
        cameraObj.transform.SetParent(player.transform);
        cameraObj.transform.localPosition = new Vector3(0, 1.6f, 0); // Eye level
        Camera cam = cameraObj.AddComponent<Camera>();
        cam.tag = "MainCamera";

        Debug.Log("Default player created with CharacterController and FirstPersonController");
    }

    // Helper methods for manual setup
    [ContextMenu("Create Ancestral Plane Setup")]
    public void CreateAncestralPlaneSetup()
    {
        // Create spawn point
        GameObject spawnPoint = new GameObject("SpawnPoint");
        spawnPoint.transform.position = Vector3.zero;

        // Create doors
        CreateDoor("Anger Door", 1, new Vector3(5, 0, 0), Color.red);
        CreateDoor("Calm Door", 2, new Vector3(0, 0, 5), Color.blue);
        CreateDoor("Joy Door", 3, new Vector3(-5, 0, 0), Color.yellow);

        // Create white door (ending)
        CreateWhiteDoor(new Vector3(0, 0, -5));

        Debug.Log("Ancestral Plane setup complete!");
    }

    private void CreateDoor(string name, int targetRealm, Vector3 position, Color color)
    {
        GameObject door = GameObject.CreatePrimitive(PrimitiveType.Cube);
        door.name = name;
        door.transform.position = position;
        door.transform.localScale = new Vector3(2, 3, 0.5f);

        // Set color - Use a simple approach
        Renderer renderer = door.GetComponent<Renderer>();
        if (renderer != null)
        {
            // Just set the color directly on the shared material
            // This will be preserved across play mode
            renderer.sharedMaterial.color = color;
        }

        // Add collider
        BoxCollider collider = door.AddComponent<BoxCollider>();
        collider.isTrigger = true;

        // Add door controller
        DoorController doorController = door.AddComponent<DoorController>();
        doorController.targetRealmIndex = targetRealm;
        doorController.doorName = name;
        doorController.isLocked = false;
    }

    private void CreateWhiteDoor(Vector3 position)
    {
        GameObject door = GameObject.CreatePrimitive(PrimitiveType.Cube);
        door.name = "White Door (Ending)";
        door.transform.position = position;
        door.transform.localScale = new Vector3(2, 3, 0.5f);

        // Set white color - Use shared material
        Renderer renderer = door.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.sharedMaterial.color = Color.white;
        }

        // Add collider
        BoxCollider collider = door.AddComponent<BoxCollider>();
        collider.isTrigger = true;

        // Add door controller
        DoorController doorController = door.AddComponent<DoorController>();
        doorController.targetRealmIndex = 0; // Stays in Ancestral Plane
        doorController.doorName = "White Door (Ending)";
        doorController.isLocked = true;
        doorController.SetBalanceThreshold(75f);
    }
    
    [ContextMenu("Create Return Door")]
    public void CreateReturnDoor()
    {
        CreateReturnDoor(new Vector3(0, 1, -5));
    }
    
    private void CreateReturnDoor(Vector3 position)
    {
        GameObject door = GameObject.CreatePrimitive(PrimitiveType.Cube);
        door.name = "Return Door";
        door.transform.position = position;
        door.transform.localScale = new Vector3(2, 3, 0.5f);
        
        // Set blue color for return door
        Renderer renderer = door.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.sharedMaterial.color = new Color(0.3f, 0.5f, 1f); // Light blue
        }
        
        // Add collider as trigger
        BoxCollider collider = door.AddComponent<BoxCollider>();
        collider.isTrigger = true;
        
        // Add ReturnDoor component
        ReturnDoor returnDoor = door.AddComponent<ReturnDoor>();
        returnDoor.targetScene = "AncestralPlane";
        returnDoor.isLocked = false;
        
        Debug.Log("Created return door at " + position);
    }
}
