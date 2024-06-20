using StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : GameUnitController
{
    [SerializeField]
    private Collider _attackHitBox;
    [SerializeField]
    private float _staminaRegenSpeed = 0.5f;
    [SerializeField]
    private float _staminaDrainSpeedWhenGuarding = 0.5f;
    [SerializeField]
    private int _attackStamina = 1;
    public Collider attackHitBox => _attackHitBox;

    private PlayerGameUnit _playerData => GlobalGameData.Instance.PlayerData;
    public PlayerInput playerInput { get; private set; }
    public InputAction actionMove { get; private set; }
    public InputAction actionAction { get; private set; }
    public InputAction actionAttack { get; private set; }
    public InputAction actionDefend { get; private set; }

    private int _animatorParameterIdMove;
    private int _animatorParameterIdAttack;

    private TriggerController _currentTriggerController;

    private List<EnemyController> _currentAttackTargets;

    public PlayerStateMachine stateMachine;

    public bool isGuarding { get; private set; }
    protected override void Awake()
    {
        base.Awake();
        _animatorParameterIdMove = Animator.StringToHash("Moving");
        _animatorParameterIdAttack = Animator.StringToHash("Attack");

        playerInput = GetComponent<PlayerInput>();
        actionMove = playerInput.actions["Move"];
        actionAction = playerInput.actions["Action"];
        actionAttack = playerInput.actions["Attack"];
        actionDefend = playerInput.actions["Defend"];

//        transform.position = GlobalGameData.Instance.NextScenePlayerPosition;

        _currentAttackTargets = new List<EnemyController>();

        stateMachine = new PlayerStateMachine(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        _currentTriggerController = null;
        stateMachine.Start();
    }

    // Update is called once per frame
    void Update()
    {
        isGuarding = actionDefend.ReadValue<float>() > 0.5f;
        if (isGuarding)
        {
            _playerData.stamina -= _staminaDrainSpeedWhenGuarding * Time.deltaTime;
        }
        else
        {
            _playerData.stamina += _staminaRegenSpeed * Time.deltaTime;

        }
        stateMachine.Update();
        /*
        if (_actionAttack.triggered)
        {
            animator.SetTrigger(_animatorParameterIdAttack);
            foreach (EnemyController enemy in _currentAttackTargets)
            {
                enemy.OnHit(1);
            }
        }
        Vector2 inputMove = _actionMove.ReadValue<Vector2>();
        float moveMagnitude = inputMove.magnitude;
        if (moveMagnitude > 0.01f)
        {
            float angleForward = Mathf.Rad2Deg * Mathf.Atan2(inputMove.y, inputMove.x) - 90;
            _playerTransform.rotation = Quaternion.Euler(0, -1 * angleForward, 0);
        }

        characterController.Move(_playerTransform.forward * moveMagnitude * _playerSpeed * Time.deltaTime);
        animator.SetBool(_animatorParameterIdMove, moveMagnitude > 0.5f);

        if (_actionAction.triggered && _currentTriggerController != null)
        {
            _currentTriggerController.Invoke();
        }
        */
    }

    private void OnTriggerEnter(Collider other)
    {
        TriggerController triggerController = other.GetComponent<TriggerController>();
        if (triggerController != null)
        {
            _currentTriggerController = triggerController; 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        TriggerController triggerController = other.GetComponent<TriggerController>();
        if (_currentTriggerController == triggerController)
        {
            _currentTriggerController = null;
        }
    }

    public void RegisterAttackTarget(EnemyController enemy)
    {
        if (!_currentAttackTargets.Contains(enemy))
        {
            _currentAttackTargets.Add(enemy);
        }
    }

    public void UnregisterAttackTarget(EnemyController enemy)
    {
        _currentAttackTargets.Remove(enemy);
    }

    public void OnHit(int damage)
    {
        if (isGuarding)
        {
            _playerData.hp -= 1;
            _playerData.stamina -= damage - 1;
        }
        else
        {
            _playerData.hp -= damage;
        }
        stateMachine.ChangeState(stateMachine.PlayerFlinchState);
    }

    public void Attack()
    {
        foreach (EnemyController enemy in _currentAttackTargets)
        {
            enemy.OnHit(_playerData.attack);
        }
    }

    public bool TryAttack()
    {
        if (_playerData.stamina >= _attackStamina)
        {
            _playerData.stamina -= _attackStamina;
            return true;
        }
        return false;
    }
}
