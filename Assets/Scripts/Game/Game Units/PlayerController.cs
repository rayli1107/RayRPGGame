using ScriptableObjects;
using StateMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackHitContext
{
    public int attackId;
    public bool attackOnce;

    public PlayerAttackHitContext()
    {
        attackId = 0;
    }
}

[Serializable]
public class PlayerAttackContext
{
    [field: SerializeField]
    public string attackAnimation { get; private set; }

    [field: SerializeField]
    public float attackMultiplier { get; private set; }

    [field: SerializeField]
    public float attackEndTime { get; private set; }

    [field: SerializeField]
    public float attackColliderStartTime { get; private set; }

    [field: SerializeField]
    public float attackColliderEndTime { get; private set; }


    [field: SerializeField]
    public bool pushBack { get; private set; }
}

public class PlayerController : BaseNPCGameUnitController
{
    [field: SerializeField]
    public WeaponController rightHandWeapon { get; private set; }

    [field: SerializeField]
    public WeaponController leftHandWeapon { get; private set; }

    [field: SerializeField]
    public Collider targetCollider { get; private set; }

    [field: SerializeField]
    public float staminaRegenSpeed { get; private set; }

    [field: SerializeField]
    public float staminaDrainSpeedWhenGuarding { get; private set; }

    [field: SerializeField]
    public PlayerAttackContext[] attackContextList { get; private set; }

    [field: SerializeField]
    public float attackMoveSpeed { get; private set; }

    public PlayerProfile playerProfile => GlobalDataManager.Instance.playerProfile;
    public override Sprite face => playerProfile.face;
    public override string name => playerProfile.name;

    public PlayerGameUnit playerUnit { get; private set; }
    public PlayerData playerData => GlobalDataManager.Instance.gameData.playerData;
    public PlayerUIController playerUIController { get; private set; }
    public PlayerInput playerInput { get; private set; }
    public InputAction actionMove { get; private set; }
    public InputAction actionAction { get; private set; }
//    public InputAction actionAttack { get; private set; }
    public InputAction actionDefend { get; private set; }

    private List<TriggerController> _currentTriggers;

    private List<BaseNPCGameUnitController> _currentTargets;

    public PlayerStateMachine stateMachine;

    public PlayerSkillController playerTriggeredAction;

    public PlayerAttackHitContext attackHitContext { get; private set; }

