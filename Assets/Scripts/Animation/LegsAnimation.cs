using TopDown.Movement;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class LegsAnimation : MonoBehaviour
{
    [SerializeField] private Mover playerMover;
    private Animator legsAnimator;
    private static readonly int IsMoving = Animator.StringToHash("moving");

    private void Awake()
    {
        legsAnimator = GetComponent<Animator>();
    }

    public void SetMovementAnimation(bool isMoving)
    {
        legsAnimator.SetBool(IsMoving, isMoving);
    }

    private void Update()
    {
        bool isMoving = playerMover.CurrentInput.magnitude > 0;
        SetMovementAnimation(isMoving);
    }
}
