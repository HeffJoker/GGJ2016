using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.IO;
using UnityEditor;
using SF_Tools.Editor;
using SF_Tools.Managers;
using SF_Tools.GUI;
using UnityEngine.EventSystems;

namespace AssemblyCSharp
{
	class Utils
	{
        [MenuItem("Tools/SliceSheet")]
        public static void SliceSheet()
        {
            Texture2D sheetObj = Selection.activeObject as Texture2D;

            if (sheetObj == null)
            {
                Debug.LogError("Not a texture!!");
                return;
            }

            Sprite[] sheet = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(sheetObj)).OfType<Sprite>().ToArray();

            foreach (Sprite sprite in sheet)
            {
                Texture2D tex = sprite.texture;
                Rect r = sprite.textureRect;
                Texture2D subtex = tex.CropTexture((int)r.x, (int)r.y, (int)r.width, (int)r.height);
                byte[] data = subtex.EncodeToPNG();
                File.WriteAllBytes(Application.persistentDataPath + "/" + sprite.name + ".png", data);
            }

            Debug.Log(Application.persistentDataPath);
        }

        [MenuItem("GameObject/SFTools/Create/GameStateManager")]
        public static void CreateGameStateManager()
        {
            List<Type> compTypes = new List<Type>() { typeof(GameStateManager) };
            compTypes.AddRange(EditorUtilities.GetTypeList<State>());
                      
            GameObject stateManager = new GameObject("GameStateManager", compTypes.ToArray());

            GameObject managerObj = CreateManagerFolder();
            stateManager.transform.parent = managerObj.transform;
        }

        [MenuItem("GameObject/SFTools/Create/GUIStateManager")]
        public static void CreateGUIStateManager()
        {
            List<Type> compTypes = new List<Type>() { typeof(GUIStateManager) };

            GameObject guiManager = new GameObject("GUIStateManager", compTypes.ToArray());
            GameObject guiObj = new GameObject("GUI");
            guiObj.transform.parent = guiManager.transform;

            GameObject eventSystem = new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
            eventSystem.transform.parent = guiManager.transform;

            GameObject managerObj = CreateManagerFolder();
            guiManager.transform.parent = managerObj.transform;
        }
 
        public static GameObject CreateManagerFolder()
        {
            GameObject folderObj = GameObject.Find("Managers");

            if (folderObj == null)
                folderObj = new GameObject("Managers");

            return folderObj;
        }

	}
}
