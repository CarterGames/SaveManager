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
            
            EditorGUI.BeginChangeCheck();
            UtilEditor.EditorSettingsObject.FindProperty("lastSaveObjectName").stringValue = EditorGUILayout.TextField(UtilEditor.EditorSettingsObject.FindProperty("lastSaveObjectName").stringValue);
            if (EditorGUI.EndChangeCheck())
            {
                UtilEditor.EditorSettingsObject.ApplyModifiedProperties();
                UtilEditor.EditorSettingsObject.Update();
            }


            EditorGUI.BeginDisabledGroup(UtilEditor.EditorSettingsObject.FindProperty("lastSaveObjectName").stringValue.Length <= 0);
            string path = string.Empty;
            
            if (GUILayout.Button("Create Save Object"))
            {
                path = EditorUtility.SaveFilePanelInProject("Save New Save Object Class", UtilEditor.EditorSettingsObject.FindProperty("lastSaveObjectName").stringValue + "SaveObject", "cs", "");
                
                UtilEditor.EditorSettingsObject.FindProperty("lastSaveObjectFileName").stringValue =
                    path.Split('/')[path.Split('/').Length - 1].Replace(".cs", string.Empty);
                UtilEditor.EditorSettingsObject.ApplyModifiedProperties();
                
                var script = AssetDatabase.FindAssets($"t:Script {nameof(SaveObjectGenerator)}")[0];
                var pathToTextFile = AssetDatabase.GUIDToAssetPath(script);
                pathToTextFile = pathToTextFile.Replace("SaveObjectGenerator.cs", "SaveObjectTemplate.txt");
                
                
                TextAsset template = AssetDatabase.LoadAssetAtPath<TextAsset>(pathToTextFile);
                template = new TextAsset(template.text);
                var replace = template.text.Replace("%SaveObjectName%",
                    UtilEditor.EditorSettingsObject.FindProperty("lastSaveObjectFileName").stringValue);

                File.WriteAllText(path, replace);
                EditorUtility.SetDirty(AssetDatabase.LoadAssetAtPath<TextAsset>(pathToTextFile));
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                UtilEditor.EditorSettingsObject.FindProperty("justCreatedSaveObject").boolValue = true;
                UtilEditor.EditorSettingsObject.ApplyModifiedProperties();
                UtilEditor.EditorSettingsObject.Update();
                
                EditorUtility.RequestScriptReload();
            }
            
            EditorGUI.EndDisabledGroup();
        }
        
        
        
        private static void CreateSaveObjectInstance()
        {
            if (EditorUtility.DisplayDialog("Create Instance",
                    "Do you want to create a new instance of the save object you just made?", "Yes", "Cancel"))
            {
                var parse = "Save." + UtilEditor.EditorSettingsObject.FindProperty("lastSaveObjectFileName").stringValue;


                var types = AppDomain.CurrentDomain
                    .GetAssemblies()
                    .SelectMany(x => x.GetTypes())
                    .FirstOrDefault(x => x.IsClass && x.FullName == parse && x.IsAssignableFrom(x));

                
                var instance = CreateInstance(types);
                
                var script =
                    AssetDatabase.FindAssets($"t:Script {UtilEditor.EditorSettingsObject.FindProperty("lastSaveObjectFileName").stringValue}")[0];
                var pathToTextFile = AssetDatabase.GUIDToAssetPath(script);

                pathToTextFile = pathToTextFile.Replace(".cs", ".asset");

                AssetDatabase.CreateAsset(instance, pathToTextFile);
            }
        }


        [DidReloadScripts]
        private static void TryCreateInstanceIfMade()
        {
            if (!UtilEditor.EditorSettingsObject.FindProperty("justCreatedSaveObject").boolValue) return;
                
            UtilEditor.EditorSettingsObject.FindProperty("justCreatedSaveObject").boolValue = false;
            UtilEditor.EditorSettingsObject.ApplyModifiedProperties();
            UtilEditor.EditorSettingsObject.Update();
            
            CreateSaveObjectInstance();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}