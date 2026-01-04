using CarterGames.Assets.SaveManager.Encryption;
using CarterGames.Shared.SaveManager;
using CarterGames.Shared.SaveManager.Editor;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace CarterGames.Assets.SaveManager.Editor
{
    public static partial class SaveManagerSettingsProvider
    {
        private static readonly GUIContent UseEncryption = new GUIContent("Encrypt Save","Defines if the save encryption is used.");
        private static readonly GUIContent SaveEncryptionType = new GUIContent("Save Encryption","Defines the handler used for the save file encryption.");


        public static readonly Evt<ISaveEncryptionHandler, ISaveEncryptionHandler> EncryptionSettingChangedEvt = new Evt<ISaveEncryptionHandler, ISaveEncryptionHandler>();


        private static int CachedTotalEncryptionHandlers = -1;


        private static void DrawSaveEncryptionSettings()
        {
            EditorGUILayout.BeginVertical();
            GUILayout.Space(1.5f);

            EditorGUILayout.LabelField("Encryption Settings", EditorStyles.boldLabel);
            GUILayout.Space(1.5f);


            // Save encryption handler.
            if (CachedTotalEncryptionHandlers == -1)
            {
                CachedTotalEncryptionHandlers = AssemblyHelper.CountClassesOfType<ISaveEncryptionHandler>(false);
            }

            if (CachedTotalEncryptionHandlers <= 0)
            {
                EditorGUILayout.HelpBox(
                    "No encryption handlers in the project. Make a class implementing the ISaveEncryptionHandler interface to be able to select it here.",
                    MessageType.Info);
            }

            EditorGUI.BeginDisabledGroup(CachedTotalEncryptionHandlers <= 0);

            EditorGUILayout.PropertyField(SettingsAssetObject.Fp("encryptSave"), UseEncryption);

            EditorGUILayout.BeginHorizontal();
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.TextField(SaveEncryptionType,
                SettingsAssetObject.Fp("encryptionHandler").Fpr("type").stringValue);
            EditorGUI.EndDisabledGroup();

            if (GUILayout.Button("Select", GUILayout.Width(100)))
            {
                SearchProviderSaveEncryptionHandlers.GetProvider().SelectionMade.Add(OnSaveEncryptionSelectionMade);
                SearchProviderSaveEncryptionHandlers.GetProvider().Open();
                return;
            }

            EditorGUI.EndDisabledGroup();

            EditorGUILayout.EndHorizontal();

            GUILayout.Space(2.5f);
            EditorGUILayout.EndVertical();
        }


        private static void OnSaveEncryptionSelectionMade(SearchTreeEntry entry)
        {
            SearchProviderSaveEncryptionHandlers.GetProvider().SelectionMade.Remove(OnSaveEncryptionSelectionMade);
            
            var selectedHandler = (ISaveEncryptionHandler)entry.userData;
            
            if (selectedHandler.GetType().FullName == SettingsAssetObject.Fp("encryptionHandler").Fpr("type").stringValue)
            {
                return;
            }

            var oldAssembly = SettingsAssetObject.Fp("encryptionHandler").Fpr("assembly").stringValue;
            var oldType = SettingsAssetObject.Fp("encryptionHandler").Fpr("type").stringValue;
            var oldHandler = new AssemblyClassDef(oldAssembly, oldType).GetDefinedType<ISaveEncryptionHandler>();

            SettingsAssetObject.Fp("encryptionHandler").Fpr("assembly").stringValue = selectedHandler.GetType().Assembly.FullName;
            SettingsAssetObject.Fp("encryptionHandler").Fpr("type").stringValue = selectedHandler.GetType().FullName;

            SettingsAssetObject.ApplyModifiedProperties();
            SettingsAssetObject.Update();
            
            EncryptionSettingChangedEvt.Raise(oldHandler, selectedHandler);
        }
    }
}