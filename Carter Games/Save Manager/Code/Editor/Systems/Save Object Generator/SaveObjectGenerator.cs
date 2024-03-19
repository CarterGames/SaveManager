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
using UnityEditor.Callbacks;
using UnityEngine;
using File = System.IO.File;

namespace CarterGames.Assets.SaveManager.Editor
{
    public class SaveObjectGenerator : EditorWindow
    {
        [MenuItem("Tools/Carter Games/Save Manager/Save Object Creator", priority = 16)]
        public static void OpenMenu()
        {
            SaveObjectGenerator window = (SaveObjectGenerator)GetWindow(typeof(SaveObjectGenerator), true, "Save Object Creator");
            window.Show();
        }


        private void OnGUI()
        {
            EditorGUILayout.LabelField("Save Object Name", EditorStyles.boldLabel);
            
            PerUserSettings.LastSaveObjectName = EditorGUILayout.TextField(PerUserSettings.LastSaveObjectName);

            EditorGUI.BeginDisabledGroup(PerUserSettings.LastSaveObjectName.Length <= 0);
            string path = string.Empty;
            
            if (GUILayout.Button("Create Save Object"))
            {
                path = EditorUtility.SaveFilePanelInProject("Save New Save Object Class", PerUserSettings.LastSaveObjectName + "SaveObject", "cs", "");
                
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
        
        
        
        private static void CreateSaveObjectInstance()
        {
            if (EditorUtility.DisplayDialog("Create Instance",
                    "Do you want to create a new instance of the save object you just made?", "Yes", "Cancel"))
            {
                var parse = "Save." + PerUserSettings.LastSaveObjectFileName;


                var types = AppDomain.CurrentDomain
                    .GetAssemblies()
                    .SelectMany(x => x.GetTypes())
                    .FirstOrDefault(x => x.IsClass && x.FullName == parse && x.IsAssignableFrom(x));

                
                var instance = CreateInstance(types);
                
                var script =
                    AssetDatabase.FindAssets($"t:Script {PerUserSettings.LastSaveObjectFileName}")[0];
                var pathToTextFile = AssetDatabase.GUIDToAssetPath(script);

                pathToTextFile = pathToTextFile.Replace(".cs", ".asset");

                AssetDatabase.CreateAsset(instance, pathToTextFile);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }


        [DidReloadScripts]
        private static void TryCreateInstanceIfMade()
        {
            if (!PerUserSettings.JustCreatedSaveObject) return;
                
            PerUserSettings.JustCreatedSaveObject = false;
            
            CreateSaveObjectInstance();
        }
    }
}