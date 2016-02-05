using UnityEditor;
using UnityEngine;
using System;
using System.IO;
using AssemblyCSharp;
using SF_Tools.Util;

namespace SF_Tools.Editor
{
	public class TextureMerger : EditorWindow
	{
		[UnityEditor.MenuItem("Window/TextureMerger")]
		public static void ShowWindow()
		{
			EditorWindow.GetWindow(typeof(TextureMerger));
		}

		private bool mergeTextures = true;

		private Texture2D topImage;
		private Texture2D botImage;

		private Sprite topSprite;
		private Sprite botSprite;
		private string outputFolder;
		private string texName;

		void OnGUI()
		{
			EditorGUILayout.BeginVertical();

			mergeTextures = EditorGUILayout.BeginToggleGroup("Textures", mergeTextures);
			TextureConfig();
			EditorGUILayout.EndToggleGroup();

			mergeTextures = !EditorGUILayout.BeginToggleGroup("Sprites", !mergeTextures);
			SpriteConfig();
			EditorGUILayout.EndToggleGroup();

			EditorGUILayout.LabelField("Output Path:", EditorStyles.boldLabel);
			texName = EditorGUILayout.TextField("Output Name:", texName);
			EditorGUILayout.BeginHorizontal();

			outputFolder = EditorGUILayout.TextField(outputFolder);

			if(GUILayout.Button("..."))
			{
				outputFolder = EditorUtility.SaveFolderPanel("Get Output Folder", "", "");
			}

			EditorGUILayout.EndHorizontal();

			if(GUILayout.Button("Merge"))
			{	
				if(string.IsNullOrEmpty(texName))
					EditorUtility.DisplayDialog("No Name", "Please enter a name for the new texture.", "OK");
				else
				{
					if(mergeTextures && botImage != null && topImage != null)
						MergeTextures(botImage, topImage, outputFolder, texName);
					else if(botSprite != null && topSprite != null)
						MergeSprites(botSprite, topSprite, outputFolder, texName);
				}
			}
	
			EditorGUILayout.EndVertical();
		}

		private void TextureConfig()
		{			
			EditorGUILayout.LabelField("Top Image:", EditorStyles.boldLabel);
			topImage = (Texture2D)EditorGUILayout.ObjectField(topImage, typeof(Texture2D), false);
			
			EditorGUILayout.LabelField("Bottom Image:", EditorStyles.boldLabel);
			botImage = (Texture2D)EditorGUILayout.ObjectField(botImage, typeof(Texture2D), false);
		}

		private void SpriteConfig()
		{			
			EditorGUILayout.LabelField("Top Sprite:", EditorStyles.boldLabel);
			topSprite = (Sprite)EditorGUILayout.ObjectField(topSprite, typeof(Sprite), false);
			
			EditorGUILayout.LabelField("Bottom Sprite:", EditorStyles.boldLabel);
			botSprite = (Sprite)EditorGUILayout.ObjectField(botSprite, typeof(Sprite), false);
		}

		private void MergeSprites(Sprite botSpr, Sprite topSpr, string path, string name)
		{
			Rect topR = topSpr.textureRect;
			Rect botR = botSpr.textureRect;
			Texture2D subTextTop = topSpr.texture.CropTexture((int)topR.x, (int)topR.y, (int)topR.width, (int)topR.height);
			Texture2D subTextBot = botSpr.texture.CropTexture((int)botR.x, (int)botR.y, (int)botR.width, (int)botR.height);

			MergeTextures(subTextBot, subTextTop, path, name);
		}

		private void MergeTextures(Texture2D botTex, Texture2D topTex, string path, string name)
		{
			if (botTex.width != topTex.width || botTex.height != topTex.height)
				throw new System.InvalidOperationException("AlphaBlend only works with two equal sized images");
			Color[] bData = botTex.GetPixels();
			Color[] tData = topTex.GetPixels();
			int count = bData.Length;
			var rData = new Color[count];
			for(int i = 0; i < count; i++)
			{
				Color B = bData[i];
				Color T = tData[i];
				float srcF = T.a;
				float destF = 1f - T.a;
				float alpha = srcF + destF * B.a;
				Color R = (T * srcF + B * B.a * destF)/alpha;
				R.a = alpha;
				rData[i] = R;
			}
			Texture2D res = new Texture2D(topTex.width, botTex.height);
			res.SetPixels(rData);
			res.Apply();

			byte[] pngData = res.EncodeToPNG();
			File.WriteAllBytes(string.Format("{0}/{1}_merged.png", path, name), pngData);
		}
	}
}

