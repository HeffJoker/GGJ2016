using UnityEngine;
using System.Collections;
using SF_Tools.GUI;
using Foundation.Messenger;
using System;

namespace SF_Tools.Managers
{
	public class MainMenuState : State {

        #region Editor Properties

        #endregion

        #region Private Members

        #endregion

        #region Public Properties

        public override GameState StateType
        {
            get
            {
                return GameState.TITLE;
            }
        }

        #endregion

        #region Public Interface

        public override void Enter (State prevState)
		{
            Messenger.ClearCache();
            //DataManager.Instance.LoadData(true);
            //RewardManager.Instance.Initialize();
            Animator animator = GUIState.GetComponent<Animator>();
            animator.SetBool("DoTap", true);
			//LevelManager.Instance.Clear();
			Camera.main.enabled = true;
		}

		public override void Exit (State nextState)
		{
            Animator animator = GUIState.GetComponent<Animator>();
            animator.SetBool("DoTap", false);
		}

		#endregion



	}
}