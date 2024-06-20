using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachine
{
    public class AbstractPlayerMoveState : AbstractPlayerState
    {
        private enum AttackState
        {
            NONE,
            PRE_ATTACK,
            POST_ATTACK
        }

        private int _animatorParameterIdMoveX;
        private int _animatorParameterIdMoveZ;
        private int _animatorParameterIdAttack;
        private int _animatorParameterIdGuard;
        private int _animatorStateIdAttacking;
        private AttackState _attackState;
        private const float _attackTimeThreshold = 0.4f;

        public AbstractPlayerMoveState(PlayerStateMachine stateMachine) : base (stateMachine)
        {
            _animatorParameterIdMoveX = Animator.StringToHash("MoveX");
            _animatorParameterIdMoveZ = Animator.StringToHash("MoveZ");
            _animatorParameterIdAttack = Animator.StringToHash("Attack");
            _animatorParameterIdGuard = Animator.StringToHash("Guard");
            _animatorStateIdAttacking = Animator.StringToHash("Attack2");
        }

        protected void setMoveAnimation(float x, float z)
        {
            controller.animator.SetFloat(_animatorParameterIdMoveX, x);
            controller.animator.SetFloat(_animatorParameterIdMoveZ, z);
        }

        public override void EnterState(StateMachineParameter param)
        {
            base.EnterState(param);
            _attackState = AttackState.NONE;
        }

        public override void Update()
        {
            base.Update();
            const int layerId = 1;

            if (controller.actionAttack.triggered &&
                _attackState == AttackState.NONE &&
                controller.TryAttack())
            {
                _attackState = AttackState.PRE_ATTACK;
                controller.animator.SetTrigger(_animatorParameterIdAttack);
            }

            AnimatorStateInfo stateInfo = controller.animator.GetCurrentAnimatorStateInfo(layerId);
            if (_attackState == AttackState.PRE_ATTACK &&
                stateInfo.shortNameHash == _animatorStateIdAttacking &&
                stateInfo.normalizedTime >= _attackTimeThreshold)
            {
                controller.Attack();
                _attackState = AttackState.POST_ATTACK;
            }

            if (_attackState == AttackState.POST_ATTACK && stateInfo.shortNameHash != _animatorStateIdAttacking)
            {
                _attackState = AttackState.NONE;
            }

            controller.animator.SetBool(_animatorParameterIdGuard, controller.isGuarding);
        }

        public override void ExitState()
        {
            base.ExitState();
            setMoveAnimation(0, 0);
            controller.animator.ResetTrigger(_animatorParameterIdAttack);
        }
    }
    public class PlayerFreeMoveState : AbstractPlayerMoveState
    {
        private float _currentAnimationBlend;
        private float _currentAnimationVelocity;

        public PlayerFreeMoveState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void EnterState(StateMachineParameter param)
        {
            base.EnterState(param);
            _currentAnimationBlend = 0;
            _currentAnimationVelocity = 0;
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
            }

            _currentAnimationBlend = Mathf.SmoothDamp(
                _currentAnimationBlend,
                moveMagnitude,
                ref _currentAnimationVelocity,
                controller.animationSmoothTime);

            controller.MoveForward(_currentAnimationBlend);
            setMoveAnimation(0, _currentAnimationBlend);
        }
    }

    public class PlayerFixedTargetMoveState : AbstractPlayerMoveState
    {
        public PlayerFixedTargetMoveState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }
    }
}