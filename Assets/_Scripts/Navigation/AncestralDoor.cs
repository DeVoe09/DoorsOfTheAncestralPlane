using UnityEngine;

public class AncestralDoor : MonoBehaviour
{
    public EmotionalState requiredState = EmotionalState.Neutral;
    public float requiredBalanceMin = -1f;
    public float requiredBalanceMax = 1f;

    [Header("Animation")]
    public float openHeight = 5f;
    public float speed = 2f;

    private bool isOpen = false;
    private Vector3 closedPosition;
    private Vector3 openPosition;

    void Start()
    {
        closedPosition = transform.position;
        openPosition = closedPosition + Vector3.up * openHeight;
    }

    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            EmotionManager em = player.GetComponentInChildren<EmotionManager>();
            if (em != null)
            {
                CheckConditions(em.currentState, em.balanceMeter);
            }
        }

        Vector3 target = isOpen ? openPosition : closedPosition;
        transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * speed);
    }

    void CheckConditions(EmotionalState state, float balance)
    {
        isOpen = (state == requiredState && balance >= requiredBalanceMin && balance <= requiredBalanceMax);
    }
}
