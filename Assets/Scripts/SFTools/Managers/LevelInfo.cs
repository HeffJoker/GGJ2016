using System;
using UnityEngine;

namespace SF_Tools.Managers
{
    [Serializable]
    public class LevelInfo : ScriptableObject
    {
        #region Editor Properties

        public string LevelName = string.Empty;
        public bool Enabled = false;

        #endregion

        #region Public Interface

        public LevelInfo(string name, bool isEnabled)
        {
            LevelName = name;
            Enabled = isEnabled;
        }


        public bool IsEnabled
        {
            get { return Enabled; }
            set { Enabled = value; }
        }

        #endregion
    }
}
