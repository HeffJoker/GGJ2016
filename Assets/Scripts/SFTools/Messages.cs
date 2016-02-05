//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18444
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using Foundation.Messenger;
using SF_Tools.Managers;

namespace SF_Tools.Messages
{
    public class Message_GameStart : IMessengerObject
    {
    }

    public class Message_GameOver : IMessengerObject
	{
	}

	public class Message_StateTransition : IMessengerObject
	{
        #region Private Members

        private GameState nextState = GameState.UNDEFINED;
        private GameState prevState = GameState.UNDEFINED;
		
		#endregion

		#region Public Properties

		public GameState NextState
		{
			get { return nextState; }
		}

		public GameState PrevState
		{
			get { return prevState; }
		}

		#endregion

		#region Public Interface

		public Message_StateTransition(GameState prev, GameState next)
		{
			prevState = prev;
			nextState = next;
		}

		#endregion
	}

    [CachedMessage]
    public class Message_DataLoaded : IMessengerObject
    {
        #region Public Properties

        public SF_Tools.Managers.DataManager.SavedData LoadedData
        {
            get;
            set;
        }

        #endregion

        #region Public Interface

        public Message_DataLoaded(DataManager.SavedData data)
        {
            LoadedData = data;
        }

        #endregion  
    }

    public class Message_LevelLoad : IMessengerObject
    {
        #region Public Properties

        public LevelInfo Level
        {
            get;
            private set;
        }

        public bool IsOverride
        {
            get;
            private set;
        }

        #endregion

        #region Public Interface

        public Message_LevelLoad(LevelInfo levelInfo, bool isOverride)
        {
            Level = levelInfo;
            IsOverride = isOverride;
        }

        #endregion
    }

    public class Message_DoUpgrade : IMessengerObject
    {
        #region Public Properties

        public DataManager.SavedData OldData
        {
            get;
            private set;
        }

        #endregion

        #region Public Interface

        public Message_DoUpgrade(DataManager.SavedData oldData)
        {
            OldData = oldData;
        }

        #endregion  
    }
}