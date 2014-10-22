using UnityEngine;
using System.Collections;

public class FSM  : MonoBehaviour {
	private State CurrentState = null;
	
	[System.Serializable]
	public class State {
		[HideInInspector]
		public FSM fsm;

		public Transform node;
		public virtual void Enter   () {}
		public virtual void Execute () {}
		public virtual void FixedExecute () {}
		public virtual void Exit    () {}
		
		public void ChangeState(State _next) {
			fsm.ChangeState(_next);
		}
	}
	
	public void  Update() {
		if (CurrentState != null) 
			CurrentState.Execute();
	}
	
	public void FixedUpdate() {
		if(CurrentState != null)
			CurrentState.FixedExecute();
	}
	
	public void  ChangeState(State NewState) {
		if (CurrentState != null) {
			CurrentState.Exit();
			if(CurrentState.node != null)
				CurrentState.node.gameObject.SetActive(false);
		}
		
		CurrentState = NewState;
		
		if (CurrentState != null) {
			CurrentState.fsm = this;
			if(CurrentState.node != null)
				CurrentState.node.gameObject.SetActive(true);
			CurrentState.Enter();
		}
	}
	
	public State GetCurrentState() {
		return CurrentState;
	}
}
