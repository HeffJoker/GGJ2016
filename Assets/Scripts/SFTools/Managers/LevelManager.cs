using Foundation.Messenger;
using SF_Tools.Messages;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SF_Tools.Managers
{
    public class LevelManager : SingletonBase<LevelManager>
	{
		#region Editor Properties

		public List<LevelInfo> Levels;
		public string GameObjectIdentifier;
        public List<LevelButtonLink> LevelButtons;

        #endregion

        #region Private Members

        private string lastLoadedScene = string.Empty;
        private LevelInfo levelLoadOverride = null;

		#endregion

        #region Public Properties

        public LevelInfo CurrentLevel
        {
            get;
            private set;
        }

        #endregion

        #region Public Interface

        public AsyncOperation SetupLevel()
		{
			if(Levels.Count <= 0)
			{
				Debug.LogError("[LEVEL MANAGER]: No Levels were registered...");
				return null;
			}

            if (levelLoadOverride != null)
                CurrentLevel = levelLoadOverride;
            else
            {
                List<LevelInfo> enabledLevels = Levels.FindAll(x => x.Enabled);

                if (enabledLevels.Count <= 0)
                {
                    Debug.LogError("[LEVEL_MANAGER]: Could not find any enabled levels...");
                    return null;
                }

                CurrentLevel = SelectLevel(enabledLevels);
            }

			string levelToLoad = CurrentLevel.LevelName;
            lastLoadedScene = levelToLoad;

			return SceneManager.LoadSceneAsync(levelToLoad, LoadSceneMode.Additive);
		}

        public AsyncOperation SetupLevel(string levelName)
        {
            LevelInfo level;

            if (levelLoadOverride != null)
            {
                level = levelLoadOverride;
                levelLoadOverride = null;
            }
            else
                level = Levels.Find(x => x.LevelName == levelName);
            
            if (level == null)
                level = new LevelInfo(levelName, true);

            CurrentLevel = level;
            string levelToLoad = CurrentLevel.LevelName;
            lastLoadedScene = levelToLoad;

            return SceneManager.LoadSceneAsync(levelToLoad, LoadSceneMode.Additive);
        }

		public void Clear()
		{
            SceneManager.UnloadScene(lastLoadedScene);
			//GameObject objectList = GameObject.Find(GameObjectIdentifier);
			//DestroyImmediate(objectList);
		}

        [Subscribe]
        public void HandleDataLoaded(Message_DataLoaded message)
        {
        }

        [Subscribe]
        public void HandleLevelLoad(Message_LevelLoad message)
        {
            if(message.IsOverride)
            {
                levelLoadOverride = message.Level;
            }
        }

        #endregion

        #region Private Routines

        private LevelInfo SelectLevel(List<LevelInfo> enabledLevels)
        {
            int lvlIndex = UnityEngine.Random.Range(0, enabledLevels.Count);

            return enabledLevels[lvlIndex];
        }

	  	protected override void OnWake ()
		{
            Messenger.Subscribe(this);

            for(int i = 0; i < LevelButtons.Count; ++i)
            {
                LevelButtons[i].ButtonObj.SetActive(false);
            }

            for(int i = 0; i < Levels.Count; ++i)
            {
                if(Levels[i].IsEnabled)
                    HandleLevelButton(Levels[i]);
            }
		}

        protected override void OnDestroy_Sub()
        {
            Messenger.Unsubscribe(this);
        }

        public void HandleLevelButton(LevelInfo level)
        {
            for (int i = 0; i < LevelButtons.Count; ++i)
            {
                if (LevelButtons[i].Level == level)
                {
                    LevelButtons[i].ButtonObj.SetActive(true);
                    break;
                }
            }
        }

		#endregion

	}

    [Serializable]
    public struct LevelButtonLink
    {
        public GameObject ButtonObj;
        public LevelInfo Level;
    }
}