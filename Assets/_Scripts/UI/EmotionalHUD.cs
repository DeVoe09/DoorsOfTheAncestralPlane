using UnityEngine;
using UnityEngine.UI;

public class EmotionalHUD : MonoBehaviour
{
    [Header("UI References")]
    public Text stateText;
    public Slider healthSlider;
    public Slider balanceSlider;
    public Image balanceFillImage;

    [Header("Colors")]
    public Color angerColor = Color.red;
    public Color calmColor = Color.cyan;
    public Color joyColor = Color.yellow;
    public Color neutralColor = Color.white;

    private EmotionManager emotionManager;
    private EmotionalCombatant combatant;

    void Start()
    {
        // Find player components
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            emotionManager = player.GetComponentInChildren<EmotionManager>();
            combatant = player.GetComponentInChildren<EmotionalCombatant>();
        }
    }

    void Update()
    {
        if (emotionManager != null)
        {
            // Update State Text and Color
            if (stateText != null)
            {
                stateText.text = "EMOTION: " + emotionManager.currentState.ToString().ToUpper();
                stateText.color = GetColorForState(emotionManager.currentState);
            }

            // Update Balance Slider (-1 to 1)
            if (balanceSlider != null)
            {
                balanceSlider.value = emotionManager.balanceMeter;
                if (balanceFillImage != null)
                {
                    balanceFillImage.color = GetColorForState(emotionManager.currentState);
                }
            }
        }

        // Update Health Slider
        if (combatant != null && healthSlider != null)
        {
            healthSlider.value = combatant.health / 100f; // Assuming 100 is max health
        }
    }

    Color GetColorForState(EmotionalState state)
    {
        switch (state)
        {
            case EmotionalState.Anger: return angerColor;
            case EmotionalState.Calm: return calmColor;
            case EmotionalState.Joy: return joyColor;
            default: return neutralColor;
        }
    }
}
