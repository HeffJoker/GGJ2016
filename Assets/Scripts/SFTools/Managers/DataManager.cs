using Foundation.Messenger;
using SF_Tools.Messages;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace SF_Tools.Managers
{
    public class DataManager : SingletonBase<DataManager>
	{
		#region Editor Properties

		public string SaveFile;

		#endregion

		#region Private Members

		private SavedData playerData;

		#endregion

        #region Public Interface

        public void SaveData(bool notifySave)
		{
            //BackupData();

			if(playerData == null)
                InitializeData();

            if(notifySave)
                OnDataSave();

			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Create(string.Format("{0}/{1}", Application.persistentDataPath, SaveFile));

			bf.Serialize(file, playerData);
			file.Close();

            PlayerPrefs.Save();
		}

        public void BackupData()
        {
            try
            {
                string filename = string.Format("{0}/{1}", Application.persistentDataPath, SaveFile);

                if (File.Exists(filename))
                    File.Copy(filename, filename + ".bak");
            }
            catch(Exception)
            { }
        }

		public void LoadData(bool notifyLoad, bool loadBackup)
		{
            string filename;

            if (false) //loadBackup)
                filename = string.Format("{0}/{1}.bak", Application.persistentDataPath, SaveFile);
			else
                filename = string.Format("{0}/{1}", Application.persistentDataPath, SaveFile);

            FileStream file = null;

            try
			{
				if(File.Exists(filename))
				{
					BinaryFormatter bf = new BinaryFormatter();
                    file = File.Open(filename, FileMode.Open);
					playerData = (SavedData)bf.Deserialize(file);
					file.Close();
				}
                else
                {
                    playerData = InitializeData();
                }
			}
			catch(Exception ex)
			{
                if (file != null)
                    file.Close();

                Debug.LogException(ex);
            }

            if(notifyLoad)
			    OnDataLoad();
		}

		public void Clear()
		{
            playerData = InitializeData();
		}
       
		#region Message Handlers

		#endregion

		#endregion

		#region Private Routines

		protected override void OnWake()
		{
            Messenger.Subscribe(this);
            LoadData(true, true);
		}

		protected override void OnDestroy_Sub ()
		{
			//SaveData(true);
            Messenger.Unsubscribe(this);
		}

        private void OnApplicationQuit()
        {
            //OnDestroy_Sub();
            SaveData(true);
        }

		private void OnDataLoad()
		{
            Messenger.Publish(new Message_DataLoaded(playerData));
		}

        private void OnDataSave()
        {
        }

        private SavedData InitializeData()
        {
            return new SavedData();
        }

        #endregion

        #region Data Interfaces

        [Serializable]
        public class SavedItem
        {
            public int ID;

            public SavedItem(int id)
            {
                ID = id;
            }
        }

		[Serializable]
		public class SavedData
		{
		}

        #endregion
    }
}
