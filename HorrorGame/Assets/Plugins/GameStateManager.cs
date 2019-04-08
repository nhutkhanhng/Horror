#if  false
using UnityEngine;

namespace DansGameTools
{
abstract public class GameStateManager //: MonoBehaviour 	
	{
	private GameState curState;

	// -------------------
	public void StartState(GameState newState, int intParam, object objParam)
		{
		if (this.curState != null)
			this.curState.OnExit();

		if ((this.curState = newState) != null)
			this.curState.OnStart(
		}
		

	// ----------------
	public void UpdateState()
		{
		if (this.curState != null)
			this.curState.OnUpdate();
		}


	}
}
#endif
