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

using UnityEditor;
using UnityEngine;

namespace CarterGames.Assets.SaveManager.Editor
{
	/// <summary>
	/// An editor window to edit the save defaults of a save object.
	/// </summary>
	public sealed class SaveDefaultsWindow : EditorWindow
	{
		/* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
		|   Fields
		───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
		
		private static SaveObject selected;
		private static SerializedObject selectedObject;
		private static Vector2 scrollRectPos;
		
		/* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
		|   Open Window Method
		───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
		
		public static void ShowDefaultsWindow(SaveObject saveObject, SerializedObject serializedObjectForSaveObject)
		{
			selected = saveObject;
			selectedObject = serializedObjectForSaveObject;

			GetWindow<SaveDefaultsWindow>(true, $"Default Values - {selected.name}").Show();
		}
		
		/* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
		|   Draw Methods
		───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

		private void OnGUI()
		{
			if (selected == null)
			{
				Close();
				return;
			}

			scrollRectPos = EditorGUILayout.BeginScrollView(scrollRectPos);
			
			DrawTab();
			
			EditorGUILayout.EndScrollView();
		}


		private void DrawTab()
		{
			if (Application.isPlaying)
			{
				EditorGUILayout.HelpBox(
					"You cannot edit the save while in play mode, please exit play mode to edit the save data.",
					MessageType.Info);
			}
			
			EditorGUILayout.HelpBox(
				$"Here you can edit all the default save values on the Save object. These are then set on a fresh save where defaults are used in the resetting.",
				MessageType.Info);

			EditorGUILayout.BeginVertical("Box");
			
			if (selected != null)
			{
				ShowPropertiesForObject();
			}
			
			EditorGUILayout.EndVertical();
		}


		private static void ShowPropertiesForObject()
		{
			selectedObject.Update();
			
			var propIterator = selectedObject.GetIterator();

			if (!propIterator.NextVisible(true)) return;
			
			while (propIterator.NextVisible(true))
			{
				var propElement = selectedObject.Fp(propIterator.name);

				if (propElement == null) continue;
				
				GUILayout.Space(3.5f);
				
				EditorGUILayout.BeginVertical("HelpBox");
				
				EditorGUI.BeginChangeCheck();
				
				EditorGUILayout.PropertyField(selectedObject.Fp(propIterator.name).Fpr("defaultValue"),
					new GUIContent(propIterator.displayName));

				if (EditorGUI.EndChangeCheck())
				{
					Undo.RecordObject(selectedObject.targetObject, "Save Value Default modified");
					
					selectedObject.ApplyModifiedProperties();
					selectedObject.Update();
				}
				
				EditorGUILayout.EndVertical();
			}
			
			GUILayout.Space(3.5f);
		}
	}
}