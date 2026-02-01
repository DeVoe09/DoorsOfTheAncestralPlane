using UnityEngine;

namespace DoorsOfTheAncestralPlane
{
    public class RealmObjective : MonoBehaviour
    {
        [Header("Objective Settings")]
        [SerializeField] private EmotionalState requiredEmotion;
        [SerializeField] private ObjectiveType objectiveType;
        [SerializeField] private bool isFinalObjective = false;

        [Header("Visual Settings")]
        [SerializeField] private Material objectiveMaterial;
        [SerializeField] private Color objectiveColor;
        [SerializeField] private float pulseSpeed = 2f;
        [SerializeField] private ParticleSystem completionParticles;

        [Header("Audio")]
        [SerializeField] private AudioClip completionSound;
        [SerializeField] private AudioClip wrongEmotionSound;

        private Renderer objectiveRenderer;
        private AudioSource audioSource;
        private bool isCompleted = false;
        private bool playerInRange = false;

        private void Awake()
        {
            objectiveRenderer = GetComponent<Renderer>();
            audioSource = GetComponent<AudioSource>();
            
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }

        private void Start()
        {
            InitializeObjective();
        }

        private void InitializeObjective()
        {
            // Set color based on required emotion
            objectiveColor = GetEmotionColor(requiredEmotion);

            // Apply material
            if (objectiveRenderer != null)
            {
                if (objectiveMaterial != null)
                {
                    objectiveRenderer.material = objectiveMaterial;
                }
                objectiveRenderer.material.color = objectiveColor;
                objectiveRenderer.material.EnableKeyword("_EMISSION");
                objectiveRenderer.material.SetColor("_EmissionColor", objectiveColor * 0.5f);
            }

            // Set tag based on objective type
            switch (objectiveType)
            {
                case ObjectiveType.Destroyable:
                    gameObject.tag = "Destroyable";
                    break;
                case ObjectiveType.Interactable:
                    gameObject.tag = "Interactable";
                    break;
                case ObjectiveType.Platform:
                    gameObject.tag = "Platform";
                    break;
                case ObjectiveType.Checkpoint:
                    gameObject.tag = "Checkpoint";
                    break;
            }
        }

        private void Update()
        {
            if (isCompleted) return;

            // Pulsing effect
            if (objectiveRenderer != null)
            {
                float pulse = Mathf.Sin(Time.time * pulseSpeed) * 0.3f + 0.7f;
                objectiveRenderer.material.SetColor("_EmissionColor", objectiveColor * pulse);
            }

            // Check for player interaction
            if (playerInRange && Input.GetKeyDown(KeyCode.E))
            {
                TryCompleteObjective();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && !isCompleted)
            {
                playerInRange = true;

                // Auto-complete for certain objective types
                if (objectiveType == ObjectiveType.Checkpoint || 
                    objectiveType == ObjectiveType.Platform)
                {
                    TryCompleteObjective();
                }
                else
                {
                    // Show interaction prompt
                    ShowPrompt();
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerInRange = false;
                HidePrompt();
            }
        }

        private void TryCompleteObjective()
        {
            if (GameManager.Instance == null) return;

            // Check if player is in the correct emotion
            if (GameManager.Instance.currentEmotion != requiredEmotion && requiredEmotion != EmotionalState.Neutral)
            {
                PlayWrongEmotionSound();
                Debug.Log($"Wrong emotion! Required: {requiredEmotion}, Current: {GameManager.Instance.currentEmotion}");
                return;
            }

            // Complete the objective
            CompleteObjective();
        }

        private void CompleteObjective()
        {
            if (isCompleted) return;

            isCompleted = true;

            // Visual feedback
            if (completionParticles != null)
            {
                completionParticles.Play();
            }

            // Audio feedback
            if (completionSound != null)
            {
                audioSource.PlayOneShot(completionSound);
            }

            // Record mindful action
            if (GameManager.Instance != null)
            {
                GameManager.Instance.RecordMindfulAction();
            }

            // If this is the final objective, complete the realm
            if (isFinalObjective && GameManager.Instance != null)
            {
                GameManager.Instance.CompleteRealm(true);
            }

            // Hide prompt
            HidePrompt();

            // Optional: Disable or change appearance
            if (objectiveRenderer != null)
            {
                objectiveRenderer.material.SetColor("_EmissionColor", Color.green * 0.5f);
            }

            Debug.Log($"Objective completed: {objectiveType}");
        }

        private void ShowPrompt()
        {
            // Could show UI prompt here
            Debug.Log($"Press E to interact with {objectiveType}");
        }

        private void HidePrompt()
        {
            // Hide UI prompt
        }

        private void PlayWrongEmotionSound()
        {
            if (wrongEmotionSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(wrongEmotionSound);
            }
        }

        private Color GetEmotionColor(EmotionalState emotion)
        {
            return emotion switch
            {
                EmotionalState.Anger => new Color(0.55f, 0f, 0f, 1f), // Deep Red
                EmotionalState.Calm => new Color(0f, 0.4f, 0.8f, 1f), // Cool Blue
                EmotionalState.Joy => new Color(1f, 0.84f, 0f, 1f),   // Warm Yellow
                _ => Color.white // Neutral
            };
        }

        // Public methods for external triggers
        public void DestroyObjective()
        {
            if (objectiveType != ObjectiveType.Destroyable)
            {
                Debug.LogWarning("Objective is not destroyable!");
                return;
            }

            CompleteObjective();

            // Destroy the object
            Destroy(gameObject, 0.5f);
        }

        public void ForceComplete()
        {
            CompleteObjective();
        }

        // For emotion-specific interactions
        public bool CanInteractWithEmotion(EmotionalState currentEmotion)
        {
            return currentEmotion == requiredEmotion || requiredEmotion == EmotionalState.Neutral;
        }
    }

    public enum ObjectiveType
    {
        Destroyable,    // Anger: Break objects
        Interactable,   // Calm: Pattern matching
        Platform,       // Joy: Platforming
        Checkpoint,     // Progress marker
        Collectible     // Optional runes
    }
}
