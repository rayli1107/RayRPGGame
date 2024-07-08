using UnityEngine;

namespace StateMachine
{
    public class AbstractEnemyMoveState : AbstractEnemyState
    {
        private int _animatorParameterIdMove;
        protected int animatorLoop { get; private set; }

        public AbstractEnemyMoveState(EnemyStateMachine stateMachine) : base(stateMachine)
        {
            _animatorParameterIdMove = Animator.StringToHash("Moving");
        }

        public override void EnterState(StateMachineParameter param)
        {
            base.EnterState(param);
            enemyController.animator.SetBool(_animatorParameterIdMove, true);
            animatorLoop = getAnimatorLoopIndex();
        }

        public override void ExitState()
        {
            base.ExitState();
            enemyController.animator.SetBool(_animatorParameterIdMove, false);
        }

        public override void Update()
        {
            base.Update();

            int currentAnimatorLoop = getAnimatorLoopIndex();
            bool updated = animatorLoop != currentAnimatorLoop;
            animatorLoop = currentAnimatorLoop;
            if (updated)
            {
                OnAnimatorLoopUpdate();
            }
        }

        public virtual void OnAnimatorLoopUpdate()
        {
        }

        private int getAnimatorLoopIndex()
        {
            AnimatorStateInfo stateInfo = enemyController.animator.GetCurrentAnimatorStateInfo(0);
            return Mathf.FloorToInt(stateInfo.normalizedTime);
        }
    }

    public class EnemyReturningState : AbstractEnemyMoveState
    {
        private bool _forcedReturning;
        public EnemyReturningState(EnemyStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void EnterState(StateMachineParameter param)
        {
            base.EnterState(param);
            _forcedReturning = param == null ? false : param.forcedReturning;
        }

        public override void Update()
        {
            enemyController.MoveTowards(enemyController.initialPosition, enemyController.moveSpeed);
            base.Update();
        }

        public override void OnAnimatorLoopUpdate()
        {
            if (!_forcedReturning && enemyController.CanNoticePlayer())
            {
                stateMachine.ChangeState(stateMachine.EnemyChasingState);
            }
            else if (enemyController.GetEnemyMoveBehavior() == EnemyMoveBehavior.CAN_WANDER)
            {
                stateMachine.ChangeState(stateMachine.EnemyIdleState);
            }
        }
    }

    public class EnemyWanderingState : AbstractEnemyMoveState
    {
        public EnemyWanderingState(EnemyStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Update()
        {
            enemyController.MoveForward();
            base.Update();
        }

        public override void EnterState(StateMachineParameter param)
        {
            base.EnterState(param);
            enemyController.Rotate(GameController.Instance.Random.Next(360));
        }

        public override void OnAnimatorLoopUpdate()
        {
            if (enemyController.CanNoticePlayer())
            {
                stateMachine.ChangeState(stateMachine.EnemyChasingState);
            }
            else if (enemyController.GetEnemyMoveBehavior() != EnemyMoveBehavior.CAN_WANDER)
            {
                stateMachine.ChangeState(stateMachine.EnemyIdleState);
            }
        }
    }

    public class EnemyChasingState : AbstractEnemyMoveState
    {
        public EnemyChasingState(EnemyStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Update()
        {
            enemyController.MoveTowards(player.transform.position, enemyController.moveSpeed);
            Vector3 delta = enemyController.transform.position - player.transform.position;
            delta.y = 0;
            base.Update();
        }

        public override void OnAnimatorLoopUpdate()
        {
            EnemyMoveBehavior behavior = enemyController.GetEnemyMoveBehavior();
            if (behavior == EnemyMoveBehavior.FORCED_RETURN)
            {
                StateMachineParameter param = new StateMachineParameter();
                param.forcedReturning = true;
                stateMachine.ChangeState(stateMachine.EnemyReturningState, param);
            }
            else if (enemyController.CanHitPlayer())
            {
                stateMachine.ChangeState(stateMachine.EnemyAttackState);
            }
        }
    }
}