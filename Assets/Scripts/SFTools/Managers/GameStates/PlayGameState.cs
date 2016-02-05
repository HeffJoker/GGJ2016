using UnityEngine;
using System.Collections;
using SF_Tools.GUI;
using SF_Tools.Spawners;
using SF_Tools.Messages;
using Foundation.Messenger;
using System;

namespace SF_Tools.Managers
{
	public class PlayGameState : State {

		#region Editor Properties

		public Spawner[] spawners;
		public string PauseState;
        public float InputWait = 0.1f;

        #endregion

        #region Public Properties

        public override GameState StateType
        {
            get
            {
                return GameState.PLAY;
            }
        }

        #endregion  

        #region Public Interface

        public override void Enter (State prevState)
		{
			if(prevState != null && prevState.StateType == GameState.PAUSE)
				return;
            
            Messenger.Publish(new Message_GameStart());

            StartCoroutine(EnableInput());
		}

		public override void Exit (State nextState)
		{
			if(nextState != null && nextState.StateType == GameState.PAUSE)
				return;
		}

        #endregion

        #region Private Routines

        IEnumerator EnableInput()
        {
            if (InputHandler != null)
                InputHandler.enabled = false;

            yield return new WaitForSeconds(InputWait);

            if (InputHandler != null)
                InputHandler.enabled = true;
        }

        #endregion
    }
}