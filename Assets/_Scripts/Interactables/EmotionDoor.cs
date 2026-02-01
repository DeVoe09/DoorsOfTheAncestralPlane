using UnityEngine;
using UnityEngine.UI;

namespace DoorsOfTheAncestralPlane
{
    public class EmotionDoor : MonoBehaviour
    {
        [Header("Door Settings")]
        [SerializeField] private EmotionalState doorEmotion;
        [SerializeField] private bool isWhiteDoor = false;
        
        [Header("Visual Settings")]
        [SerializeField] private Material doorMaterial;
        [SerializeField] private Color doorColor;
        [SerializeField] private float glowIntensity = 2f;
        [SerializeField] private ParticleSystem entranceParticles;
        
        [Header("Audio")]
        [SerializeField] private AudioClip enterSound;
        [SerializeField] private AudioClip lockedSound;
        
        [Header("UI")]
        [SerializeField] private GameObject interactionPrompt;
        [SerializeField] private Text emotionText;
        
        private Renderer doorRenderer;
        private AudioSource audioSource;
        private bool playerInRange = false;
        private bool isUnlocked = false;

        private void Awake()
        {
            doorRenderer = GetComponent<Renderer>();
            audioSource = GetComponent<AudioSource>();
            
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }

            if (interactionPrompt != null)
            {
                interactionPrompt.SetActive(false);
            }
        }

        private void Start()
        {
            InitializeDoor();
        }

        private void InitializeDoor()
        {
            // Set door color based on emotion
            if (!isWhiteDoor)
            {
                doorColor = GetEmotionColor(doorEmotion);
            }
            else
            {
                doorColor = Color.white;
            }

            // Apply material
            if (doorRenderer != null)
            {
                if (doorMaterial != null)
                {
                    doorRenderer.material = doorMaterial;
                }
                doorRenderer.material.color = doorColor;
                doorRenderer.material.EnableKeyword("_EMISSION");
                doorRenderer.material.SetColor("_EmissionColor", doorColor * glowIntensity);
            }

            // Update UI text
            if (emotionText != null)
            {
                emotionText.text = isWhiteDoor ? "White Door" : doorEmotion.ToString();
                emotionText.color = doorColor;
            }

            // Check if door should be unlocked
            CheckUnlockStatus();
        }

        private void Update()
        {
            if (playerInRange && Input.GetKeyDown(KeyCode.E))
            {
                TryEnterDoor();
            }

            // Pulsing glow effect
            if (doorRenderer != null)
            {
                float pulse = Mathf.Sin(Time.time * 2f) * 0.5f + 1f;
                doorRenderer.material.SetColor("_EmissionColor", doorColor * (glowIntensity * pulse));
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerInRange = true;
                ShowInteractionPrompt();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerInRange = false;
                HideInteractionPrompt();
            }
        }

        private void TryEnterDoor()
        {
            if (GameManager.Instance == null) return;

            if (isWhiteDoor)
            {
                if (GameManager.Instance.CanEnterWhiteDoor())
                {
                    EnterWhiteDoor();
                }
                else
                {
                    PlayLockedSound();
                    Debug.Log("White door is locked. Complete all realms with high balance.");
                }
                return;
            }

            // Check if player is already in a realm
            if (GameManager.Instance.isPlayerInRealm)
            {
                Debug.Log("Already in a realm. Complete current realm first.");
                return;
            }

            // Enter the emotion realm
            GameManager.Instance.EnterRealm(doorEmotion);
            
            // Visual feedback
            if (entranceParticles != null)
            {
                entranceParticles.Play();
            }

            // Play enter sound
            if (enterSound != null)
            {
                audioSource.PlayOneShot(enterSound);
            }

            // Hide interaction prompt
            HideInteractionPrompt();

            // Optional: Teleport player to realm start point
            // TeleportToRealmStart();
        }

        private void EnterWhiteDoor()
        {
            // Trigger ending
            Debug.Log("=== ENTERING WHITE DOOR ===");
            
            if (entranceParticles != null)
            {
                entranceParticles.Play();
            }

            if (enterSound != null)
            {
                audioSource.PlayOneShot(enterSound);
            }

            // Trigger ending sequence
            StartCoroutine(GameManager.Instance.TriggerEnding(true));
        }

        private void CheckUnlockStatus()
        {
            if (isWhiteDoor) return;

            if (GameManager.Instance == null) return;

            // Check if this emotion realm is already completed
            bool completed = doorEmotion switch
            {
                EmotionalState.Anger => GameManager.Instance.angerRealmCompleted,
                EmotionalState.Calm => GameManager.Instance.calmRealmCompleted,
                EmotionalState.Joy => GameManager.Instance.joyRealmCompleted,
                _ => false
            };

            if (completed)
            {
                // Change appearance to show completed
                doorColor = Color.gray;
                if (doorRenderer != null)
                {
                    doorRenderer.material.color = doorColor;
                    doorRenderer.material.SetColor("_EmissionColor", Color.black);
                }
            }
        }

        private void ShowInteractionPrompt()
        {
            if (interactionPrompt != null)
            {
                interactionPrompt.SetActive(true);
            }

            if (emotionText != null)
            {
                emotionText.text = isWhiteDoor ? 
                    (GameManager.Instance.CanEnterWhiteDoor() ? "Enter White Door" : "Locked") : 
                    $"Press E to enter {doorEmotion}";
            }
        }

        private void HideInteractionPrompt()
        {
            if (interactionPrompt != null)
            {
                interactionPrompt.SetActive(false);
            }
        }

        private void PlayLockedSound()
        {
            if (lockedSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(lockedSound);
            }
        }

        private Color GetEmotionColor(EmotionalState emotion)
        {
            return emotion switch
            {
                EmotionalState.Anger => new Color(0.55f, 0f, 0f, 1f), // Deep Red
                EmotionalState.Calm => new Color(0f, 0.4f, 0.8f, 1f), // Cool Blue
                EmotionalState.Joy => new Color(1f, 0.84f, 0f, 1f),   // Warm Yellow
                _ => Color.white
            };
        }

        // Public method for external triggers
        public void UnlockDoor()
        {
            isUnlocked = true;
            doorColor = GetEmotionColor(doorEmotion);
            if (doorRenderer != null)
            {
                doorRenderer.material.color = doorColor;
                doorRenderer.material.SetColor("_EmissionColor", doorColor * glowIntensity);
            }
        }

        // For realm completion trigger
        public void CompleteRealm()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.CompleteRealm(true);
            }
        }
    }
}