    public bool isGuarding { get; private set; }
    protected override void Awake()
    {
        base.Awake();
        
        playerUIController = GetComponent<PlayerUIController>();
        playerInput = GetComponent<PlayerInput>();
        actionMove = playerInput.actions["Move"];
        actionAction = playerInput.actions["Action"];
//        actionAttack = playerInput.actions["Attack"];
        actionDefend = playerInput.actions["Defend"];

        //        transform.position = GlobalGameData.Instance.NextScenePlayerPosition;

        _currentTargets = new List<BaseNPCGameUnitController>();
        _currentTriggers = new List<TriggerController>();

        stateMachine = new PlayerStateMachine(this);
        attackHitContext = new PlayerAttackHitContext();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        PlayerActionManager.Instance.playerController = this;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        PlayerActionManager.Instance.playerController = null;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerUnit = new PlayerGameUnit(GlobalDataManager.Instance.gameData.playerData);
        transform.position = GlobalDataManager.Instance.NextScenePlayerPosition;
        SetRotation(GlobalDataManager.Instance.NextScenePlayerRotation);
        characterController.enabled = true;

        stateMachine.Start();
        _currentTargets.Clear();
        _currentTriggers.Clear();

        PlayerUnitUIController unitUIController = GetComponent<PlayerUnitUIController>();
        if (unitUIController != null)
        {
            unitUIController.enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        playerTriggeredAction = null;
        PlayerActionManager.Instance.RunActions(this);

        isGuarding = actionDefend.ReadValue<float>() > 0.5f;
        if (isGuarding)
        {
            playerUnit.Stamina.value -= staminaDrainSpeedWhenGuarding * Time.deltaTime;
        }
        else
        {
            playerUnit.Stamina.value += staminaDrainSpeedWhenGuarding * Time.deltaTime;
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

    public NPCController GetCurrentNPCTarget()
    {
        foreach (BaseNPCGameUnitController target in _currentTargets)
        {
            NPCController npc = target.GetComponent<NPCController>();
            if (npc != null)
            {
                return npc;
            }
        }
        return null;
    }

    /*
    private void OnTriggerEnter(Collider other)
    {
        TriggerController triggerController = other.GetComponent<TriggerController>();
        if (triggerController != null)
        {
            currentTriggerController = triggerController; 

            string message = triggerController.message;
            if (message.Length > 0)
            {
                GameUIManager.Instance.ShowQuickMessage(message);
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        TriggerController triggerController = other.GetComponent<TriggerController>();
        if (currentTriggerController == triggerController)
        {
            currentTriggerController = null;

            GameUIManager.Instance.HideQuickMessage();
        }
    }*/

    public void RegisterTarget(BaseNPCGameUnitController target)
    {
        if (!_currentTargets.Contains(target))
        {
            _currentTargets.Add(target);
        }
    }

    public void UnregisterTarget(BaseNPCGameUnitController target)
    {
        _currentTargets.Remove(target);
    }

    public void OnHit(int damage)
    {
        Debug.LogFormat("OnHit {0}", damage);
        if (isGuarding)
        {
            playerUnit.HP.value -= 1;
            playerUnit.Stamina.value -= damage - 1;
        }
        else
        {
            playerUnit.HP.value -= damage;
        }
        stateMachine.ChangeState(stateMachine.PlayerFlinchState);
    }

    public void Attack()
    {
        List<EnemyController> enemies = new List<EnemyController>();

        foreach (BaseNPCGameUnitController target in _currentTargets)
        {
            EnemyController enemy = target.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemies.Add(enemy);
            }
        }
        foreach (EnemyController enemy in enemies)
        {
            enemy.OnHit(playerUnit.Attack.value, 1);
        }
    }

    public bool CheckStaminaCost(int staminaCost)
    {
//        Debug.LogFormat("CheckStaminaCost cost {0} stamina {1}", staminaCost, playerUnit.Stamina.value);
        return playerUnit.Stamina.value >= staminaCost;
    }

    public bool PayStaminaCost(int staminaCost)
    {
        if (CheckStaminaCost(staminaCost))
        {
            playerUnit.Stamina.value -= staminaCost;
            return true;
        }
        return false;
    }

    public void GainExp(int exp)
    {
        playerData.Exp.value += exp;
    }

    public bool hasTriggerController => _currentTriggers.Count > 0;
    public TriggerController currentTriggerController => hasTriggerController ? _currentTriggers[0] : null;

    public void RegisterTriggerController(TriggerController triggerController)
    {
        if (!_currentTriggers.Contains(triggerController))
        {
            _currentTriggers.Add(triggerController);
            if (_currentTriggers.Count == 1 &&
                triggerController.message.Length > 0)
            {
                GameUIManager.Instance.ShowQuickMessage(triggerController.message);
            }
        }
    }

    public void UnregisterTriggerController(TriggerController triggerController)
    {
        if (_currentTriggers.Remove(triggerController))
        {
            if (_currentTriggers.Count > 0 &&
                _currentTriggers[0].message.Length > 0)
            {
                GameUIManager.Instance.ShowQuickMessage(_currentTriggers[0].message);
            }
            else
            {
                GameUIManager.Instance.HideQuickMessage();
            }
        }
    }

    public void RegisterAttackHitContext(bool attackOnce)
    {
        ++attackHitContext.attackId;
        attackHitContext.attackOnce = attackOnce;
    }

    public void EnableSpinAttackHitBox(bool enable)
    {
//        spinAttackHitBox.enabled = enable;
    }
}
