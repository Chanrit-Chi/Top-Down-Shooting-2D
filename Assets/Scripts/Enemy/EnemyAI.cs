using UnityEngine;
 
namespace EnemyAI
{
    public class EnemyPatrolBehaviour : MonoBehaviour
    {
        [Header("Movement Parameters")]
        [SerializeField] private float movementSpeed = 10;
        [SerializeField] private float stoppingDistance = 0.1f;
        [SerializeField] private float changeDirectionDelay = 1;
        private float delayTimer;
 
        [Header("Animation")]
        [SerializeField] private Animator animator;
 
        [Header("Patrol Path")]
        [SerializeField] private Vector3[] patrolEdges;
        private Vector3 destinationPoint;
        private int patrolPointIndex;
 
        private void Awake()
        {
            SetPatrolPoint(0);
        }
        private void Update()
        {
            //If patrol path null return to avoid crashes
            if (patrolEdges.Length == 0 || patrolEdges == null) return;
 
            //Move transform to destination point or change destination if arrived
            bool moving = false;
            if (Vector3.Distance(transform.position, destinationPoint) <= stoppingDistance)
                SetNextDestination();
            else
            {
                moving = true;
                transform.position = Vector3.MoveTowards(transform.position, destinationPoint, movementSpeed * Time.deltaTime);
            }
 
            //Set movement animator
            animator.SetBool("moving", moving);
        }
 
        private void SetNextDestination()
        {
            //Increment delay timer and wait until it reached necessary value
            delayTimer += Time.deltaTime;
            if (delayTimer < changeDirectionDelay) return;
 
            //Set next destination point
            patrolPointIndex++;
            SetPatrolPoint(patrolPointIndex);
 
            //Turn enemy to face destination
            transform.localScale = new Vector3(Mathf.Sign(transform.position.x - patrolEdges[patrolPointIndex].x), 1, 1);
 
            //Reset timer
            delayTimer = 0;
        }
        private void SetPatrolPoint(int index)
        {
            //If patrol path null return to avoid crashes
            if (patrolEdges.Length == 0 || patrolEdges == null) return;
 
            //If index outside range reset to 0
            if (index >= patrolEdges.Length) index = 0;
 
            //Set destination point
            destinationPoint = patrolEdges[index];
            patrolPointIndex = index;
        }
 
        //Visualize patrol points
        private void OnDrawGizmos()
        {
            foreach (var patrolEdge in patrolEdges)
                Gizmos.DrawIcon(patrolEdge, "Patrol Edge");
        }
    }
}