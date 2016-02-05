using SF_Tools.Editor;
using SF_Tools.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace SFTools.Editor
{
    [CustomEditor(typeof(GUIStateManager))]
    public class GUIStateManager_Editor : UnityEditor.Editor
    {
        #region Private Members

        private readonly string PREFAB_PATH = "Assets/Resources/SF_Tools/GUI/";
        private readonly string PREFAB_NAME = "guiCanvas";

        private GUIStateManager manager = null;
        private List<EditorData<GUIState>> guiStates = new List<EditorData<GUIState>>();
        private Transform guiFolder = null;

        private string newStateName = string.Empty;

        #endregion

        #region Public Interface

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            UnityEngine.GUI.changed = false;

            DrawGUIStateConfig();

            if(UnityEngine.GUI.changed)
            {
                EditorUtility.SetDirty(manager);
            }
        }

        #endregion

        #region Private Routines

        private void Awake()
        {
            manager = (GUIStateManager)target;

            guiFolder = manager.transform.FindChild("GUI");

            if(guiFolder == null)
            {
                GameObject guiFolderObj = new GameObject("GUI");
                guiFolder = guiFolderObj.transform;
                guiFolder.parent = manager.transform;
            }

            for(int i = 0; i < guiFolder.childCount; ++i)
            {
                Transform currChild = guiFolder.GetChild(i);
                GUIState state = currChild.GetComponent<GUIState>();

                if(state != null)
                {
                    guiStates.Add(new EditorData<GUIState>(state, currChild.name));
                }
            }
        }

        private void DrawGUIStateConfig()
        {
            EditorGUILayout.BeginHorizontal();
            newStateName = EditorGUILayout.TextField("Name:", newStateName);

            if(GUILayout.Button("Add") && !string.IsNullOrEmpty(newStateName))
            {
                GameObject newObj = CreateNewGUIState(newStateName);
                newObj.transform.parent = guiFolder;

                GUIState stateObj = newObj.GetComponent<GUIState>();
                guiStates.Add(new EditorData<GUIState>(stateObj, newObj.name));
                newStateName = string.Empty;
                Repaint();
            }

            EditorGUILayout.EndHorizontal();

            EditorUtilities.EditableList(guiStates, DrawGUIState, true, false, false, false);

        }

        private GameObject CreateNewGUIState(string name)
        {
            GUIState guiPrefab = Resources.Load(PREFAB_NAME) as GUIState;

            if(guiPrefab == null)
            {
                guiPrefab = CreateGUIPrefab();
            }
            
            GameObject retObj = PrefabUtility.InstantiatePrefab(guiPrefab) as GameObject;
            retObj.name = name;
            return retObj;
        }

        private GUIState CreateGUIPrefab()
        {
            List<Type> compTypes = new List<Type>() { typeof(Canvas), typeof(GUIState), typeof(Animator), typeof(AnimationClipOverride) };
            GameObject newObj = new GameObject(PREFAB_NAME, compTypes.ToArray());
            GameObject retObj = PrefabUtility.CreatePrefab(PREFAB_PATH + PREFAB_NAME + ".prefab", newObj);

            GameObject.DestroyImmediate(newObj);
            GUIState retState = retObj.GetComponent<GUIState>();

            return retState;
        }

        private void DrawGUIState(GUIState stateData)
        {

        }

        #endregion
    }
}
