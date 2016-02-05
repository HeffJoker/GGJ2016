using UnityEngine;
using System;
using System.Collections;
using SF_Tools.Managers;
using Foundation.Messenger;
using SF_Tools.Messages;
using UnityEngine.SceneManagement;

namespace SF_Tools.GUI
{
	public class ButtonActions : MonoBehaviour {

		#region Public Interface

		public void PopGameState()
		{
			GameStateManager.Instance.PopState();
		}

		public void PushGameState(GameState state)
		{
			GameStateManager.Instance.PushState(state);
		}

		public void TransitionGameState(GameState gameState)
		{
			GameStateManager.Instance.MoveToState(gameState);
		}

		public void PushGameState(string state)
		{
			object stateEnum = Enum.Parse(typeof(GameState), state);

			if(stateEnum != null)
				GameStateManager.Instance.PushState((GameState)stateEnum);
		}

		public void TransitionGameState(string state)
		{
			object stateEnum = Enum.Parse(typeof(GameState), state);

			if(stateEnum != null)
				GameStateManager.Instance.MoveToState((GameState)stateEnum);
		}

		public void TransitionGUIState(string guiState)
		{
			GUIStateManager.Instance.MoveToGUIState(guiState);
		}

		public void PushGUIState_NoExit(string stateName)
		{
			GUIStateManager.Instance.PushState(stateName, false);
		}

		public void PushGUIState_Exit(string stateName)
		{
			GUIStateManager.Instance.PushState(stateName, true);
		}

		public void PopGUIState()
		{
			GUIStateManager.Instance.PopState();
		}

		public void ToTitleScreen()
		{
			Message_StateTransition message = new Message_StateTransition(GameStateManager.Instance.CurrState.StateType, GameState.TITLE);
			message.Publish();

            SaveData();
		}

        public void ToGame()
        {
            Message_StateTransition message = new Message_StateTransition(GameStateManager.Instance.CurrState.StateType, GameState.PLAY);
            message.Publish();
        }

		public void ToLoading()
		{
			TransitionGameState(GameState.LOADING);
		}

		public void QuitGame()
		{
			Application.Quit();
		}

		public void LoadScene(string name)
		{
            if (!string.IsNullOrEmpty(name))
                SceneManager.LoadScene(name, LoadSceneMode.Single);
		}

        public void UnloadScene(string name)
        {
            if (!string.IsNullOrEmpty(name))
                SceneManager.UnloadScene(name);
        }

		public void LoadSceneAdditive(string name)
		{
            if (!string.IsNullOrEmpty(name))
                SceneManager.LoadScene(name, LoadSceneMode.Additive);
		}

		public void ClearLevel()
		{
			LevelManager.Instance.Clear();
		}

        public void SaveData()
        {
            DataManager.Instance.SaveData(true);
        }

        public void SetPlayerPref_True(string name)
        {
            PlayerPrefs.SetInt(name, 1);
            PlayerPrefs.Save();
        }

        public void SetPlayerPref_False(string name)
        {
            PlayerPrefs.SetInt(name, 0);
            PlayerPrefs.Save();
        }

        public void TogglePlayerPref(string name)
        {
            if(PlayerPrefs.HasKey(name))
            {
                bool isOn = (PlayerPrefs.GetInt(name, 1) > 0);

                if (isOn)
                    SetPlayerPref_False(name);
                else
                    SetPlayerPref_True(name);
            }
            else
            {
                SetPlayerPref_True(name);
            }
        }

        public void SetNextLevelToLoad(LevelInfo level)
        {
            Messenger.Publish(new Message_LevelLoad(level, true));
        }

		#endregion
	}
}
