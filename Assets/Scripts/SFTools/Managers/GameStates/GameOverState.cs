using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using SF_Tools.GUI;
using SF_Tools.Messages;
using Foundation.Messenger;
using System;

namespace SF_Tools.Managers
{
	public class GameOverState : State {

        #region Editor Properties

        #endregion

        #region Private Members

        #endregion

        #region Public Properties

        public override GameState StateType
        {
            get
            {
                return GameState.GAME_OVER;
            }
        }

        #endregion

        #region Public Interface

        public override void Enter (State prevState)
		{
		}

		public override void Exit (State nextState)
		{
		}

        #endregion

        #region Private Routines

        #endregion


    }
}