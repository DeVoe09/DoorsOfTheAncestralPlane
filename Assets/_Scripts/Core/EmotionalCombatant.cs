using UnityEngine;

public class EmotionalCombatant : MonoBehaviour
{
    [Header("Combat Stats")]
    public float health = 100f;
    public float emotionalResilience = 50f; // Resistance to emotional shifts
    
    [Header("Current Power")]
    public float attackMultiplier = 1f;
    public float defenseMultiplier = 1f;

    private EmotionManager emotionManager;

    void Start()
    {
        emotionManager = GetComponent<EmotionManager>();
        if (emotionManager == null)
        {
            emotionManager = GetComponentInChildren<EmotionManager>();
        }
    }

    void Update()
    {
        if (emotionManager != null)
        {
            ApplyEmotionalModifiers(emotionManager.currentState);
        }
    }

    void ApplyEmotionalModifiers(EmotionalState state)
    {
        switch (state)
        {
            case EmotionalState.Anger:
                attackMultiplier = 1.5f;
                defenseMultiplier = 0.5f; // Glass cannon
                break;
            case EmotionalState.Calm:
                attackMultiplier = 0.8f;
                defenseMultiplier = 1.5f; // Tanky
                break;
            case EmotionalState.Joy:
                attackMultiplier = 1.1f;
                defenseMultiplier = 1.1f; // Balanced buff
                break;
            default:
                attackMultiplier = 1.0f;
                defenseMultiplier = 1.0f;
                break;
        }
    }

    public void TakeDamage(float amount, EmotionalState damageType)
    {
        // Damage is mitigated by defense multiplier
        float finalDamage = amount / defenseMultiplier;
        
        // If damage type matches a "weakness" based on current state, increase it
        if (IsWeakTo(emotionManager.currentState, damageType))
        {
            finalDamage *= 1.5f;
            Debug.Log("Critical Emotional Hit!");
        }

        health -= finalDamage;
        
        // Damage also affects the balance meter
        if (emotionManager != null)
        {
            float balanceShift = (damageType == EmotionalState.Anger) ? -0.1f : 0.05f;
            emotionManager.UpdateBalance(balanceShift);
        }

        if (health <= 0) Die();
    }

    bool IsWeakTo(EmotionalState current, EmotionalState incoming)
    {
        // Example Rock-Paper-Scissors: Anger beats Calm, Calm beats Joy, Joy beats Anger
        if (current == EmotionalState.Calm && incoming == EmotionalState.Anger) return true;
        if (current == EmotionalState.Joy && incoming == EmotionalState.Calm) return true;
        if (current == EmotionalState.Anger && incoming == EmotionalState.Joy) return true;
        return false;
    }

    void Die()
    {
        Debug.Log(gameObject.name + " succumbed to emotional instability.");
        Destroy(gameObject);
    }
}
