using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using SF_Tools.Managers;
using SF_Tools.Util;

namespace SF_Tools.Editor
{
    [CustomEditor(typeof(LevelManager))]
	public class LevelManager_Editor : UnityEditor.Editor
    {
        #region Private Members

        private LevelManager manager;

        private string newLevelName = string.Empty;
        private GameObject currButton;
        private LevelInfo buttonLevel;

        private bool isLevelEnabled = false;

        private List<EditorData<LevelInfo>> levels = new List<EditorData<LevelInfo>>();
        private List<EditorData<LevelButtonLink>> levelButtons = new List<EditorData<LevelButtonLink>>();

        #endregion

        #region Public Interface

        public override void OnInspectorGUI()
        {
            UnityEngine.GUI.changed = false;

            DrawConfigOptions();

            manager.Levels.Clear();
            manager.LevelButtons.Clear();

            levels.ForEach(x => manager.Levels.Add(x.Data));
            levelButtons.ForEach(x => manager.LevelButtons.Add(x.Data));

            if (UnityEngine.GUI.changed)
            {
                EditorUtility.SetDirty(manager);
                manager.Levels.ForEach(x => EditorUtility.SetDirty(x));
            }

            //AssetDatabase.SaveAssets();
        }
        
        #endregion

        #region Private Routines

        private void Awake()
        {
            manager = (LevelManager)target;

            manager.Levels.ForEach(x =>
            {
                string name = string.Format("{0} - {1}", x.LevelName, (x.Enabled ? "Enabled" : "Disabled"));
                levels.Add(new EditorData<LevelInfo>(x, name));
            });

            manager.LevelButtons.ForEach(x =>
            {
                string name = string.Format("{0} button", x.Level.LevelName);
                levelButtons.Add(new EditorData<LevelButtonLink>(x, name));
            });
        }

        private void DrawConfigOptions()
        {
            manager.GameObjectIdentifier = EditorGUILayout.TextField(new GUIContent("Game Object ID", "The name of the game object to delete when the scene is reloaded.  This holds all of the levels game objects and managers."),
                                                                     manager.GameObjectIdentifier);

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.BeginVertical();

            newLevelName = EditorGUILayout.TextField("Name", newLevelName);
            isLevelEnabled = EditorGUILayout.Toggle("Is Enabled?", isLevelEnabled);

            EditorGUILayout.EndVertical();

            if (GUILayout.Button("Add", GUILayout.ExpandHeight(true), GUILayout.MaxHeight(30)) 
                && !string.IsNullOrEmpty(newLevelName) && !DoesLevelExist(newLevelName))
            {
                LevelInfo newLevel = (LevelInfo)GameUtil.CreateAsset<LevelInfo>(newLevelName, false, "Assets/Resources/Prefabs/Assets/GameLevels/");
                newLevel.LevelName = newLevelName;
                newLevel.Enabled = isLevelEnabled;

                string name = string.Format("{0} - {1}", newLevel.LevelName, (newLevel.Enabled ? "Enabled" : "Disabled"));
                levels.Add(new EditorData<LevelInfo>(newLevel, name));

                newLevelName = string.Empty;
                isLevelEnabled = false;
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.LabelField("Levels", EditorStyles.boldLabel);
            ++EditorGUI.indentLevel;

            EditorUtilities.EditableList(levels, DrawLevel, true, false, false, false);

            --EditorGUI.indentLevel;

            if(GUILayout.Button("Reload Levels"))
            {
                levels.Clear();

                LevelInfo[] existingLevels = Resources.LoadAll<LevelInfo>("Prefabs/Assets/GameLevels");

                for(int i = 0; i < existingLevels.Length; ++i)
                {
                    string name = string.Format("{0} - {1}", existingLevels[i].LevelName, (existingLevels[i].Enabled ? "Enabled" : "Disabled"));
                    levels.Add(new EditorData<LevelInfo>(existingLevels[i], name));
                }
            }
            
            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical();

            currButton = EditorGUILayout.ObjectField("Button:", currButton, typeof(GameObject), true) as GameObject;
            buttonLevel = EditorGUILayout.ObjectField("Level:", buttonLevel, typeof(LevelInfo), false) as LevelInfo;

            EditorGUILayout.EndVertical();

            if(GUILayout.Button("Add") && currButton != null && buttonLevel != null)
            {
                LevelButtonLink newLevelButton = new LevelButtonLink();
                newLevelButton.ButtonObj = currButton;
                newLevelButton.Level = buttonLevel;
                levelButtons.Add(new EditorData<LevelButtonLink>(newLevelButton, string.Format("{0} button", buttonLevel.LevelName)));

                currButton = null;
                buttonLevel = null;
                Repaint();
            }

            EditorGUILayout.EndHorizontal();

            EditorUtilities.EditableList(levelButtons, DrawLevelButton);
        }

        private void DrawLevelButton(LevelButtonLink levelButtonData)
        {
        }

        private void DrawLevel(LevelInfo levelData)
        {
            EditorGUILayout.BeginVertical();

            levelData.LevelName = EditorGUILayout.TextField("Name", levelData.LevelName);
            levelData.Enabled = EditorGUILayout.Toggle("Is Enabled?", levelData.Enabled);

            EditorGUILayout.EndVertical();
        }

        private bool DoesLevelExist(string levelName)
        {
            EditorData<LevelInfo> level = levels.Find(x => x.Data.LevelName == levelName);

            return (level != null);
        }

        #endregion
    }
}
