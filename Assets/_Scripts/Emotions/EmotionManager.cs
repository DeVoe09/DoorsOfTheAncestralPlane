using UnityEngine;
using System;

public enum EmotionalState
{
    Neutral,
    Anger,
    Calm,
    Joy
}

public class EmotionManager : MonoBehaviour
{
    [Header("Emotion State")]
    public EmotionalState currentState = EmotionalState.Neutral;
    [Range(-1f, 1f)] public float balanceMeter = 0f;

    [Header("Events")]
    public static Action<EmotionalState> OnEmotionChanged;
    public static Action<float> OnBalanceChanged;

    public void SetEmotion(EmotionalState newState)
    {
        if (currentState != newState)
        {
            currentState = newState;
            OnEmotionChanged?.Invoke(newState);
            Debug.Log($"Emotion changed to: {newState}");
        }
    }

    public void UpdateBalance(float delta)
    {
        balanceMeter = Mathf.Clamp(balanceMeter + delta, -1f, 1f);
        OnBalanceChanged?.Invoke(balanceMeter);
    }

    public EmotionalState GetCurrentState()
    {
        return currentState;
    }
}
