using UnityEngine;

namespace StateMachine
{
    public class EnemyIdleState : AbstractEnemyState
    {
        public EnemyIdleState(EnemyStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Update()
        {
            base.Update();
            if (Time.time - stateStartTime >= enemyController.idleDuration)
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

                }
                else if (enemyController.CanNoticePlayer())
                {
                    stateMachine.ChangeState(stateMachine.EnemyChasingState);
                }
                else if (behavior == EnemyMoveBehavior.SHOULD_RETURN)
                {
                    stateMachine.ChangeState(stateMachine.EnemyReturningState);
                }
                else
                {
                    stateMachine.ChangeState(stateMachine.EnemyWanderingState);
                }
            }
        }
    }
}