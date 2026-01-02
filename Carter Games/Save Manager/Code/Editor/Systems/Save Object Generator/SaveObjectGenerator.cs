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

using CarterGames.Shared.SaveManager.Editor;
using UnityEditor;
using UnityEngine;
using File = System.IO.File;

namespace CarterGames.Assets.SaveManager.Editor
{
    public class SaveObjectGenerator : EditorWindow
    {
        [MenuItem("Tools/Carter Games/Save Manager/Save Object Creator", priority = 30)]
        public static void OpenMenu()
        {
            var window = (SaveObjectGenerator)GetWindow(typeof(SaveObjectGenerator));
            window.titleContent = new GUIContent("Save Object Creator");
            window.minSize = new Vector2(400, 250);
            window.maxSize = new Vector2(400, 250);
            window.ShowPopup();
        }


        private void OnGUI()
        {
            EditorGUILayout.LabelField("Save Object Name", EditorStyles.boldLabel);
            
            PerUserSettings.SaveObjectGenClassName = EditorGUILayout.TextField(PerUserSettings.SaveObjectGenClassName);
            
            if (ScriptableRef.GetAssetDef<DataAssetSettings>().AssetRef.UseSaveSlots)
            {
                PerUserSettings.SaveObjectGenType = EditorGUILayout.IntPopup(
                    new GUIContent("Save Object Type:"),
                    PerUserSettings.SaveObjectGenType,
                    new GUIContent[2]
                    {
                        new GUIContent(SaveObjectGenerationType.Global.ToString()),
                        new GUIContent(SaveObjectGenerationType.Slot.ToString())
                    }, new int[2]
                    {
                        (int)SaveObjectGenerationType.Global,
                        (int)SaveObjectGenerationType.Slot
                    });
            }

            EditorGUI.BeginDisabledGroup(PerUserSettings.SaveObjectGenClassName.Length <= 0);
            string path = string.Empty;
            
            if (GUILayout.Button("Create Save Object"))
            {
                path = EditorUtility.SaveFilePanelInProject("Save New Save Object Class", PerUserSettings.SaveObjectGenClassName + "SaveObject", "cs", "");
                
                PerUserSettings.LastSaveObjectFileName =
                    path.Split('/')[path.Split('/').Length - 1].Replace(".cs", string.Empty);
                
                var script = AssetDatabase.FindAssets($"t:Script {nameof(SaveObjectGenerator)}")[0];
                var pathToTextFile = AssetDatabase.GUIDToAssetPath(script);
                pathToTextFile = pathToTextFile.Replace("SaveObjectGenerator.cs", "SaveObjectTemplate.txt");
                
                
                TextAsset template = AssetDatabase.LoadAssetAtPath<TextAsset>(pathToTextFile);
                template = new TextAsset(template.text);
                var replace = template.text.Replace("%SaveObjectName%", PerUserSettings.LastSaveObjectFileName);

                File.WriteAllText(path, replace);
                EditorUtility.SetDirty(AssetDatabase.LoadAssetAtPath<TextAsset>(pathToTextFile));
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                PerUserSettings.JustCreatedSaveObject = true;
                
                EditorUtility.RequestScriptReload();
            }
            
            EditorGUI.EndDisabledGroup();
        }
    }
}