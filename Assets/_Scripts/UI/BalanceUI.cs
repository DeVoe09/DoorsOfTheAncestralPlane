using UnityEngine;
using UnityEngine.UI;

public class BalanceUI : MonoBehaviour
{
    [Header("UI References")]
    public Slider balanceSlider;
    public Text balanceText;
    public Text realmText;
    public Text emotionText;
    public Text timerText;

    [Header("Colors")]
    public Color chaosColor = Color.red;
    public Color balancedColor = Color.yellow;
    public Color clarityColor = Color.green;

    private void OnEnable()
    {
        // Subscribe to events
        if (GameManager.Instance != null)
        {
            GameManager.OnBalanceChanged += UpdateBalanceDisplay;
        }

        if (EmotionManager.Instance != null)
        {
            EmotionManager.OnEmotionChanged += UpdateEmotionDisplay;
        }
    }

    private void OnDisable()
    {
        // Unsubscribe from events
        if (GameManager.Instance != null)
        {
            GameManager.OnBalanceChanged -= UpdateBalanceDisplay;
        }

        if (EmotionManager.Instance != null)
        {
            EmotionManager.OnEmotionChanged -= UpdateEmotionDisplay;
        }
    }

    private void Start()
    {
        UpdateAllDisplays();
    }

    private void Update()
    {
        UpdateTimerDisplay();
    }

    private void UpdateAllDisplays()
    {
        if (GameManager.Instance != null)
        {
            UpdateBalanceDisplay(GameManager.Instance.balanceMeter);
            UpdateRealmDisplay();
        }

        if (EmotionManager.Instance != null)
        {
            UpdateEmotionDisplay(EmotionManager.Instance.GetCurrentState());
        }
    }

    private void UpdateBalanceDisplay(float newBalance)
    {
        if (balanceSlider != null)
        {
            balanceSlider.value = newBalance / 100f;
        }

        if (balanceText != null)
        {
            balanceText.text = "Balance: " + Mathf.RoundToInt(newBalance) + "%";
        }

        // Update color based on balance
        if (balanceSlider != null && balanceSlider.fillRect != null)
        {
            Image fillImage = balanceSlider.fillRect.GetComponent<Image>();
            if (fillImage != null)
            {
                if (newBalance < 25f)
                {
                    fillImage.color = chaosColor;
                }
                else if (newBalance > 75f)
                {
                    fillImage.color = clarityColor;
                }
                else
                {
                    fillImage.color = balancedColor;
                }
            }
        }
    }

    private void UpdateRealmDisplay()
    {
        if (realmText != null && GameManager.Instance != null)
        {
            realmText.text = "Realm: " + GameManager.Instance.GetCurrentRealmName();
        }
    }

    private void UpdateEmotionDisplay(EmotionalState newState)
    {
        if (emotionText != null)
        {
            emotionText.text = "Emotion: " + newState.ToString();
        }
    }

    private void UpdateTimerDisplay()
    {
        if (timerText != null && GameManager.Instance != null)
        {
            float minutes = GameManager.Instance.GetSessionTimeMinutes();
            int mins = Mathf.FloorToInt(minutes);
            int secs = Mathf.FloorToInt((minutes - mins) * 60);
            timerText.text = string.Format("Time: {0:00}:{1:00}", mins, secs);
        }
    }

    // Public method for manual updates
    public void RefreshUI()
    {
        UpdateAllDisplays();
    }
}
