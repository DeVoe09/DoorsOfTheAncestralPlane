using UnityEngine;
using UnityEngine.UI;

public class BalanceUI : MonoBehaviour
{
    [Header("UI References")]
    public Slider balanceSlider;
    public Text balanceText;
    public Text emotionText;
    
    [Header("Colors")]
    public Color lowBalanceColor = Color.red;
    public Color mediumBalanceColor = Color.yellow;
    public Color highBalanceColor = Color.green;
    
    private void Start()
    {
        // Find or create UI elements if not assigned
        if (balanceSlider == null)
        {
            balanceSlider = GetComponentInChildren<Slider>();
        }
        
        if (balanceText == null)
        {
            // Try to find a Text component
            Text[] texts = GetComponentsInChildren<Text>();
            foreach (Text t in texts)
            {
                if (t.name.Contains("Balance") || t.name.Contains("balance"))
                {
                    balanceText = t;
                    break;
                }
            }
        }
        
        // Subscribe to balance changes
        if (GameManager.Instance != null)
        {
            UpdateBalanceUI(GameManager.Instance.balanceMeter);
        }
        
        // Subscribe to emotion changes
        if (EmotionManager.Instance != null)
        {
            UpdateEmotionUI(EmotionManager.Instance.GetCurrentState());
        }
    }
    
    private void OnEnable()
    {
        // Subscribe to events
        GameManager.OnBalanceChanged += UpdateBalanceUI;
        EmotionManager.OnEmotionChanged += UpdateEmotionUI;
    }
    
    private void OnDisable()
    {
        // Unsubscribe from events
        GameManager.OnBalanceChanged -= UpdateBalanceUI;
        EmotionManager.OnEmotionChanged -= UpdateEmotionUI;
    }
    
    private void UpdateBalanceUI(float newBalance)
    {
        // Update slider
        if (balanceSlider != null)
        {
            balanceSlider.value = newBalance / 100f; // Normalize to 0-1 range
        }
        
        // Update text
        if (balanceText != null)
        {
            balanceText.text = "Balance: " + newBalance.ToString("F0") + "%";
            
            // Change color based on balance
            if (newBalance < 30)
            {
                balanceText.color = lowBalanceColor;
            }
            else if (newBalance < 70)
            {
                balanceText.color = mediumBalanceColor;
            }
            else
            {
                balanceText.color = highBalanceColor;
            }
        }
        
        Debug.Log("BalanceUI updated: " + newBalance + "%");
    }
    
    private void UpdateEmotionUI(EmotionalState newEmotion)
    {
        if (emotionText != null)
        {
            string emotionName = newEmotion.ToString();
            emotionText.text = "Emotion: " + emotionName;
            
            // Change color based on emotion
            switch (newEmotion)
            {
                case EmotionalState.Anger:
                    emotionText.color = Color.red;
                    break;
                case EmotionalState.Calm:
                    emotionText.color = Color.blue;
                    break;
                case EmotionalState.Joy:
                    emotionText.color = Color.yellow;
                    break;
                case EmotionalState.Neutral:
                    emotionText.color = Color.white;
                    break;
            }
        }
        
        Debug.Log("EmotionUI updated: " + newEmotion);
    }
}
