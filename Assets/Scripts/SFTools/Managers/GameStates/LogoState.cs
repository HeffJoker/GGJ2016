using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using UnityEngine;
using SF_Tools.Messages;
using Foundation.Messenger;

namespace SF_Tools.Managers
{
	public class LogoState : State
    {
        #region Editor Properties

        public float TimeToWait = 2f;

        #endregion

        #region Public Properties

        public override GameState StateType
        {
            get
            {
                return GameState.LOGO;
            }
        }

        #endregion

        #region Public Interface

        public override void Enter(State prevState)
        {
            StartCoroutine(ShowLogo());
        }

        public override void Exit(State nextState)
        {
        }

        #endregion

        #region Private Routines

        private IEnumerator ShowLogo()
        {
            yield return new WaitForSeconds(TimeToWait);
                        
            Message_StateTransition message = new Message_StateTransition(GameStateManager.Instance.CurrState.StateType, GameState.TITLE);
            message.Publish();
            GameStateManager.Instance.MoveToState(GameState.LOADING);
        }

        #endregion
    }
}
