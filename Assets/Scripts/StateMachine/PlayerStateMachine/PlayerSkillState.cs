using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachine
{
    public class SpinAttackSkillState : AbstractPlayerSkillState
    {
        public SpinAttackSkillState(PlayerStateMachine stateMachine) :
            base(stateMachine, "Spin Attack", "Spin Attack")
        {

        }

        public override void EnterState(StateMachineParameter param)
        {
            base.EnterState(param);
            controller.EnableSpinAttackHitBox(true);
        }

        public override void ExitState()
        {
            base.ExitState();
            controller.EnableSpinAttackHitBox(false);
        }
    }

    public class AbstractPlayerSkillState : AbstractPlayerState
    {
        private int _animatorParamId;
        private int _animatorStateId;
        private bool _enteredState;

        public AbstractPlayerSkillState(
            PlayerStateMachine stateMachine,
            string animatorParam,
            string animatorState) : base (stateMachine)
        {
            _animatorParamId = Animator.StringToHash(animatorParam);
            _animatorStateId = Animator.StringToHash(animatorState);
        }

        public override void EnterState(StateMachineParameter param)
        {
            base.EnterState(param);
            _enteredState = false;
            controller.animator.SetTrigger(_animatorParamId);
        }

        public override void Update()
        {
            base.Update();
            const int layerId = 0;

            AnimatorStateInfo stateInfo = controller.animator.GetCurrentAnimatorStateInfo(layerId);
            if (!_enteredState && stateInfo.shortNameHash == _animatorStateId)
            {
                _enteredState = true;
            }
            else if (_enteredState && stateInfo.shortNameHash != _animatorStateId)
            {
                stateMachine.ChangeState(stateMachine.PlayerFreeMoveState);
            }
        }
    }
}