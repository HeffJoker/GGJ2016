using UnityEngine;
using System.Collections;
using SF_Tools.GUI;
using Foundation.Messenger;
using SF_Tools.Messages;
using UnityEngine.SceneManagement;
using System;

namespace SF_Tools.Managers
{
	public class LoadingState : State {
		
		#region Editor Properties

		public float LoadTime = 2f;
		public string playScene = string.Empty;
		public string titleScene = string.Empty;

		#endregion	
		
		#region Private Members

		private bool toTitle = true;
        private bool isReloading = false;

        #endregion

        #region Public Properties

        public override GameState StateType
        {
            get
            {
                return GameState.LOADING;
            }
        }

        #endregion

        #region Public Interface

        public override void Enter (State prevState)
		{
            if (isReloading)
            {
                toTitle = true;
                isReloading = false;
            }
            else
            {
                if (prevState == null)
                    toTitle = true;
                else if (prevState.StateType == GameState.TITLE)
                    toTitle = false;
            }

            if (toTitle)
                StartCoroutine(LoadGame(GameState.TITLE));
            else
				StartCoroutine(LoadGame(GameState.PLAY));
		}
		
		public override void Exit (State nextState)
		{
		}

		public override void Start ()
		{
			base.Start();
			Messenger.Subscribe(this);
		}

        public override void OnDestroy()
        {
            Messenger.Unsubscribe(this);
        }

		#region Message Handlers
		
		[Subscribe]
		public void HandleTransition(Message_StateTransition message)
		{
            if (message.NextState == GameState.TITLE)
            {
                toTitle = true;
            }
            else
            {
                toTitle = false;
            }
				
		}

        [Subscribe]
        public void HandleLevelLoad(Message_LevelLoad message)
        {
            if (message.IsOverride)
                isReloading = true;
        }
		
		#endregion
					
		#endregion

		#region Private Routines

		private IEnumerator LoadGame(GameState nextState)
		{			
			AsyncOperation loadVar = null;

			if(toTitle)
			{
				loadVar = SceneManager.LoadSceneAsync(titleScene, LoadSceneMode.Additive);
				SceneManager.UnloadScene(titleScene);
			}
			else
				loadVar = SceneManager.LoadSceneAsync(playScene, LoadSceneMode.Additive);
			
            float currTime = LoadTime;

            while (!loadVar.isDone || currTime > 0)
            {
                currTime -= Time.unscaledDeltaTime;
                yield return null;
            }

			GameStateManager.Instance.MoveToState(nextState);
		}

		#endregion
		
		
		
	}
}
