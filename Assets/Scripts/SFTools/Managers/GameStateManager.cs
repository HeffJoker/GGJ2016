using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SF_Tools.GUI;
using SF_Tools.Messages;
using Foundation.Messenger;

namespace SF_Tools.Managers
{
	[Serializable]
    public enum GameState
    {
        UNDEFINED = -1,
        LOGO,
        TITLE,
        LOADING,
        PLAY,
        PAUSE,        
        GAME_OVER
    }

	public class GameStateManager : SingletonBase<GameStateManager> {

        #region Editor Properties

        public GameState StartState = GameState.UNDEFINED;

        #endregion

        #region Private Members

		private Dictionary<GameState, State> m_StateMap = new Dictionary<GameState, State>();
		private Stack<State> stateStack = new Stack<State>();

		#endregion

		#region Public Properties

		public State CurrState
		{
			get { return stateStack.Peek(); }
		}

        #endregion

        #region Public Routines

        public void MoveToState(GameState state)
		{
			State currState = null;
			State nextState;

			if(m_StateMap.ContainsKey(state))
				nextState = m_StateMap[state];
			else
			{
				Debug.LogError(string.Format("[GAME_STATE_MANAGER]: State {0} does not exist in the FSM!", state.ToString()));
				return;
			}

			while(stateStack.Count > 0)
			{
				currState = stateStack.Pop();
				currState.DoExit(nextState);
			}

			stateStack.Push(nextState);
			nextState.DoEnter(currState, false);
		}

		public void PushState(GameState state)
		{
			State nextState, currState = null;
			
			if(m_StateMap.ContainsKey(state))
				nextState = m_StateMap[state];
			else
			{
				Debug.LogError(string.Format("[GAME_STATE_MANAGER]: State {0} does not exist in the FSM!", state.ToString()));
				return;
			}

			if(stateStack.Count > 0)
			{
				currState = stateStack.Peek();

				if(currState.InputHandler != null)
					currState.InputHandler.enabled = false;
			}

			stateStack.Push(nextState);
			nextState.DoEnter(currState, true);
		}

		public void PopState()
		{
			State currState = stateStack.Pop();
			State nextState = (stateStack.Count > 0 ? stateStack.Peek() : null);

			currState.DoExit(nextState); 

			if(nextState != null && nextState.InputHandler != null)
				nextState.InputHandler.enabled = true;
		}

		public void GameOver()
		{
			if(CurrState.StateType != GameState.GAME_OVER)
			{
				MoveToState(GameState.GAME_OVER);
				Message_GameOver gameOver = new Message_GameOver();
				Messenger.Publish(gameOver);
			}
		}

		#endregion

		#region Private Routines

		// Use this for initialization
		void Start () {

            State[] states = GetComponents<State>();

            foreach (State state in states)
            {
                if(!m_StateMap.ContainsKey(state.StateType))
                    m_StateMap.Add(state.StateType, state);
            }

			MoveToState(StartState);
		}

		#endregion

        #region Private Routines

        protected override void OnDestroy_Sub()
        {
            foreach (KeyValuePair<GameState, State> item in m_StateMap)
                item.Value.OnDestroy();           
        }

        #endregion
    }

	public abstract class State : MonoBehaviour
	{
		#region Editor Properties

		public float WaitTimeout = 5f;
		public bool WaitGUIEnter = true;
		public bool WaitGUIExit = true;
		public GUIState GUIState;
		public MonoBehaviour InputHandler;

		#endregion

		#region Private Routines

		private string prevState = string.Empty;
		private float currTime = 0;

        #endregion

        #region Public Properties

        public abstract GameState StateType
        {
            get;
        }

        #endregion

        #region Public Interface

        public virtual void Start()	
		{
			if(InputHandler != null)
				InputHandler.enabled = false;
		}

		public void DoEnter(State prevState, bool pushGUIState)
		{
			if(pushGUIState)
				GUIStateManager.Instance.PushState(GUIState.Name, false);
			else
				GUIStateManager.Instance.MoveToGUIState(GUIState.Name);

			StartCoroutine(FinishEnter(prevState));
		}

		public void DoExit(State nextState)
		{
			StartCoroutine(FinishExit(nextState));
		}

		public abstract void Enter(State prevState);

		public abstract void Exit(State nextState);

        public virtual void OnDestroy() { }

		#endregion

		#region Private Routines

		private IEnumerator FinishEnter(State prevState)
		{
            if (WaitGUIEnter)
			{
				currTime = 0;

				while(!GUIState.EnterDone && currTime <= WaitTimeout)
				{
					currTime += Time.unscaledDeltaTime;
					yield return null;
				}
			}


            if (InputHandler != null)
                InputHandler.enabled = true;

            Enter(prevState);
        }

		private IEnumerator FinishExit(State nextState)
		{
            if (InputHandler != null)
                InputHandler.enabled = false;

            if (WaitGUIExit)
			{
				currTime = 0;

				while(!GUIState.ExitDone && currTime <= WaitTimeout)
				{
					currTime += Time.unscaledDeltaTime;
					yield return null;
				}
			}
			
			Exit(nextState);
		}

		#endregion
	}
}
