using UnityEngine;

namespace StateMachine
{
    public partial class StateMachineParameter
    {
    }

    public class AbstractState
    {
        protected float stateStartTime { get; private set; }

        public AbstractState()
        {
        }

        public virtual void EnterState(StateMachineParameter param)
        {
            stateStartTime = Time.time;
        }

        public virtual void ExitState()
        {

        }

        public virtual void Update()
        {

        }
    }

    public abstract class AbstractStateMachine
    {
        public AbstractState currentState { get; protected set; }

        public AbstractStateMachine()
        {
        }

        public virtual void Start(StateMachineParameter param = null)
        {
            currentState.EnterState(param);
        }

        public virtual void ChangeState(AbstractState newState, StateMachineParameter param = null)
        {
            if (currentState != null)
            {
                currentState.ExitState();
            }
            currentState = newState;

            if (currentState != null)
            {
                currentState.EnterState(param);
            }
        }

        public virtual void Update()
        {
            if (currentState != null)
            {
                currentState.Update();
            }
        }

        public virtual bool CheckState(AbstractState state)
        {
            return currentState == state;
        }
    }
}