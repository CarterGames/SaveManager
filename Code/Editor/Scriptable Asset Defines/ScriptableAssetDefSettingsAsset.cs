/*
 * Save Manager (3.x)
 * Copyright (c) 2025-2026 Carter Games
 *
 * This program is free software: you can redistribute it and/or modify it under the terms of the
 * GNU General Public License as published by the Free Software Foundation,
 * either version 3 of the License, or (at your option) any later version. 
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or
 * FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. 
 *
 * You should have received a copy of the GNU General Public License along with this program.
 * If not, see <https://www.gnu.org/licenses/>. 
 */

using System;
using CarterGames.Shared.SaveManager.Editor;
using UnityEditor;

namespace CarterGames.Assets.SaveManager.Editor
{
	/// <summary>
	/// Handles the creation and referencing of the settings asset file.
	/// </summary>
	public sealed class ScriptableAssetDefSettingsAsset : IScriptableAssetDef<DataAssetSettings>
	{
		// IScriptableAssetDef Implementation
		/* ────────────────────────────────────────────────────────────────────────────────────────────────────────── */
		
		private static DataAssetSettings cache;
		private static SerializedObject objCache;

		public Type AssetType => typeof(DataAssetSettings);
		public string DataAssetFileName => "[Save Manager] Runtime Settings Asset.asset";
		public string DataAssetFilter => $"t:{typeof(DataAssetSettings).FullName}";
		public string DataAssetPath => $"{ScriptableRef.FullPathData}{DataAssetFileName}";

		public DataAssetSettings AssetRef => ScriptableRef.GetOrCreateAsset(this, ref cache);
		public SerializedObject ObjectRef => ScriptableRef.GetOrCreateAssetObject(this, ref objCache);
		
		public void OnCreated() { }
		
		public void TryCreate()
		{
			ScriptableRef.GetOrCreateAsset(this, ref cache);
		}
	}
}