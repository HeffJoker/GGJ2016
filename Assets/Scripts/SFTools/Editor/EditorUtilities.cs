using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using System.Reflection;

namespace SF_Tools.Editor
{
	public class EditorUtilities
	{
        public delegate void InspectorDrawMethod<T>(T data);
        
        public static void EditableList<T>(List<EditorData<T>> dataList, InspectorDrawMethod<T> canvasDrawer, 
            bool allowDelete = true, bool removeFromListOnly = true, bool allowItemMove = true, bool useExpandAndCollapse = true)
        {
            EditorGUILayout.BeginVertical();
            
            if (useExpandAndCollapse)
            {
                EditorGUILayout.BeginHorizontal();

                if (GUILayout.Button("Expand All"))
                {
                    for (int i = 0; i < dataList.Count; ++i)
                    {
                        EditorData<T> data = dataList[i];
                        data.IsEditing = true;
                        dataList[i] = data;
                    }
                }

                if (GUILayout.Button("Collapse All"))
                {
                    for (int i = 0; i < dataList.Count; ++i)
                    {
                        EditorData<T> data = dataList[i];
                        data.IsEditing = false;
                        dataList[i] = data;
                    }
                }

                EditorGUILayout.EndHorizontal();
            }

            ++EditorGUI.indentLevel;

            for (int i = 0; i < dataList.Count; ++i)
            {
                EditorData<T> editorData = dataList[i];

                if (editorData.Data == null)
                {
                    dataList.Remove(editorData);
                    --i;
                    continue;
                }

                EditorGUILayout.BeginHorizontal();

                editorData.IsEditing = EditorGUILayout.Foldout(editorData.IsEditing, editorData.Name);

                if (allowItemMove)
                {
                    if (i == 0)
                        UnityEngine.GUI.enabled = false;

                    if (GUILayout.Button("+", GUILayout.MaxWidth(30)))
                    {
                        EditorData<T> otherData = dataList[i];
                        dataList[i] = dataList[i - 1];
                        dataList[i - 1] = otherData;
                        editorData = dataList[i];
                    }

                    UnityEngine.GUI.enabled = true;

                    if (i == dataList.Count - 1)
                        UnityEngine.GUI.enabled = false;

                    if (GUILayout.Button("-", GUILayout.MaxWidth(30)))
                    {
                        EditorData<T> otherGoal = dataList[i];
                        dataList[i] = dataList[i + 1];
                        dataList[i + 1] = otherGoal;
                        editorData = dataList[i];
                    }

                    UnityEngine.GUI.enabled = true;
                }

                UnityEngine.GUI.enabled = allowDelete || removeFromListOnly;

                if (GUILayout.Button("X", GUILayout.MaxWidth(30)))
                {
                    if (!removeFromListOnly)
                    {
                        if (editorData.Data is ScriptableObject)
                        {
                            string assetPath = AssetDatabase.GetAssetPath(((ScriptableObject)(object)editorData.Data).GetInstanceID());
                            AssetDatabase.DeleteAsset(assetPath);
                        }
                        else if (editorData.Data is UnityEngine.Object)
                        {
                            string assetPath = AssetDatabase.GetAssetPath(((MonoBehaviour)(object)editorData.Data).GetInstanceID());
                            AssetDatabase.DeleteAsset(assetPath);
                        }
                    }

                    dataList.Remove(editorData);
                    --i;
                    continue;
                }

                UnityEngine.GUI.enabled = true;

                EditorGUILayout.EndHorizontal();

                ++EditorGUI.indentLevel;

                if (editorData.IsEditing)
                {
                    canvasDrawer(editorData.Data);
                }

                --EditorGUI.indentLevel;

                dataList[i] = editorData;
            }

            --EditorGUI.indentLevel;

            EditorGUILayout.EndVertical();
        }

        public static List<Type> GetTypeList<T>()
        {
            List<Type> retList = new List<Type>();

            foreach(Type type in Assembly.GetAssembly(typeof(T)).GetTypes().Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(T))))
                retList.Add(type);

            return retList;
        }

        public static bool EditListContains<T>(List<EditorData<T>> list, T data)
        {
            foreach (EditorData<T> item in list)
            {
                if (item.Data.Equals(data))
                    return true;
            }

            return false;
        }
	}
}
