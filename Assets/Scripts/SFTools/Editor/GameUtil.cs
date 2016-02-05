using UnityEngine;
using UnityEditor;
using System.IO;
using System;

namespace SF_Tools.Util
{
    public static class GameUtil
	{
		public static ScriptableObject CreateAsset<T>(string name, bool autoSelect = true, string path = "") where T : ScriptableObject
		{
			T asset = ScriptableObject.CreateInstance<T>();

            if(path == string.Empty)
			    path = AssetDatabase.GetAssetPath(Selection.activeObject);
			if(path == string.Empty)
				path = "Assets";
			else if(Path.GetExtension(path) != string.Empty)
				path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), string.Empty);

			string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(string.Format("{0}/{1}.asset", path, name));

			AssetDatabase.CreateAsset(asset, assetPathAndName);
            AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();

            if (autoSelect)
            {
                EditorUtility.FocusProjectWindow();            
                Selection.activeObject = asset;
            }
                       
            return asset;
		}

        public static ScriptableObject CreateAsset(Type type, string name, bool autoSelect = true, string folder = "")
        {
            ScriptableObject asset = ScriptableObject.CreateInstance(type);

            string path = AssetDatabase.GetAssetPath(Selection.activeObject);

            if (folder != string.Empty)
                path = folder;
            else
            {
                if (path == string.Empty)
                    path = "Assets";
                else if (Path.GetExtension(path) != string.Empty)
                    path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), string.Empty);
            }

            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(string.Format("{0}/{1}.asset", path, name));

            AssetDatabase.CreateAsset(asset, assetPathAndName);
            //AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            if (autoSelect)
            {
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = asset;
            }

            return asset;
        }  
 
        [MenuItem("Assets/NestAnimation")]
        public static void NestAnimation()
        {
            if (Selection.objects.Length < 2)
            {
                Debug.LogError("You must select more than one object to nest!");
                return;
            }

            UnityEngine.Object[] objs = Selection.objects;
            UnityEngine.Object animator = null;

            foreach (UnityEngine.Object obj in objs)
            {
                if (obj.name.Contains("Animator"))
                {
                    animator = obj;
                    break;
                }
            }

            foreach (UnityEngine.Object obj in objs)
            {
                if (obj != animator && !obj.name.Contains("Animator"))
                {
                    UnityEngine.Object newObj = UnityEngine.Object.Instantiate(obj);
                    newObj.name = newObj.name.Replace("(Clone)", string.Empty);
                    AssetDatabase.AddObjectToAsset(newObj, animator);
                    AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(obj));
                    AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(animator));
                }
            }

            
        }
    }
}