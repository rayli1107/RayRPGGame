using ScriptableObjects;
using StateMachine;
using UnityEngine;

public class GameUnitController : MonoBehaviour
{
    [SerializeField]
    private Transform _animatorTansform;
    [SerializeField]
    private float _moveSpeed = 10f;
    [SerializeField]
    private float _flinchSpeed = 5f;
    [SerializeField]
    private float _animationSmoothTime = 0.1f;
    public float animationSmoothTime => _animationSmoothTime;


    public Animator animator { get; private set; }
    public CharacterController characterController { get; private set; }

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    public void Rotate(float angleDegrees)
    {
        _animatorTansform.Rotate(Vector3.up, angleDegrees);
    }

    public void RotateTowards(Vector3 target)
    {
        Vector3 delta = target - transform.position;
        float angleForward = Mathf.Rad2Deg * Mathf.Atan2(delta.z, delta.x) - 90;
        _animatorTansform.rotation = Quaternion.Euler(0, -1 * angleForward, 0);
    }

    public void FlinchBackwards(float multiplier = 1f)
    {
        Vector3 move = -1 * _animatorTansform.forward * _flinchSpeed * Time.deltaTime * multiplier;
        move.y = 0;
        characterController.Move(move);
    }

    public void MoveForward(float multiplier = 1f)
    {
        Vector3 move = _animatorTansform.forward * _moveSpeed * Time.deltaTime * multiplier;
        move.y = 0;
        characterController.Move(move);
    }

    public void MoveTowards(Vector3 target, float multiplier = 1f)
    {
        RotateTowards(target);
        MoveForward(multiplier);
    }
}
