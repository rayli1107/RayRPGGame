using UnityEngine;

namespace StateMachine
{
    public class EnemyAttackState : AbstractEnemyState
    {
        private int _animatorParameterIdAttack;
        private int _animatorStateIdAttack;
        private const float _attackRegisterTime = 0.5f;
        private bool attacked;
        public EnemyAttackState(EnemyStateMachine stateMachine) : base(stateMachine)
        {
            _animatorParameterIdAttack = Animator.StringToHash("Attack");
            _animatorStateIdAttack = Animator.StringToHash("Attack");
        }

        public override void EnterState(StateMachineParameter param)
        {
            base.EnterState(param);
            enemyController.animator.SetTrigger(_animatorParameterIdAttack);
            attacked = false;
        }

        public override void Update()
        {
            base.Update();

            AnimatorStateInfo stateInfo = enemyController.animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.shortNameHash == _animatorStateIdAttack &&
                !attacked &&
                stateInfo.normalizedTime >= _attackRegisterTime)
            {
                if (enemyController.CanHitPlayer())
                {
                    player.OnHit(enemyController.gameUnit.Attack.value);
                }
                attacked = true;
            }

            if (Time.time - stateStartTime >= enemyController.attackCooldown)
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
}