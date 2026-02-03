using UnityEngine;

public class EmotionalEntity : MonoBehaviour
{
    public float detectRange = 10f;
    public float moveSpeed = 3f;

    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        EmotionManager em = player.GetComponentInChildren<EmotionManager>();
        if (em == null) return;

        float dist = Vector3.Distance(transform.position, player.transform.position);
        if (dist > detectRange) return;

        Vector3 direction = (player.transform.position - transform.position).normalized;

        switch (em.currentState)
        {
            case EmotionalState.Anger:
                // Flee
                transform.position -= direction * moveSpeed * Time.deltaTime;
                break;
            case EmotionalState.Joy:
                // Approach
                transform.position += direction * moveSpeed * Time.deltaTime;
                break;
            case EmotionalState.Calm:
                // Passive/Stay put
                break;
        }
    }
}
