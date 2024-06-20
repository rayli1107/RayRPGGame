using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachine
{
    public class AbstractPlayerMoveState : AbstractPlayerState
    {
        private int _animatorParameterIdMoveX;
        private int _animatorParameterIdMoveZ;
        private int _animatorParameterIdAttack;
        private int _animatorParameterIdGuard;

        public AbstractPlayerMoveState(PlayerStateMachine stateMachine) : base (stateMachine)
        {
            _animatorParameterIdMoveX = Animator.StringToHash("MoveX");
            _animatorParameterIdMoveZ = Animator.StringToHash("MoveZ");
            _animatorParameterIdAttack = Animator.StringToHash("Attack");
            _animatorParameterIdGuard = Animator.StringToHash("Guard");
        }

        protected void setMoveAnimation(float x, float z)
        {
            controller.animator.SetFloat(_animatorParameterIdMoveX, x);
            controller.animator.SetFloat(_animatorParameterIdMoveZ, z);
        }

        public override void Update()
        {
            base.Update();

            if (controller.actionAttack.triggered)
            {
                controller.animator.SetTrigger(_animatorParameterIdAttack);
            }

            controller.animator.SetBool(
                _animatorParameterIdGuard, controller.actionDefend.ReadValue<float>() > 0.5f);
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