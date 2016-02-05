using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using SF_Tools.Util;
using SF_Tools.Util;

namespace SF_Tools.Editor
{
    [CustomEditor(typeof(ScreenShotHelper))]
    public class ScreenShotHelper_Editor : UnityEditor.Editor
    {
        #region Private Members

        private ScreenShotHelper instance = null;
        private List<EditorData<GameObject>> objects = new List<EditorData<GameObject>>();
        private GameObject currObj = null;

        #endregion

        #region Public Interface

        public override void OnInspectorGUI()
        {
            UnityEngine.GUI.changed = false;

            instance.OutputFolder = EditorGUILayout.TextField("Output Folder:", instance.OutputFolder);
            instance.CameraToUse = EditorGUILayout.ObjectField("Camera:", instance.CameraToUse, typeof(Camera), true) as Camera;

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            currObj = EditorGUILayout.ObjectField("New Obj:", currObj, typeof(GameObject), true) as GameObject;

            if(currObj != null)
            {
                objects.Add(new EditorData<GameObject>(currObj, currObj.name));
                currObj = null;
                Repaint();
            }

            EditorGUILayout.EndHorizontal();

            EditorUtilities.EditableList(objects, DrawObject, true, true, false, false);
                       
            instance.ObjsToCapture = new GameObject[objects.Count];
            for (int i = 0; i < objects.Count; ++i)
                instance.ObjsToCapture[i] = objects[i].Data;

            if(UnityEngine.GUI.changed)
            {
                EditorUtility.SetDirty(instance);
            }

            if (GUILayout.Button("Do Screenshots"))
            {
                instance.DoScreenShots();
            }
        }

        #endregion

        #region Private Routines

        private void DrawObject(GameObject obj)
        {
        }

        private void Awake()
        {
            instance = (ScreenShotHelper)target;

            foreach(GameObject obj in instance.ObjsToCapture)
            {
                objects.Add(new EditorData<GameObject>(obj, obj.name));
            }
        }

        #endregion
    }
}
