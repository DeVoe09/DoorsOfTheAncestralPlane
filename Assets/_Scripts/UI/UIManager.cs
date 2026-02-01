using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace DoorsOfTheAncestralPlane
{
    public class UIManager : MonoBehaviour
    {
        [Header("HUD Elements")]
        [SerializeField] private Image balanceMeter;
        [SerializeField] private TextMeshProUGUI balanceText;
        [SerializeField] private Image emotionIcon;
        [SerializeField] private TextMeshProUGUI objectiveText;
        [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] private GameObject lowBalanceWarning;

        [Header("Menu Panels")]
        [SerializeField] private GameObject mainMenuPanel;
        [SerializeField] private GameObject pauseMenuPanel;
        [SerializeField] private GameObject settingsPanel;
        [SerializeField] private GameObject endingPanel;

        [Header("Settings UI")]
        [SerializeField] private Slider masterVolumeSlider;
        [SerializeField] private Slider mouseSensitivitySlider;
        [SerializeField] private Dropdown resolutionDropdown;
        [SerializeField] private Toggle fullscreenToggle;

        [Header("Emotion Icons")]
        [SerializeField] private Sprite neutralIcon;
        [SerializeField] private Sprite angerIcon;
        [SerializeField] private Sprite calmIcon;
        [SerializeField] private Sprite joyIcon;

        private bool isPaused = false;
        private float timer = 0f;

        private void OnEnable()
        {
            GameManager.OnBalanceChanged += UpdateBalanceMeter;
            GameManager.OnEmotionChanged += UpdateEmotionIcon;
            GameManager.OnObjectiveUpdated += UpdateObjectiveText;
        }

        private void OnDisable()
        {
            GameManager.OnBalanceChanged -= UpdateBalanceMeter;
            GameManager.OnEmotionChanged -= UpdateEmotionIcon;
            GameManager.OnObjectiveUpdated -= UpdateObjectiveText;
        }

        private void Start()
        {
            // Initialize UI
            UpdateBalanceMeter(50f);
            UpdateEmotionIcon(EmotionalState.Neutral);
            UpdateObjectiveText("Enter the Ancestral Plane");

            // Hide all menus at start
            HideAllMenus();
            mainMenuPanel.SetActive(true);

            // Initialize settings
            InitializeSettings();
        }

        private void Update()
        {
            // Update timer if in realm
            if (GameManager.Instance != null && GameManager.Instance.isPlayerInRealm)
            {
                timer = GameManager.Instance.GetRealmTimeRemaining();
                UpdateTimer(timer);
            }
            else
            {
                if (timerText != null)
                {
                    timerText.text = "";
                }
            }

            // Pause menu toggle
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                TogglePause();
            }
        }

        private void UpdateBalanceMeter(float balance)
        {
            if (balanceMeter != null)
            {
                balanceMeter.fillAmount = balance / 100f;

                // Color gradient based on balance
                if (balance < 25f)
                {
                    balanceMeter.color = Color.red;
                    if (lowBalanceWarning != null) lowBalanceWarning.SetActive(true);
                }
                else if (balance < 75f)
                {
                    balanceMeter.color = Color.yellow;
                    if (lowBalanceWarning != null) lowBalanceWarning.SetActive(false);
                }
                else
                {
                    balanceMeter.color = Color.green;
                    if (lowBalanceWarning != null) lowBalanceWarning.SetActive(false);
                }
            }

            if (balanceText != null)
            {
                balanceText.text = $"Balance: {balance:F0}%";
                balanceText.color = balance < 25f ? Color.red : (balance < 75f ? Color.yellow : Color.green);
            }
        }

        private void UpdateEmotionIcon(EmotionalState emotion)
        {
            if (emotionIcon == null) return;

            Sprite icon = emotion switch
            {
                EmotionalState.Neutral => neutralIcon,
                EmotionalState.Anger => angerIcon,
                EmotionalState.Calm => calmIcon,
                EmotionalState.Joy => joyIcon,
                _ => neutralIcon
            };

            emotionIcon.sprite = icon;

            // Color tint
            Color tint = emotion switch
            {
                EmotionalState.Anger => new Color(0.55f, 0f, 0f, 1f),
                EmotionalState.Calm => new Color(0f, 0.4f, 0.8f, 1f),
                EmotionalState.Joy => new Color(1f, 0.84f, 0f, 1f),
                _ => Color.white
            };

            emotionIcon.color = tint;
        }

        private void UpdateObjectiveText(string objective)
        {
            if (objectiveText != null)
            {
                objectiveText.text = objective;
            }
        }

        private void UpdateTimer(float timeRemaining)
        {
            if (timerText == null) return;

            if (timeRemaining > 0)
            {
                int minutes = Mathf.FloorToInt(timeRemaining / 60);
                int seconds = Mathf.FloorToInt(timeRemaining % 60);
                timerText.text = $"Time: {minutes:00}:{seconds:00}";

                // Warning color if low time
                if (timeRemaining < 30f)
                {
                    timerText.color = Color.red;
                }
                else
                {
                    timerText.color = Color.white;
                }
            }
            else
            {
                timerText.text = "Time's Up!";
                timerText.color = Color.red;
            }
        }

        // Menu Management
        private void HideAllMenus()
        {
            if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
            if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
            if (settingsPanel != null) settingsPanel.SetActive(false);
            if (endingPanel != null) endingPanel.SetActive(false);
        }

        public void ShowMainMenu()
        {
            HideAllMenus();
            if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
            Time.timeScale = 0f;
            isPaused = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void StartGame()
        {
            HideAllMenus();
            Time.timeScale = 1f;
            isPaused = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            // Reset game state if needed
            if (GameManager.Instance != null)
            {
                // Could reset here
            }
        }

        public void TogglePause()
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

        private void PauseGame()
        {
            isPaused = true;
            Time.timeScale = 0f;
            HideAllMenus();
            if (pauseMenuPanel != null) pauseMenuPanel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        private void ResumeGame()
        {
            isPaused = false;
            Time.timeScale = 1f;
            HideAllMenus();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        public void ShowSettings()
        {
            HideAllMenus();
            if (settingsPanel != null) settingsPanel.SetActive(true);
        }

        public void BackToMenu()
        {
            ShowMainMenu();
        }

        public void QuitGame()
        {
            Debug.Log("Quitting game...");
            Application.Quit();
        }

        // Settings
        private void InitializeSettings()
        {
            // Volume
            if (masterVolumeSlider != null)
            {
                masterVolumeSlider.value = AudioListener.volume;
                masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
            }

            // Mouse Sensitivity
            if (mouseSensitivitySlider != null)
            {
                mouseSensitivitySlider.value = 2f; // Default
                mouseSensitivitySlider.onValueChanged.AddListener(SetMouseSensitivity);
            }

            // Resolution
            if (resolutionDropdown != null)
            {
                resolutionDropdown.ClearOptions();
                resolutionDropdown.AddOptions(GetAvailableResolutions());
                resolutionDropdown.onValueChanged.AddListener(SetResolution);
            }

            // Fullscreen
            if (fullscreenToggle != null)
            {
                fullscreenToggle.isOn = Screen.fullScreen;
                fullscreenToggle.onValueChanged.AddListener(SetFullscreen);
            }
        }

        private void SetMasterVolume(float volume)
        {
            AudioListener.volume = volume;
        }

        private void SetMouseSensitivity(float sensitivity)
        {
            // Find player controller and update sensitivity
            var player = FindObjectOfType<FirstPersonController>();
            if (player != null)
            {
                player.SetMouseSensitivity(sensitivity);
            }
        }

        private void SetResolution(int index)
        {
            // Resolution handling would go here
            // For now, just log
            Debug.Log($"Resolution changed to index: {index}");
        }

        private void SetFullscreen(bool fullscreen)
        {
            Screen.fullScreen = fullscreen;
        }

        private System.Collections.Generic.List<string> GetAvailableResolutions()
        {
            var resolutions = new System.Collections.Generic.List<string>();
            resolutions.Add("1920x1080");
            resolutions.Add("1280x720");
            resolutions.Add("1366x768");
            resolutions.Add("1600x900");
            return resolutions;
        }

        // Ending Screen
        public void ShowEnding(bool highBalance)
        {
            HideAllMenus();
            if (endingPanel != null)
            {
                endingPanel.SetActive(true);
                
                // Update ending text based on balance
                var endingText = endingPanel.GetComponentInChildren<TextMeshProUGUI>();
                if (endingText != null)
                {
                    if (highBalance)
                    {
                        endingText.text = "The steady mind sees truth.\n\nYou have mastered your emotions and found balance. The white door opens to peace.";
                        endingText.color = Color.white;
                    }
                    else
                    {
                        endingText.text = "You have witnessed.\n\nThe journey is complete. You have learned about the nature of emotion.";
                        endingText.color = Color.yellow;
                    }
                }
            }

            Time.timeScale = 0f;
            isPaused = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        // Public helper methods
        public bool IsPaused()
        {
            return isPaused;
        }

        public void HideEnding()
        {
            if (endingPanel != null)
            {
                endingPanel.SetActive(false);
            }
        }
    }
}
