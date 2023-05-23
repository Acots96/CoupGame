
namespace CoupGame.GameLogic.FSM
{
	// Base class for any state of the FiniteStateMachine
	public abstract class State
	{
		protected FiniteStateMachine Fsm;

		protected State(FiniteStateMachine fsm)
		{
			Fsm = fsm;
		}

		public abstract void Enter();
		public abstract State Update();
		public abstract void Exit();
	}
}
