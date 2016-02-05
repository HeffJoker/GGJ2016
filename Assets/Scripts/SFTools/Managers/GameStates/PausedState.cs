using UnityEngine;
using System.Collections;
using SF_Tools.GUI;
using UnityEngine.UI;
using System;

namespace SF_Tools.Managers
{
	public class PausedState : State {

		#region Editor Properties

		public float UnpauseWaitTime = 1f;
        public Button PauseButton;
        public float InputWait = 0.1f;

        #endregion

        #region Private Members

        #endregion

        #region Public Properties

        public override GameState StateType
        {
            get
            {
                return GameState.PAUSE;
            }
        }

        #endregion

        #region Public Interface

        public override void Enter (State prevState)
		{
            if (PauseButton != null)
                PauseButton.interactable = false;         
			//UnitManager.Instance.PauseAll();
            StopAllCoroutines(); // (DoUnpause());
            StartCoroutine(EnableInput());

            Animator animator = GUIState.GetComponent<Animator>();
            animator.SetBool("Idle", true);

			Time.timeScale = 0;
		}
		
		public override void Exit (State nextState)
		{
            if (PauseButton != null)
                PauseButton.interactable = false;

            Animator animator = GUIState.GetComponent<Animator>();
            animator.SetBool("Idle", false);
			GUIStateManager.Instance.ExitState(GUIState.Name);
			StartCoroutine(DoUnpause());
		}
		
		#endregion

		#region Private Routines

		public IEnumerator DoUnpause()
		{
			float currTime = UnpauseWaitTime;

			while(currTime > 0)
			{
				currTime -= Time.unscaledDeltaTime;
				yield return null;
			}

			Time.timeScale = 1f;
		}

        public IEnumerator EnableInput()
        {
            if (InputHandler != null)
                InputHandler.enabled = false;

            float currTime = InputWait;

            while(currTime > 0)
            {
                currTime -= Time.unscaledDeltaTime;
                yield return null;
            }
            
            if (InputHandler != null)
                InputHandler.enabled = true;
        }

		#endregion
	}
}