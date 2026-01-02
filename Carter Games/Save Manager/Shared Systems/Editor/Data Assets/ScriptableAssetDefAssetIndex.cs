/*
 * Copyright (c) 2025 Carter Games
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
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using System;
using UnityEditor;

namespace CarterGames.Shared.SaveManager.Editor
{
	/// <summary>
	/// Handles the creation and referencing of the asset index file.
	/// </summary>
	public sealed class ScriptableAssetDefAssetIndex : IScriptableAssetDef<SmDataAssetIndex>
	{
		// IScriptableAssetDef Implementation
		/* ────────────────────────────────────────────────────────────────────────────────────────────────────────── */
		
		private static SmDataAssetIndex cache;
		private static SerializedObject objCache;

		public Type AssetType => typeof(SmDataAssetIndex);
		public string DataAssetFileName => $"[{AssetVersionData.AssetName}] Asset Index.asset";
		public string DataAssetFilter => $"t:{typeof(SmDataAssetIndex).FullName} name={DataAssetFileName}";
		public string DataAssetPath => $"{ScriptableRef.FullPathResources}{DataAssetFileName}";

		public SmDataAssetIndex AssetRef => ScriptableRef.GetOrCreateAsset(this, ref cache);
		public SerializedObject ObjectRef => ScriptableRef.GetOrCreateAssetObject(this, ref objCache);
		
		public void TryCreate()
		{
			ScriptableRef.GetOrCreateAsset(this, ref cache);
		}

		public void OnCreated() { }

		// ILegacyAssetPort Implementation
		/* ────────────────────────────────────────────────────────────────────────────────────────────────────────── */
		
		public bool CanPort => AssetDatabaseHelper.TypeExistsElsewhere<SmDataAssetIndex>(DataAssetPath);
		

		public void PortAsset()
		{
			TryCreate();

			var assets = AssetDatabaseHelper.GetAssetPathNotAtPath<SmDataAssetIndex>(DataAssetPath);

			if (assets != null)
			{
				foreach (var entry in assets)
				{
					AssetDatabase.DeleteAsset(entry);
				}
			}
			
			AssetDatabase.SaveAssets();
			AssetIndexHandler.UpdateIndex();
		}
	}
}