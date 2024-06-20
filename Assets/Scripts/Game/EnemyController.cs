using ScriptableObjects;
using StateMachine;
using UnityEngine;
using UnityEngine.InputSystem;

public enum EnemyMoveBehavior
{
    FORCED_RETURN,
    SHOULD_RETURN,
    CAN_WANDER
}

public class EnemyController : GameUnitController
{
    [SerializeField]
    private EnemyProfile _enemyProfile;
    [SerializeField]
    private Collider _attackHitBox;

    [SerializeField]
    private float _idleDuration = 2f;
    public float idleDuration => _idleDuration;

    [SerializeField]
    private float _attackCooldown = 2f;
    public float attackCooldown => _attackCooldown;

    [SerializeField]
    private float _minWanderDistance = 3f;
    [SerializeField]
    private float _maxWanderDistance = 10f;
    [SerializeField]
    private float _noticeDistance = 5f;
    private PlayerController _player => GameController.Instance.player;
    private int _animatorParameterIdMove;
    private int _animatorParameterIdAttack;

    public Vector3 initialPosition { get; private set; }
    public EnemyStateMachine stateMachine { get; private set; }

    public BaseGameUnit gameUnit { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        initialPosition = transform.position;
        gameUnit = new BaseGameUnit(_enemyProfile);
        stateMachine = new EnemyStateMachine(this);

        GetComponent<EnemyUIController>().gameUnit = gameUnit; 
    }

    // Start is called before the first frame update
    void Start()
    {
        stateMachine.Start(null);
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.Update();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == _player.attackHitBox)
        {
            _player.RegisterAttackTarget(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == _player.attackHitBox)
        {
            _player.UnregisterAttackTarget(this);
        }
    }

    public EnemyMoveBehavior GetEnemyMoveBehavior()
    {
        Vector3 delta = transform.position - initialPosition;
        delta.y = 0;
        float distance = delta.magnitude;
        if (distance >= _maxWanderDistance)
        {
            return EnemyMoveBehavior.FORCED_RETURN;
        }
        else if (distance >= _minWanderDistance)
        {
            return EnemyMoveBehavior.SHOULD_RETURN;
        }
        else
        {
            return EnemyMoveBehavior.CAN_WANDER;
        }
    }

    public bool CanNoticePlayer() {
        Vector3 delta = transform.position - GameController.Instance.player.transform.position;
        delta.y = 0;
        return delta.magnitude <= _noticeDistance;
    }

    public bool CanHitPlayer()
    {
        return _attackHitBox.bounds.Intersects(
            GameController.Instance.player.characterController.bounds);
    }

    public void OnHit(int damage)
    {
        gameUnit.hp -= damage;
        if (gameUnit.hp == 0)
        {
            Destroy(gameObject);
        }
        else
        {
            stateMachine.ChangeState(stateMachine.EnemyFlinchedState);
        }
    }

    /*
    public float GetDistanceFromInitialPosition()
    {
        Vector3 delta = transform.position - initialPosition;
        delta.y = 0;
        return delta.magnitude;
    }

    public float GetDistanceFromPlayer()
    {
        Vector3 delta = transform.position - GameController.Instance.player.transform.position;
        delta.y = 0;
        return delta.magnitude;
    }


    public bool IsNearInitialPosition()
    {
        Vector3 delta = transform.position - initialPosition;
        delta.y = 0;
        return delta.magnitude <= _wanderDistance;
    }


    public bool ShouldGiveUpChase()
    {
        Vector3 delta = transform.position - initialPosition;
        delta.y = 0;
        return delta.magnitude >= _chaseDistance;
    }*/

}
