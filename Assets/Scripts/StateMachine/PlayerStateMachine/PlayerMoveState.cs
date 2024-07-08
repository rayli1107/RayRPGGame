using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachine
{
    public class PlayerRollState : AbstractPlayerState
    {
        private int _animatorStateIdRoll;
        private bool _isRolling;

        public PlayerRollState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
            _animatorStateIdRoll = Animator.StringToHash("Blend Tree - Roll");
        }

        public override void EnterState(StateMachineParameter param)
        {
            base.EnterState(param);
            _isRolling = false;
            controller.animator.SetTrigger(animatorParameterIdRoll);
            PlayerActionManager.Instance.playerRollAction.FinishInvoke(controller);
        }

        public override void Update()
        {
            base.Update();

            bool _ = controller.actionAction.triggered;
            AnimatorStateInfo stateInfo = 
                controller.animator.GetCurrentAnimatorStateInfo(idBaseLayer);
            if (_isRolling && stateInfo.shortNameHash != _animatorStateIdRoll)
            {
                stateMachine.ChangeState(stateMachine.PlayerFreeMoveState);
            }
            else
            {
                _isRolling = stateInfo.shortNameHash == _animatorStateIdRoll;

                Vector2 move = controller.UpdateAnimatorMoveBlend(Vector2.up);
                controller.MoveForward(move);
            }
        }
    }

    public class AbstractPlayerMoveState : AbstractPlayerState
    {
        public AbstractPlayerMoveState(PlayerStateMachine stateMachine) : base (stateMachine)
        {
        }

        public override void EnterState(StateMachineParameter param)
        {
            base.EnterState(param);
        }

        public override void Update()
        {
            base.Update();
        }

        public override void ExitState()
        {
            base.ExitState();
            controller.animator.ResetTrigger(animatorParameterIdAttack);
        }
    }
    public class PlayerFreeMoveState : AbstractPlayerMoveState
    {
        public PlayerFreeMoveState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
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

            if (controller.actionAction.triggered)
            {
                TriggerController trigger = controller.currentTriggerController;
                NPCController npc = controller.GetCurrentNPCTarget();
                if (npc != null)
                {
                    npc.RunBehaviour();
                    return;
                }
                else if (trigger != null)
                {
                    trigger.Invoke();
                    return;
                }
            }

            if (controller.playerTriggeredAction != null)
            {
                bool invoked = false;
                switch (controller.playerTriggeredAction.actionType)
                {
                    case PlayerTriggeredActionType.ROLL:
                        if (moveMagnitude > 0.01f)
                        {
                            invoked = true;
                            stateMachine.ChangeState(stateMachine.PlayerRollState);
                        }
                        break;

                    case PlayerTriggeredActionType.ATTACK:
                        invoked = true;
                        stateMachine.EnterPlayerAttackState(0);
                        break;

                    case PlayerTriggeredActionType.SPIN_ATTACK:
                        invoked = true;
                        stateMachine.ChangeState(stateMachine.SpinAttackSkillState);
                        break;
                }
                if (invoked)
                {
                    if (controller.playerTriggeredAction.autoFinishInvoke)
                    {
                        controller.playerTriggeredAction.FinishInvoke(controller);
                    }
                    controller.playerTriggeredAction.EnterCooldown();
                }
            }

            controller.animator.SetBool(animatorParameterIdGuard, controller.isGuarding);

            Vector2 actualMove = controller.UpdateAnimatorMoveBlend(inputMove);
            controller.MoveForward(actualMove);
        }
    }

    public class PlayerFixedTargetMoveState : AbstractPlayerMoveState
    {
        public PlayerFixedTargetMoveState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }
    }
}