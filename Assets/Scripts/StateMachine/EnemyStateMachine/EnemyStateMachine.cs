namespace StateMachine
{
    public partial class StateMachineParameter
    {
        public bool forcedReturning;
    }

    public class AbstractEnemyState : AbstractState
    {
        protected EnemyStateMachine stateMachine { get; private set; }
        protected EnemyController enemyController => stateMachine.enemyController;
        protected PlayerController player => GameController.Instance.player;

        public AbstractEnemyState(EnemyStateMachine stateMachine) : base()
        {
            this.stateMachine = stateMachine;
        }
    }

    public class EnemyStateMachine : AbstractStateMachine
    {
        public EnemyIdleState EnemyIdleState { get; private set; }
        public EnemyWanderingState EnemyWanderingState { get; private set; }
        public EnemyReturningState EnemyReturningState { get; private set; }
        public EnemyChasingState EnemyChasingState { get; private set; }
        public EnemyAttackState EnemyAttackState { get; private set; }
        public EnemyFlinchedState EnemyFlinchedState { get; private set; }

        public EnemyController enemyController { get; private set; }

        public EnemyStateMachine(EnemyController enemyController) : base()
        {
            this.enemyController = enemyController;

            EnemyIdleState = new EnemyIdleState(this);
            EnemyWanderingState = new EnemyWanderingState(this);
            EnemyReturningState = new EnemyReturningState(this);
            EnemyChasingState = new EnemyChasingState(this);
            EnemyAttackState = new EnemyAttackState(this);
            EnemyFlinchedState = new EnemyFlinchedState(this);

            currentState = EnemyIdleState;
        }
    }
}