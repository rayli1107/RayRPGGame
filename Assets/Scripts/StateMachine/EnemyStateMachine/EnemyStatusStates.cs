using UnityEngine;

namespace StateMachine
{
    public class EnemyFlinchedState : AbstractEnemyState
    {
        private int _animatorParameterIdFlinched;
        private int _animatorStateIdFlinched;
        private bool _flinched;
        public EnemyFlinchedState(EnemyStateMachine stateMachine) : base(stateMachine)
        {
            _animatorParameterIdFlinched = Animator.StringToHash("Flinched");
            _animatorStateIdFlinched = Animator.StringToHash("Flinched");
        }

        public override void EnterState(StateMachineParameter param)
        {
            base.EnterState(param);
            enemyController.animator.SetTrigger(_animatorParameterIdFlinched);
            _flinched = false;
            enemyController.RotateTowards(player.transform.position);
        }

        public override void Update()
        {
            base.Update();

            enemyController.FlinchBackwards();

            AnimatorStateInfo stateInfo = enemyController.animator.GetCurrentAnimatorStateInfo(0);
            bool done = _flinched && stateInfo.shortNameHash != _animatorStateIdFlinched;
            _flinched = stateInfo.shortNameHash == _animatorStateIdFlinched;

            if (done)
            {
                if (enemyController.CanHitPlayer())
                {
                    stateMachine.ChangeState(stateMachine.EnemyAttackState);
                }
                else
                {
                    stateMachine.ChangeState(stateMachine.EnemyChasingState);
                }
            }
        }
    }

    public class EnemyDeadState : AbstractEnemyState
    {
        private int _animatorParameterIdDead;

        public EnemyDeadState(EnemyStateMachine stateMachine) : base(stateMachine)
        {
            _animatorParameterIdDead = Animator.StringToHash("Dead");
        }

        public override void EnterState(StateMachineParameter param)
        {
            base.EnterState(param);
            enemyController.canTrigger = false;
            enemyController.animator.SetTrigger(_animatorParameterIdDead);
        }

        public override void Update()
        {
            base.Update();
            if (Time.time - stateStartTime > 1f)
            {
                Object.Destroy(enemyController.gameObject);
            }
        }
    }
}