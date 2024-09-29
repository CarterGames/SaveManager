/*
 * Copyright (c) 2024 Carter Games
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NON-INFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using System.Reflection;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace CarterGames.Assets.SaveManager.Editor
{
	/// <summary>
	/// Handles resetting save objects to defaults on builds, then restoring them when the build is done. 
	/// </summary>
	public sealed class BuildHandlerResetSaveObjects : IPreprocessBuildWithReport, IPostprocessBuildWithReport
	{
		public int callbackOrder { get; }
        
        
		public void OnPreprocessBuild(BuildReport report)
		{
			var assets = AssetDatabase.FindAssets($"t:{typeof(SaveObject)}");

			if (assets.Length <= 0) return;
			
			foreach (var asset in assets)
			{
				var fileInstancePath = AssetDatabase.GUIDToAssetPath(asset);
				var fileInstance = AssetDatabase.LoadAssetAtPath<SaveObject>(fileInstancePath);
				
				var assetObject = new SerializedObject(fileInstance);

				EditorPrefs.SetString(assetObject.targetObject.GetInstanceID().ToString(), EditorJsonUtility.ToJson(assetObject.targetObject));
				
				var propIterator = assetObject.GetIterator();
            
				if (propIterator.NextVisible(true))
				{
					while (propIterator.NextVisible(true))
					{
						var propElement = assetObject.Fp(propIterator.name);
						
						if (propElement == null) continue;
						if (!propElement.type.Contains("SaveValue")) continue;
						if (propElement.propertyType != propIterator.propertyType) continue;

						var field = assetObject.targetObject.GetType()
							.GetField(propElement.propertyPath.Split('.')[0],
								BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
						
						field.GetValue(assetObject.targetObject).GetType()
							.GetMethod("ResetValue", BindingFlags.Public | BindingFlags.Instance).Invoke(field.GetValue(assetObject.targetObject), new object[] { true });
					}
				}
				
				assetObject.ApplyModifiedProperties();
				assetObject.Update();
			}
		}
        
        
		public void OnPostprocessBuild(BuildReport report)
		{
			var assets = AssetDatabase.FindAssets($"t:{typeof(SaveObject)}");

			if (assets.Length <= 0) return;

			foreach (var asset in assets)
			{
				var fileInstancePath = AssetDatabase.GUIDToAssetPath(asset);
				var fileInstance = AssetDatabase.LoadAssetAtPath<SaveObject>(fileInstancePath);

				var assetObject = new SerializedObject(fileInstance);
				
				EditorJsonUtility.FromJsonOverwrite(EditorPrefs.GetString(assetObject.targetObject.GetInstanceID().ToString()), assetObject.targetObject);
				EditorUtility.SetDirty(assetObject.targetObject);
			}
			
			AssetDatabase.SaveAssets();
		}
	}
}