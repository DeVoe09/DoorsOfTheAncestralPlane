using UnityEngine;

public class EmotionalEnvironment : MonoBehaviour
{
    [Header("Colors")]
    public Color angerColor = Color.red;
    public Color calmColor = Color.cyan;
    public Color joyColor = Color.yellow;
    public Color neutralColor = Color.white;

    [Header("Settings")]
    public float transitionSpeed = 2f;

    private Material targetMaterial; // Set this in Inspector for global effect
    private Color targetColor;

    void OnEnable()
    {
        EmotionManager.OnEmotionChanged += HandleEmotionChanged;
    }

    void OnDisable()
    {
        EmotionManager.OnEmotionChanged -= HandleEmotionChanged;
    }

    void HandleEmotionChanged(EmotionalState state)
    {
        switch (state)
        {
            case EmotionalState.Anger: targetColor = angerColor; break;
            case EmotionalState.Calm: targetColor = calmColor; break;
            case EmotionalState.Joy: targetColor = joyColor; break;
            default: targetColor = neutralColor; break;
        }
    }

    void Update()
    {
        // Example: If you have a global shader or fog, lerp it here
        RenderSettings.fogColor = Color.Lerp(RenderSettings.fogColor, targetColor, Time.deltaTime * transitionSpeed);
    }
}
