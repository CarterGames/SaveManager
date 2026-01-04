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
        private string SaveObjectGenClassName
        {
            get => EditorUserSettings.GetString(string.Format(SaveManagerConstants.PrefFormat,
                "save_object_gen_last_used_class_name"));
            set => EditorUserSettings.SetString(string.Format(SaveManagerConstants.PrefFormat,
                "save_object_gen_last_used_class_name"), value);
        }
        
        private int SaveObjectGenType
        {
            get => EditorUserSettings.GetInt(string.Format(SaveManagerConstants.PrefFormat,
                "save_object_gen_last_used_type"));
            set => EditorUserSettings.SetInt(string.Format(SaveManagerConstants.PrefFormat,
                "save_object_gen_last_used_type"), value);
        }
        
        private string SaveObjectLastFileName
        {
            get => EditorUserSettings.GetString(string.Format(SaveManagerConstants.PrefFormat,
                "save_object_gen_last_used_file_name"));
            set => EditorUserSettings.SetString(string.Format(SaveManagerConstants.PrefFormat,
                "save_object_gen_last_used_file_name"), value);
        }
        
        
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
            
            SaveObjectGenClassName = EditorGUILayout.TextField(SaveObjectGenClassName);
            
            if (ScriptableRef.GetAssetDef<DataAssetSettings>().AssetRef.UseSaveSlots)
            {
                SaveObjectGenType = EditorGUILayout.IntPopup(
                    new GUIContent("Save Object Type:"),
                    SaveObjectGenType,
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

            EditorGUI.BeginDisabledGroup(SaveObjectGenClassName.Length <= 0);
            string path = string.Empty;
            
            if (GUILayout.Button("Create Save Object"))
            {
                switch (SaveObjectGenType)
                {
                    case (int)SaveObjectGenerationType.Global:
                        path = EditorUtility.SaveFilePanelInProject("Save New Save Object Class", SaveObjectGenClassName + "SaveObject", "cs", "");
                        break;
                    case (int)SaveObjectGenerationType.Slot:
                        path = EditorUtility.SaveFilePanelInProject("Save New Slot Save Object Class", SaveObjectGenClassName + "SlotSaveObject", "cs", "");
                        break;
                }
                
                SaveObjectLastFileName =
                    path.Split('/')[path.Split('/').Length - 1].Replace(".cs", string.Empty);
                
                var script = AssetDatabase.FindAssets($"t:Script {nameof(SaveObjectGenerator)}")[0];
                var pathToTextFile = AssetDatabase.GUIDToAssetPath(script);

                switch (SaveObjectGenType)
                {
                    case (int)SaveObjectGenerationType.Global:
                        pathToTextFile = pathToTextFile.Replace("SaveObjectGenerator.cs", "Templates/SaveObjectTemplate.txt");
                        break;
                    case (int)SaveObjectGenerationType.Slot:
                        pathToTextFile = pathToTextFile.Replace("SaveObjectGenerator.cs", "Templates/SlotSaveObjectTemplate.txt");
                        break;
                }
                
                TextAsset template = AssetDatabase.LoadAssetAtPath<TextAsset>(pathToTextFile);
                template = new TextAsset(template.text);
                var replace = template.text.Replace("%SaveObjectName%", SaveObjectLastFileName);

                File.WriteAllText(path, replace);
                EditorUtility.SetDirty(AssetDatabase.LoadAssetAtPath<TextAsset>(pathToTextFile));
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                
                EditorUtility.RequestScriptReload();
            }
            
            EditorGUI.EndDisabledGroup();
        }
    }
}