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
            animator.SetTrigger(_animatorParameterIdFlinched);
            _flinched = false;
            enemyController.RotateTowards(player.transform.position);
        }

        public override void Update()
        {
            base.Update();

            enemyController.FlinchBackwards();

            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
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
            animator.SetTrigger(_animatorParameterIdDead);
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

    public class EnemyStaggeredState : AbstractEnemyState
    {
        private int _animatorParameterIdFlinched;

        public EnemyStaggeredState(EnemyStateMachine stateMachine) : base(stateMachine)
        {
            _animatorParameterIdFlinched = Animator.StringToHash("Flinched");
        }

        public override void EnterState(StateMachineParameter param)
        {
            base.EnterState(param);
            animator.SetBool(_animatorParameterIdFlinched, true);
            enemyUnit.StaggerDuration.value = enemyUnit.StaggerDuration.maxValue;
        }

        public override void ExitState()
        {
            base.ExitState();
            animator.SetBool(_animatorParameterIdFlinched, true);
        }

        public override void Update()
        {
            base.Update();
            enemyUnit.StaggerDuration.value -= Time.deltaTime;
            if (enemyUnit.StaggerDuration.value <= 0f)
            {
                enemyController.gameUnit.Stagger.value = 0;
                stateMachine.ChangeState(stateMachine.EnemyIdleState);
            }
        }
    }
}