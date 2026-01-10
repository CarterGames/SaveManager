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
using UnityEditor;

namespace CarterGames.Shared.SaveManager.Editor
{
	/// <summary>
	/// Implement to define a scriptable asset in the asset.
	/// </summary>
	/// <typeparam name="T">The type of the asset itself.</typeparam>
	public interface IScriptableAssetDef<out T> where T : SmDataAsset
	{
		Type AssetType { get; }
		string DataAssetFileName { get; }
		string DataAssetFilter { get; }
		string DataAssetPath { get; }
		T AssetRef { get; }
		SerializedObject ObjectRef { get; }
		void TryCreate();
		void OnCreated();
	}
}