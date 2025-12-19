using UnityEngine;

namespace TopDown.Movement
{
    public class Rotator : MonoBehaviour
    {
        [SerializeField] protected float rotationSpeed = 15f;
        
        protected void lookAt(Transform rotatedTransform, Vector3 target)
        {
            float lookAngle = AngleBetweenPoints(rotatedTransform.position, target) - 90f;
            Quaternion targetRotation = Quaternion.Euler(0, 0, lookAngle);
            rotatedTransform.rotation = Quaternion.Slerp(rotatedTransform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        static private float AngleBetweenPoints(Vector3 a, Vector3 b)
        {
            return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
        }
    }
    
}
