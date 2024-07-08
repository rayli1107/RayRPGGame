using UnityEngine;

public class BaseGameUnitController : MonoBehaviour
{
    [field: SerializeField]
    public Transform animatorTansform { get; private set; }

    [field: SerializeField]
    public float moveSpeed { get; private set; }

    [field: SerializeField]
    public float flinchSpeed { get; private set; }

    [field: SerializeField]
    public float animationSmoothTime { get; private set; }

    public virtual Sprite face => null;
    public new virtual string name => null;

    protected virtual string animatorParamNameMoveX => "MoveX";
    protected virtual string animatorParamNameMoveZ => "MoveZ";
    protected int animatorParamIdMoveX { get; private set; }
    protected int animatorParamIdMoveZ { get; private set; }

    private Vector2 _currentAnimationBlend;
    private Vector2 _currentAnimationVelocity;

    public Animator animator { get; private set; }
    public CharacterController characterController { get; private set; }

    public BaseGameUnitController() : base()
    {
        moveSpeed = 5;
        flinchSpeed = 6;
        animationSmoothTime = 0.1f;
    }

    protected virtual void Awake()
    {
        if (GlobalDataManager.Instance == null)
        {
            gameObject.SetActive(false);
            return;
        }

        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        animatorParamIdMoveX = Animator.StringToHash(animatorParamNameMoveX);
        animatorParamIdMoveZ = Animator.StringToHash(animatorParamNameMoveZ);
    }

    protected virtual void OnEnable()
    {
        _currentAnimationBlend = Vector2.zero;
        _currentAnimationVelocity = Vector2.zero;
    }

    public void Rotate(float angleDegrees)
    {
        animatorTansform.Rotate(Vector3.up, angleDegrees);
    }

    public void RotateTowards(Vector3 target)
    {
        Vector3 delta = target - transform.position;
        float angleForward = Mathf.Rad2Deg * Mathf.Atan2(delta.z, delta.x) - 90;
        animatorTansform.rotation = Quaternion.Euler(0, -1 * angleForward, 0);
    }

    public void FlinchBackwards(float multiplier = 1f)
    {
        Vector3 move = -1 * animatorTansform.forward * flinchSpeed * Time.deltaTime * multiplier;
        move.y = 0;
        characterController.Move(move);
    }

    public void MoveForward()
    {
        MoveForward(Vector2.up, moveSpeed);
    }


    public void MoveForward(Vector2 move)
    {
        MoveForward(move, moveSpeed);
    }

    public void MoveForward(Vector2 move, float speed)
    {
        Vector3 actualMove =
            animatorTansform.right * speed * Time.deltaTime * move.x +
            animatorTansform.forward * speed * Time.deltaTime * move.y;
        actualMove.y = 0;
        characterController.Move(actualMove);
    }

    public void MoveTowards(Vector3 target)
    {
        MoveTowards(target, moveSpeed);
    }

    public void MoveTowards(Vector3 target, float speed)
    {
        RotateTowards(target);
        MoveForward(Vector2.up, speed);
    }

    public void SetRotation(Quaternion quaternion)
    {
        animatorTansform.rotation = quaternion;
    }

    public Vector2 UpdateAnimatorMoveBlend(Vector2 target)
    {
        _currentAnimationBlend = Vector2.SmoothDamp(
            _currentAnimationBlend,
            target,
            ref _currentAnimationVelocity,
            animationSmoothTime);
        animator.SetFloat(animatorParamIdMoveX, _currentAnimationBlend.x);
        animator.SetFloat(animatorParamIdMoveZ, _currentAnimationBlend.y);
        return _currentAnimationBlend;
    }

    public Vector2 GetReflectedVector(Vector2 original)
    {
        Vector3 right3 = animatorTansform.right;
        Vector2 right2 = new Vector2(right3.x, right3.z);
        float angle = Vector2.SignedAngle(right2, original) * Mathf.Deg2Rad;
        float magnitude = original.magnitude;
        Vector2 result = new Vector2(magnitude * Mathf.Cos(angle), magnitude * Mathf.Sin(angle));
        return result;
    }
}
