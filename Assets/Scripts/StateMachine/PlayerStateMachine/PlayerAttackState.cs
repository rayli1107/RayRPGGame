using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachine
{
    public partial class StateMachineParameter
    {
        public int playerAttackIndex;
    }

    public class PlayerAttackState : AbstractPlayerMoveState
    {
        private enum AttackState
        {
            INITIAL,
            PRE_ATTACK,
            PRE_ATTACK_TRANSITION,
            ATTACKING,
            POST_ATTACK_TRANSITION,
        }

        private int _currentAttackIndex;
        private int _currentAnimatorId;
        private AttackState _attackState;
        private PlayerSkillController _currentTriggeredAction;
        private Vector2 _processedMoveVector;
        private bool _attackedColliderStarted;

        public PlayerAttackState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void EnterState(StateMachineParameter param)
        {
            base.EnterState(param);
            _currentAttackIndex = param.playerAttackIndex;
            _currentAnimatorId = Animator.StringToHash(
                    controller.attackContextList[_currentAttackIndex].attackAnimation);
            _attackState = AttackState.INITIAL;
            _currentTriggeredAction = null;
            _processedMoveVector = Vector2.zero;
            _attackedColliderStarted = false;

            controller.animator.SetTrigger(animatorParameterIdAttack);
        }

        public override void ExitState()
        {
            base.ExitState();
            controller.rightHandWeapon.attackHitBox.enabled = false;
        }

        private void processTriggeredAction()
        {
            if (controller.playerTriggeredAction != null && _currentTriggeredAction == null)
            {
                if (controller.playerTriggeredAction.actionType == PlayerTriggeredActionType.ROLL)
                {
                    _currentTriggeredAction = controller.playerTriggeredAction;
                }

                if (controller.playerTriggeredAction.actionType == PlayerTriggeredActionType.ATTACK &&
                    _currentAttackIndex + 1 < controller.attackContextList.Length)
                {
                    _currentTriggeredAction = controller.playerTriggeredAction;
                }
            }
        }

        private void move()
        {
            Vector2 actualMove = controller.UpdateAnimatorMoveBlend(_processedMoveVector);
            controller.MoveForward(actualMove, controller.attackMoveSpeed);
        }

        public override void Update()
        {
            base.Update();

            switch (_attackState)
            {
                case AttackState.INITIAL:
                    Vector2 inputMove = controller.actionMove.ReadValue<Vector2>();
                    float moveMagnitude = inputMove.magnitude;
                    if (moveMagnitude > 0.01f)
                    {
                        Vector3 delta = new Vector3(inputMove.x, 0, inputMove.y);
                        controller.RotateTowards(controller.transform.position + delta);
                        _processedMoveVector = controller.GetReflectedVector(inputMove);
                    }
                    move();
                    _attackState = AttackState.PRE_ATTACK;
                    break;

                case AttackState.PRE_ATTACK:
                    move();
                    if (controller.animator.IsInTransition(idBaseLayer))
                    {
                        AnimatorStateInfo nextStateInfo =
                            controller.animator.GetNextAnimatorStateInfo(idBaseLayer);
                        if (nextStateInfo.shortNameHash == _currentAnimatorId)
                        {
                            PlayerActionManager.Instance.playerAttackAction.FinishInvoke(controller);
                            _attackState = AttackState.PRE_ATTACK_TRANSITION;
                            controller.RegisterAttackHitContext(true);
                        }
                        else
                        {
                            PlayerActionManager.Instance.playerAttackAction.Trigger(controller);
                            _attackState = AttackState.POST_ATTACK_TRANSITION;
                        }
                    }
                    break;

                case AttackState.PRE_ATTACK_TRANSITION:
                    move();
                    processTriggeredAction();
                    if (!controller.animator.IsInTransition(idBaseLayer))
                    {
                        _attackState = AttackState.ATTACKING;
                    }

                    break;

                case AttackState.ATTACKING:
                    move();
                    processTriggeredAction();


                    AnimatorStateInfo stateInfo =
                        controller.animator.GetCurrentAnimatorStateInfo(idBaseLayer);
                    if (!_attackedColliderStarted &&
                        stateInfo.normalizedTime >= controller.attackContextList[_currentAttackIndex].attackColliderStartTime)
                    {
                        controller.rightHandWeapon.attackHitBox.enabled = true;
                    }
                    else if (_attackedColliderStarted &&
                             stateInfo.normalizedTime >= controller.attackContextList[_currentAttackIndex].attackColliderEndTime)
                    {
                        controller.rightHandWeapon.attackHitBox.enabled = false;
                    }

                    if (controller.animator.IsInTransition(idBaseLayer))
                    {
                        _attackState = AttackState.POST_ATTACK_TRANSITION;
                    }
                    else if (_currentTriggeredAction != null &&
                             stateInfo.normalizedTime >=
                             controller.attackContextList[_currentAttackIndex].attackEndTime)
                    {
                        switch (_currentTriggeredAction.actionType)
                        {
                            case PlayerTriggeredActionType.ATTACK:
                                stateMachine.EnterPlayerAttackState(_currentAttackIndex + 1);
                                break;

                            case PlayerTriggeredActionType.ROLL:
                                stateMachine.ChangeState(stateMachine.PlayerRollState);
                                break;
                        }
                    }
                    break;

                case AttackState.POST_ATTACK_TRANSITION:
                    move();
                    if (!controller.animator.IsInTransition(idBaseLayer))
                    {
                        stateMachine.ChangeState(stateMachine.PlayerFreeMoveState);
                    }
                    break;
            }
        }
    }
    /*
    public class PlayerAttackState : AbstractPlayerMoveState
    {
        private const float _attackTimeThreshold = 0.8f;
        private int[] _animatorParameterIdAttacks;
        private int _currentAttackIndex;
        private PlayerActionController _triggeredAttackAction;
        private PlayerActionController _triggeredRollAction;
        private bool _attackStarted;
        private bool _attackFinished;

        public PlayerAttackState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
            _animatorParameterIdAttacks = new int[controller.attackContextList.Length];
            for (int i = 0; i < controller.attackContextList.Length; ++i)
            {
                _animatorParameterIdAttacks[i] = Animator.StringToHash(
                    controller.attackContextList[i].attackAnimation);
            }
        }

        private void enterAttackState(int index)
        {
            Debug.LogFormat("enterAttackState {0}", index);
            _currentAttackIndex = index;
            controller.animator.SetTrigger(animatorParameterIdAttack);
            _triggeredAttackAction = null;
            _triggeredRollAction = null;
            _attackStarted = false;
        }

        public override void EnterState(StateMachineParameter param)
        {
            base.EnterState(param);
            _attackFinished = false;
            enterAttackState(0);
        }

        private bool matchAttackAnimation(AnimatorStateInfo stateInfo)
        {
            return stateInfo.shortNameHash == _animatorParameterIdAttacks[_currentAttackIndex];
        }

        private bool passThreshold(AnimatorStateInfo stateInfo)
        {
            return stateInfo.normalizedTime >= _attackTimeThreshold;
        }

        private int getAttackIndex(int animation_id)
        {
            for (int i = 0; i < _animatorParameterIdAttacks.Length; ++i)
            {
                if (_animatorParameterIdAttacks[i] == animation_id)
                {
                    return i;
                }
            }
            return -1;
        }


        public override void Update()
        {
            base.Update();

            Vector2 inputMove = controller.actionMove.ReadValue<Vector2>();
            float moveMagnitude = inputMove.magnitude;
            if (moveMagnitude > 0.01f)
            {
                Vector3 delta = new Vector3(inputMove.x, 0, inputMove.y);
                controller.RotateTowards(controller.transform.position + delta);
                inputMove = controller.GetReflectedVector(inputMove);
            }

            Vector2 actualMove = controller.UpdateAnimatorMoveBlend(inputMove);
            controller.MoveForward(actualMove);

            if (controller.playerTriggeredAction != null)
            {
                switch (controller.playerTriggeredAction.actionType)
                {
                    case PlayerTriggeredActionType.ROLL:
                        if (moveMagnitude > 0.01f)
                        {
                            _triggeredRollAction = controller.playerTriggeredAction;
                        }
                        break;

                    case PlayerTriggeredActionType.ATTACK:
                        Debug.Log("Attack Action Triggered");
                        _triggeredAttackAction = controller.playerTriggeredAction; ;
                        break;
                }
            }

            AnimatorStateInfo stateInfo = controller.animator.GetCurrentAnimatorStateInfo(idBaseLayer);
            if (_attackFinished)
            {
                if (getAttackIndex(stateInfo.shortNameHash) == -1)
                {
                    stateMachine.ChangeState(stateMachine.PlayerFreeMoveState);
                }
            }
            else if (!_attackStarted)
            {
                if (matchAttackAnimation(stateInfo))
                {
                    Debug.LogFormat("attack Started {0}", _currentAttackIndex);
                    _attackStarted = true;
                }
            }
            else if (controller.animator.IsInTransition(idBaseLayer))
            {
                AnimatorStateInfo nextStateInfo = controller.animator.GetNextAnimatorStateInfo(idBaseLayer);
                if (getAttackIndex(nextStateInfo.shortNameHash) == -1)
                {
                    Debug.Log("Transitioning to move state");
                    _attackFinished = true;
                }
            }
            else if (passThreshold(stateInfo)) 
            {
                if (_triggeredAttackAction != null &&
                    _currentAttackIndex < _animatorParameterIdAttacks.Length - 1)
                {
                    Debug.LogFormat("Attack Action FinishInvoke, normalized time: {0}", stateInfo.normalizedTime);
                    _triggeredAttackAction.FinishInvoke(controller);
                    enterAttackState(_currentAttackIndex + 1);
                }
                else if (_triggeredRollAction != null)
                {
                    _triggeredRollAction.FinishInvoke(controller);
                    stateMachine.ChangeState(stateMachine.PlayerRollState);
                }
            }
        }
    }
    */
}