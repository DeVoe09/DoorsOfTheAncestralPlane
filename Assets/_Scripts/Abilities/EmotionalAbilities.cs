using UnityEngine;
using System.Collections;

public class EmotionalAbilities : MonoBehaviour
{
    [Header("Joy: Radiant Pulse")]
    public float joyPulseRadius = 5f;
    public float joyHealAmount = 5f;
    public float joyPulseDuration = 3f;

    [Header("Calm: Chronos Field")]
    public float calmFieldRadius = 7f;
    public float calmTimeScale = 0.3f;
    public float calmDuration = 5f;

    [Header("Anger: Spiteful Spikes")]
    public float angerSpikeDamage = 25f;
    public float angerSpikeRange = 5f;

    private EmotionManager emotionManager;
    private EmotionalCombatant combatant;

    void Start()
    {
        emotionManager = GetComponent<EmotionManager>();
        if (emotionManager == null) emotionManager = GetComponentInChildren<EmotionManager>();
        combatant = GetComponent<EmotionalCombatant>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            UseAbility();
        }
    }

    void UseAbility()
    {
        if (emotionManager == null) return;

        switch (emotionManager.currentState)
        {
            case EmotionalState.Joy:
                StartCoroutine(ExecuteRadiantPulse());
                break;
            case EmotionalState.Calm:
                StartCoroutine(ExecuteChronosField());
                break;
            case EmotionalState.Anger:
                ExecuteSpitefulSpikes();
                break;
        }
    }

    IEnumerator ExecuteRadiantPulse()
    {
        Debug.Log("Executing Radiant Pulse");
        float elapsed = 0;
        while (elapsed < joyPulseDuration)
        {
            Collider[] targets = Physics.OverlapSphere(transform.position, joyPulseRadius);
            foreach (var t in targets)
            {
                if (t.gameObject == gameObject)
                {
                    if (combatant != null) combatant.health = Mathf.Min(combatant.health + (joyHealAmount * Time.deltaTime), 100f);
                }
            }
            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator ExecuteChronosField()
    {
        Debug.Log("Executing Chronos Field");
        // Simplified: In a real Unity setup, you'd use a Trigger trigger or affect Enemy AI scripts directly
        // Here we just flag the activation for 5 seconds
        yield return new WaitForSeconds(calmDuration);
        Debug.Log("Chronos Field Expired");
    }

    void ExecuteSpitefulSpikes()
    {
        Debug.Log("Executing Spiteful Spikes");
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, angerSpikeRange))
        {
            EmotionalCombatant target = hit.collider.GetComponent<EmotionalCombatant>();
            if (target != null)
            {
                target.TakeDamage(angerSpikeDamage, EmotionalState.Anger);
            }
        }
    }
}
