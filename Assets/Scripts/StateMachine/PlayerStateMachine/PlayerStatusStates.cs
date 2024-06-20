using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachine
{
    public class PlayerFlinchState : AbstractPlayerState
    {
        private int _animatorParameterIdDamaged;
        private int _animatorStateIdDamaged;
        private bool _flinched;

        public PlayerFlinchState(PlayerStateMachine stateMachine) : base (stateMachine)
        {
            _animatorParameterIdDamaged = Animator.StringToHash("Damaged");
            _animatorStateIdDamaged = Animator.StringToHash("Flinched");
        }

        public override void EnterState(StateMachineParameter param)
        {
            base.EnterState(param);
            _flinched = false;
            controller.animator.SetTrigger(_animatorParameterIdDamaged);
        }

        public override void Update()
        {
            base.Update();

            AnimatorStateInfo stateInfo = controller.animator.GetCurrentAnimatorStateInfo(0);
            if (_flinched && stateInfo.shortNameHash != _animatorStateIdDamaged)
            {
                stateMachine.ChangeState(stateMachine.PlayerFreeMoveState);
            }
            _flinched = stateInfo.shortNameHash == _animatorStateIdDamaged;
        }

    }
}