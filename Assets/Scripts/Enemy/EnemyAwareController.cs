using UnityEngine;

public class EnemyAwareController : MonoBehaviour
{

    public bool awareOfPlayer {get; private set;}
    public Vector2 DirectionToPlayer {get; private set;}
    public float DistanceToPlayer {get; private set;}
    [SerializeField] private float playerAwarenessDistance;
    private Transform playerTransform;

    private void Awake() 
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;    
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 enemyToPlayer = playerTransform.position - transform.position;
        DirectionToPlayer = enemyToPlayer.normalized;
        DistanceToPlayer = enemyToPlayer.magnitude;
        awareOfPlayer = true;
    }
}
