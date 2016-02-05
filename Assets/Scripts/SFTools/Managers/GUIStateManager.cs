using UnityEngine;
using System;
using System.Collections.Generic;
using SF_Tools.Managers;

namespace SF_Tools.GUI
{
	public class GUIStateManager : SingletonBase<GUIStateManager> {

		#region Editor Properties

		//public List<GUIState> States = new List<GUIState>();
		public GUIState StartState;
		public bool Verbose = false;

		#endregion

		#region Private Members

		private Stack<GUIState> stateStack = new Stack<GUIState>();
		private Dictionary<string, GUIState> stateMap = new Dictionary<string, GUIState>();
		private GUIState prevState;

		#endregion

		#region Public Properties

		public GUIState PrevState
		{
			get { return prevState; }
		}

		public GUIState CurrState
		{
			get { return stateStack.Peek(); }
		}

		#endregion

		#region Public Interface

		public void Awake()
		{
            /*
			GUIState[] states = GetComponentsInChildren<GUIState>();

			foreach(GUIState state in states)
				stateMap.Add(state.Name, state);

			foreach(GUIState state in states)
				state.Initialize();*/
			//States.ForEach(x => x.Initialize());
		}
			
		public float MoveToGUIState(string stateName)
		{
			if(Verbose)
				Debug.Log(string.Format("[GUI_STATE_MANAGER]: Moving to state {0}", stateName.ToUpper()));

			return StartTransition(stateName, true, true);
		}

		public float PushState(string stateName, bool exitCurrState)
		{
			if(Verbose)
				Debug.Log(string.Format("[GUI_STATE_MANAGER]: Pushing state {0} onto stack. {1} states on stack.", stateName.ToUpper(), stateStack.Count));

			return StartTransition(stateName, false, exitCurrState);
		}

		public void PopState()
		{
			if(stateStack.Count <= 1)
				return;

			GUIState currState = stateStack.Pop();

			if(Verbose)
				Debug.Log(string.Format("[GUI_STATE_MANAGER]: Popping state {0} from the stack. {1} states on stack.", currState.Name.ToUpper(), stateStack.Count));

			currState.Exit();
			Transition();
		}

		public void ExitState(string stateName)
		{
			if(!stateMap.ContainsKey(stateName))
			{
				Debug.LogError(string.Format("[GUI_STATE_MANAGER]: State {0} does not exist in FSM!!", stateName.ToUpper()));
				return;
			}

			GUIState currState = stateMap[stateName];

			if(stateStack.Peek() == currState)
			{
				stateStack.Pop();
				if(Verbose)
					Debug.Log(string.Format("[GUI_STATE_MANAGER]: Popping state {0} from the stack. {1} states on stack.", currState.Name.ToUpper(), stateStack.Count));
			}

			if(Verbose)
				Debug.Log(string.Format("[GUI_STATE_MANAGER]: Exiting state {0}.", currState.Name));

			currState.Exit();
		}

		public void ClearStack(string exception)
		{
			GUIState keepState = null;
			GUIState currState = null;

			if(Verbose)
			{
				Debug.Log("[GUI_STATE_MANAGER]: Clearing state stack.");

				if(!string.IsNullOrEmpty(exception))
					Debug.Log(string.Format("[GUI_STATE_MANAGER]: Keeping state {0}.", exception));
			}

			if(stateMap.ContainsKey(exception))
				keepState = stateMap[exception];			

			while(stateStack.Count > 0)
			{
				currState = stateStack.Pop();
				if(currState != keepState)
					currState.Hide();
			}

			if(keepState != null)
				stateStack.Push(keepState);
		}

        public void AddState(GUIState newState)
        {
            if (!stateMap.ContainsKey(newState.Name))
            {
                stateMap.Add(newState.Name, newState);
                newState.Initialize();
            }
        }

        public void RemoveState(GUIState oldState)
        {
            if (stateMap.ContainsKey(oldState.Name))
                stateMap.Remove(oldState.Name);
        }

		#endregion

		#region Private Routines

		private float StartTransition(string stateName, bool popCurrState, bool exitCurrState)
		{
			if(!stateMap.ContainsKey(stateName))
			{
				Debug.LogError(string.Format("[GUI_STATE_MANAGER]: State {0} does not exist in FSM!!", stateName.ToUpper()));
				return 0;
			}

			GUIState nextState = stateMap[stateName];
			GUIState currState = null;

			if(stateStack.Contains(nextState))
			{
				while(stateStack.Peek() != nextState)
				{
					currState = stateStack.Pop();

					if(!currState.IsHidden)
						currState.Exit();
				}

				currState = stateStack.Peek();
				currState.Enter();
			}
			else
			{
				if(stateStack.Count > 0)
					currState = stateStack.Peek();

				if(currState != null)
				{
					if(exitCurrState)
						currState.Exit();

					if(popCurrState)
						stateStack.Pop();

					if(Verbose)
						Debug.Log(string.Format("[GUI_STATE_MANAGER]: Exiting state {0}.", currState.Name));
				}
				
				stateStack.Push(nextState);
				
				if(Verbose)
					Debug.Log(string.Format("[GUI_STATE_MANAGER]: Pushing state {0} onto stack. {1} states on stack.", nextState.Name, stateStack.Count));
				
				Transition();
			}

			return 0f;
		}

		private void Transition()
		{
			GUIState currState = stateStack.Peek();
			currState.Enter();	
			prevState = currState;

			if(Verbose)
				Debug.Log(string.Format("[GUI_STATE_MANAGER]: Transitioning to state {0}.", currState.Name));
		}

        protected override void OnDestroy_Sub()
        {
            stateMap.Clear();
            stateStack.Clear();
        }

		#endregion
	}

	/*
	[Serializable]
	public class GUIState
	{
		public string Name = string.Empty;
		public Canvas Canvas = null;
		public float TransitionOnTime = 0f;
		public float TransitionOffTime = 0f;

		#region Private Members

		private List<Animator> animators = new List<Animator>();

		#endregion

		#region Public Interface

		public void Start()
		{
			Animator topLevelAnimator = Canvas.GetComponent<Animator>();

			if(topLevelAnimator != null)
				animators.Add(Canvas.GetComponent<Animator>());

			animators.AddRange(Canvas.GetComponentsInChildren<Animator>());
		}

		public void Enter()
		{
			foreach(Animator animator in animators)
				animator.SetTrigger("IsEntering");
		}

		public void Exit()
		{
			foreach(Animator animator in animators)
				animator.SetTrigger("IsLeaving");
		}

		public void Hide()
		{
			foreach(Animator animator in animators)
			{
				animator.ResetTrigger("IsEntering");
				animator.ResetTrigger("IsLeaving");
				animator.SetTrigger("IsHiding");
			}
		}

		#endregion
	}
	*/
}