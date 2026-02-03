using UnityEngine;

public class EmotionalAttacker : MonoBehaviour
{
    public float baseDamage = 10f;
    public float attackRange = 2f;
    public float attackCooldown = 1f;
    private float nextAttackTime;

    void Update()
    {
        if (Time.time >= nextAttackTime && Input.GetButtonDown("Fire1"))
        {
            PerformAttack();
        }
    }

    void PerformAttack()
    {
        EmotionManager em = GetComponent<EmotionManager>();
        if (em == null) em = GetComponentInChildren<EmotionManager>();
        
        EmotionalState currentType = (em != null) ? em.currentState : EmotionalState.Neutral;
        
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, attackRange))
        {
            EmotionalCombatant target = hit.collider.GetComponent<EmotionalCombatant>();
            if (target != null)
            {
                float damage = baseDamage;
                if (GetComponent<EmotionalCombatant>() != null)
                {
                    damage *= GetComponent<EmotionalCombatant>().attackMultiplier;
                }
                
                target.TakeDamage(damage, currentType);
                nextAttackTime = Time.time + attackCooldown;
                Debug.Log("Attacked with " + currentType + " for " + damage + " damage.");
            }
        }
    }
}
