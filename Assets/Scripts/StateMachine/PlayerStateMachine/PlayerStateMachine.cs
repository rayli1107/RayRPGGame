using System;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine
{
    public class AbstractPlayerState : AbstractState
    {
        protected PlayerStateMachine stateMachine { get; private set; }
        protected PlayerController controller => stateMachine.playerController;
        private int _animatorStateIdUpperIdle;
        private float _animatorWeightVelocity;

        protected List<Tuple<bool, PlayerActionController>> playerActionsTriggered { get; private set; }

        public AbstractPlayerState(PlayerStateMachine stateMachine) : base()
        {
            this.stateMachine = stateMachine;
            _animatorStateIdUpperIdle = Animator.StringToHash("Upper Idle State");

            playerActionsTriggered = new List<Tuple<bool, PlayerActionController>>();
        }

        public override void EnterState(StateMachineParameter param)
        {
            base.EnterState(param);
            _animatorWeightVelocity = 0;
        }

        public override void Update()
        {
            base.Update();
            const int layerId = 1;
            AnimatorStateInfo stateInfo = controller.animator.GetCurrentAnimatorStateInfo(layerId);
            float targetWeight = stateInfo.shortNameHash == _animatorStateIdUpperIdle ? 0 : 1;
            targetWeight = Mathf.SmoothDamp(
                controller.animator.GetLayerWeight(layerId),
                targetWeight,
                ref _animatorWeightVelocity,
                0.1f);
            controller.animator.SetLayerWeight(layerId, targetWeight);
        }
    }

    public class PlayerStateMachine : AbstractStateMachine
    {
        public PlayerFreeMoveState PlayerFreeMoveState { get; private set; }
        public PlayerFixedTargetMoveState PlayerFixedTargetMoveState { get; private set; }
        public PlayerFlinchState PlayerFlinchState { get; private set; }
        public SpinAttackSkillState SpinAttackSkillState { get; private set; }

        public PlayerController playerController { get; private set; }

        public PlayerStateMachine(PlayerController playerController) : base()
        {
            this.playerController = playerController;

            PlayerFreeMoveState = new PlayerFreeMoveState(this);
            PlayerFixedTargetMoveState = new PlayerFixedTargetMoveState(this);
            PlayerFlinchState = new PlayerFlinchState(this);
            SpinAttackSkillState = new SpinAttackSkillState(this);

            currentState = PlayerFreeMoveState;
        }
    }
}