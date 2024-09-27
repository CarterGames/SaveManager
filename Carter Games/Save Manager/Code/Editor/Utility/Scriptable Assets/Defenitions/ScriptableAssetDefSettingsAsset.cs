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

using System;
using System.Linq;
using UnityEditor;

namespace CarterGames.Assets.SaveManager.Editor
{
	/// <summary>
	/// Handles the creation and referencing of the settings asset file.
	/// </summary>
	public sealed class ScriptableAssetDefSettingsAsset : IScriptableAssetDef<AssetGlobalRuntimeSettings>, ILegacyAssetPort
	{
		// IScriptableAssetDef Implementation
		/* ────────────────────────────────────────────────────────────────────────────────────────────────────────── */
		
		private static AssetGlobalRuntimeSettings cache;
		private static SerializedObject objCache;

		public Type AssetType => typeof(AssetGlobalRuntimeSettings);
		public string DataAssetFileName => "[Save Manager] Runtime Settings Asset.asset";
		public string DataAssetFilter => $"t:{typeof(AssetGlobalRuntimeSettings).FullName}";
		public string DataAssetPath => $"{ScriptableRef.FullPathData}{DataAssetFileName}";

		public AssetGlobalRuntimeSettings AssetRef => ScriptableRef.GetOrCreateAsset(this, ref cache);
		public SerializedObject ObjectRef => ScriptableRef.GetOrCreateAssetObject(this, ref objCache);
		
		public void TryCreate()
		{
			ScriptableRef.GetOrCreateAsset(this, ref cache);
		}
		
		// ILegacyAssetPort Implementation
		/* ────────────────────────────────────────────────────────────────────────────────────────────────────────── */
		
		public bool CanPort => AssetDatabaseHelper.TypeExistsElsewhere<AssetGlobalRuntimeSettings>(DataAssetPath);
		
		
		public void PortAsset()
		{
			TryCreate();
			
			var assets = AssetDatabaseHelper.GetAssetPathNotAtPath<AssetGlobalRuntimeSettings>(DataAssetPath).ToArray();

			if (assets.Length <= 0) return;

			SerializedPropertyHelper.TransferProperties(
				new SerializedObject(AssetDatabase.LoadAssetAtPath(assets[0], typeof(AssetGlobalRuntimeSettings))), ObjectRef);

			ObjectRef.ApplyModifiedProperties();
			ObjectRef.Update();

			foreach (var entry in assets)
			{
				AssetDatabase.DeleteAsset(entry);
			}
			
			AssetDatabase.SaveAssets();
		}
	}
}