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

public class EnemyController : BaseNPCGameUnitController
{
    [field: SerializeField]
    public EnemyProfile enemyProfile { get; private set; }
    [field: SerializeField]
    public Collider attackHitBox { get; private set; }
    [field: SerializeField]
    public float idleDuration { get; private set; }

    [field: SerializeField]
    public float attackCooldown { get; private set; }

    [field: SerializeField]
    public float minWanderDistance { get; private set; }
    [field: SerializeField]
    public float maxWanderDistance { get; private set; }
    [field: SerializeField]
    public float noticeDistance { get; private set; }

    public Vector3 initialPosition { get; private set; }
    public EnemyStateMachine stateMachine { get; private set; }

    public EnemyGameUnit gameUnit { get; private set; }

    private int _lastHitAttackId;

    public EnemyController() : base()
    {
        idleDuration = 2;
        attackCooldown = 2;
        minWanderDistance = 3;
        maxWanderDistance = 15;
        noticeDistance = 5;
    }

    protected override void Awake()
    {
        base.Awake();
        initialPosition = transform.position;
        gameUnit = new EnemyGameUnit(enemyProfile);
        stateMachine = new EnemyStateMachine(this);

        GetComponent<EnemyUIController>().gameUnit = gameUnit; 
    }

    // Start is called before the first frame update
    void Start()
    {
        _lastHitAttackId = 0;
        stateMachine.Start(null);
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.Update();
    }

    public EnemyMoveBehavior GetEnemyMoveBehavior()
    {
        Vector3 delta = transform.position - initialPosition;
        delta.y = 0;
        float distance = delta.magnitude;
        if (distance >= maxWanderDistance)
        {
            return EnemyMoveBehavior.FORCED_RETURN;
        }
        else if (distance >= minWanderDistance)
        {
            return EnemyMoveBehavior.SHOULD_RETURN;
        }
        else
        {
            return EnemyMoveBehavior.CAN_WANDER;
        }
    }

    public bool CanNoticePlayer() {
        Vector3 delta = transform.position - player.transform.position;
        delta.y = 0;
        return delta.magnitude <= noticeDistance;
    }

    public bool CanHitPlayer()
    {
        return attackHitBox.bounds.Intersects(
            player.characterController.bounds);
    }

    public void OnHit(int damage, int staggerDamage)
    {
        Debug.LogFormat("OnHit {0} {1}", damage, staggerDamage);
        int multiplier = gameUnit.IsStaggered ? 2 : 1;
        gameUnit.HP.value -= damage * multiplier;

        if (!gameUnit.IsStaggered)
        {
            gameUnit.Stagger.value += staggerDamage;
        }

        if (gameUnit.HP.value <= 0)
        {
            player.GainExp(gameUnit.Exp.value);
            GlobalDataManager.Instance.gameData.RegisterMonsterKill(enemyProfile.id);
            stateMachine.ChangeState(stateMachine.EnemyDeadState);
        }
        else if (gameUnit.Stagger.value >= gameUnit.Stagger.maxValue)
        {
            stateMachine.ChangeState(stateMachine.EnemyStaggeredState);
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

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (other == player.rightHandWeapon.attackHitBox)
        {
            if (_lastHitAttackId != player.attackHitContext.attackId ||
                !player.attackHitContext.attackOnce)
            {
                OnHit(GlobalDataManager.Instance.gameData.playerData.Attack.value, 1);
                _lastHitAttackId = player.attackHitContext.attackId;
            }
        }
    }
}
