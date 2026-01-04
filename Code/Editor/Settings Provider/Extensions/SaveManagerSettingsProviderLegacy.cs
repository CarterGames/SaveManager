using CarterGames.Assets.SaveManager.Legacy;
using CarterGames.Shared.SaveManager;
using CarterGames.Shared.SaveManager.Editor;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace CarterGames.Assets.SaveManager.Editor
{
    public static partial class SaveManagerSettingsProvider
    {
        private static readonly GUIContent PortLegacySave = new GUIContent("Port Legacy Save?","Defines if the setup tries to port an old 2.x save to the 3.x structure.");
        private static readonly GUIContent LegacySaveHandlerType = new GUIContent("Legacy Save Handler","Defines the handler used for trying to load legacy 2.x save data to the 3.x structure.");


        public static readonly Evt<ILegacySaveHandler, ILegacySaveHandler> LegacyHandlerSettingChangedEvt = new Evt<ILegacySaveHandler, ILegacySaveHandler>();
        
        
        private static void DrawLegacySaveSettings()
        {
            EditorGUILayout.BeginVertical();
            GUILayout.Space(1.5f);
            
            EditorGUILayout.LabelField("Legacy Save Settings", EditorStyles.boldLabel);
            GUILayout.Space(1.5f);
            
            EditorGUILayout.HelpBox("The legacy setup will attempt to port a 2.x save to the 3.x setup if possible. Note that success rates of this will vary.", MessageType.Info);

            EditorGUILayout.PropertyField(SettingsAssetObject.Fp("tryPortLegacySave"), PortLegacySave);
            
            // Save encryption handler.
            EditorGUILayout.BeginHorizontal();
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.TextField(LegacySaveHandlerType, SettingsAssetObject.Fp("legacySaveHandler").Fpr("type").stringValue);

            EditorGUI.EndDisabledGroup();
            
            if (GUILayout.Button("Select", GUILayout.Width(100)))
            {
                SearchProviderLegacySaveHandlers.GetProvider().SelectionMade.Add(OnLegacySaveHandlerSelectionMade);
                SearchProviderLegacySaveHandlers.GetProvider().Open();
                return;
            }
            
            EditorGUILayout.EndHorizontal();
            
            GUILayout.Space(2.5f);
            EditorGUILayout.EndVertical();
        }
        
        
        private static void OnLegacySaveHandlerSelectionMade(SearchTreeEntry entry)
        {
            SearchProviderLegacySaveHandlers.GetProvider().SelectionMade.Remove(OnLegacySaveHandlerSelectionMade);
            
            var selectedHandler = (ILegacySaveHandler)entry.userData;
            
            if (selectedHandler.GetType().FullName == SettingsAssetObject.Fp("legacySaveHandler").Fpr("type").stringValue)
            {
                return;
            }

            ILegacySaveHandler oldHandler = null;
            
            if (!string.IsNullOrEmpty(SettingsAssetObject.Fp("legacySaveHandler").Fpr("assembly").stringValue))
            {
                var oldAssembly = SettingsAssetObject.Fp("legacySaveHandler").Fpr("assembly").stringValue;
                var oldType = SettingsAssetObject.Fp("legacySaveHandler").Fpr("type").stringValue;
                oldHandler = new AssemblyClassDef(oldAssembly, oldType).GetDefinedType<ILegacySaveHandler>();
            }

            SettingsAssetObject.Fp("legacySaveHandler").Fpr("assembly").stringValue = selectedHandler.GetType().Assembly.FullName;
            SettingsAssetObject.Fp("legacySaveHandler").Fpr("type").stringValue = selectedHandler.GetType().FullName;

            SettingsAssetObject.ApplyModifiedProperties();
            SettingsAssetObject.Update();
            
            LegacyHandlerSettingChangedEvt.Raise(oldHandler, selectedHandler);
        }
    }
}