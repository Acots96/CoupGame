
namespace CoupGame.GameLogic.FSM
{
	// States machine class to manage the states
	public class FiniteStateMachine
	{
		private State _currentState;

		private CoupGame _game;
		public CoupGame Game => _game;

		public FiniteStateMachine(CoupGame game)
		{
			_game = game;
		}

		public void Start()
		{
			_currentState = new PlayerTakeActionState(this);
			_currentState.Enter();
		}

		public void Update()
		{
			State nextState = _currentState.Update();

			if (nextState != null)
			{
				_currentState.Exit();
				_currentState = nextState;
				_currentState.Enter();
			}
		}
	}
}
