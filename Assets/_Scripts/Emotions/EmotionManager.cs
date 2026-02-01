using UnityEngine;

public enum EmotionalState
{
    Neutral,
    Anger,
    Calm,
    Joy
}

public class EmotionManager : MonoBehaviour
{
    public static EmotionManager Instance { get; private set; }

    [Header("Current State")]
    public EmotionalState currentState = EmotionalState.Neutral;

    [Header("Emotion Effects")]
    [Tooltip("Anger: +40% speed, red tint, aggressive abilities")]
    public float angerSpeedMultiplier = 1.4f;
    
    [Tooltip("Calm: -30% speed, blue tint, slow-mo perception")]
    public float calmSpeedMultiplier = 0.7f;
    
    [Tooltip("Joy: +20% speed, yellow tint, double jump")]
    public float joySpeedMultiplier = 1.2f;

    [Header("Visual Effects")]
    public Material angerTint;
    public Material calmTint;
    public Material joyTint;
    public Material neutralTint;

    // Events
    public delegate void EmotionChanged(EmotionalState newState);
    public static event EmotionChanged OnEmotionChanged;

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

    public void SetEmotion(EmotionalState newState)
    {
        if (currentState == newState) return;

        currentState = newState;
        Debug.Log("Emotion changed to: " + newState);

        // Notify listeners
        OnEmotionChanged?.Invoke(newState);

        // Apply visual effects
        ApplyEmotionEffects(newState);
    }

    private void ApplyEmotionEffects(EmotionalState state)
    {
        // This would be applied to the player's camera/post-processing
        // For now, just log the effect
        switch (state)
        {
            case EmotionalState.Anger:
                Debug.Log("Anger Effects: +40% speed, red distortion, aggressive abilities");
                break;
            case EmotionalState.Calm:
                Debug.Log("Calm Effects: -30% speed, blue tint, slow-mo perception");
                break;
            case EmotionalState.Joy:
                Debug.Log("Joy Effects: +20% speed, yellow glow, double jump");
                break;
            case EmotionalState.Neutral:
                Debug.Log("Neutral Effects: Normal speed, clear perception");
                break;
        }
    }

    public float GetCurrentSpeedMultiplier()
    {
        switch (currentState)
        {
            case EmotionalState.Anger:
                return angerSpeedMultiplier;
            case EmotionalState.Calm:
                return calmSpeedMultiplier;
            case EmotionalState.Joy:
                return joySpeedMultiplier;
            case EmotionalState.Neutral:
            default:
                return 1.0f;
        }
    }

    public bool CanDoubleJump()
    {
        return currentState == EmotionalState.Joy;
    }

    public bool HasSlowMo()
    {
        return currentState == EmotionalState.Calm;
    }

    public bool HasDash()
    {
        return currentState == EmotionalState.Anger;
    }

    public EmotionalState GetCurrentState()
    {
        return currentState;
    }
}
