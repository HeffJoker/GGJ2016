using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using SF_Tools.Managers;
using Foundation.Messenger;
using SF_Tools.Messages;
using UnityEngine;

namespace SF_Tools.Editor
{
    [CustomEditor(typeof(DataManager))]
	public class DataManager_Editor : UnityEditor.Editor
    {
        #region Private Members

        private DataManager manager;
        private DataManager.SavedData playerData;

        #endregion

        #region Public Interface

        public void Awake()
        {
            manager = (DataManager)target;

            Messenger.Unsubscribe(this);
            Messenger.Subscribe(this);

            manager.LoadData(true, false);
        }

        public override void OnInspectorGUI()
        {
            UnityEngine.GUI.changed = false;

            manager.SaveFile = EditorGUILayout.TextField("Save File", manager.SaveFile);

            ++EditorGUI.indentLevel;
            
            --EditorGUI.indentLevel;

            if(GUILayout.Button("Reload Data"))
            {
                manager.LoadData(true, false);
                this.Repaint();
            }

            if(GUILayout.Button("Reset Data"))
            {
                manager.Clear();
                manager.SaveData(false);
                //manager.LoadData(true, false);
                PlayerPrefs.DeleteAll();
                this.Repaint();
            }

            if (UnityEngine.GUI.changed)
            {
                EditorUtility.SetDirty(manager);
            }
        }

        [Subscribe]
        public void HandleDataLoad(Message_DataLoaded message)
        {
            playerData = message.LoadedData;
            if (playerData == null)
            {
                manager.Clear();
                manager.SaveData(false);
            }
        }

        #endregion

    }
}
