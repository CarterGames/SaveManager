using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace CarterGames.Assets.SaveManager.Editor
{
    public class SaveManagerEditorCache : IAssetEditorInitialize
    {
        private static List<SaveObject> saveObjects = new List<SaveObject>();
        private static List<string> saveObjectPaths = new List<string>();
        
        private static Dictionary<SaveObject, SaveObjectEditor> editorsLookup = new Dictionary<SaveObject, SaveObjectEditor>();
        private static Dictionary<SaveObject, SerializedObject> soLookup = new Dictionary<SaveObject, SerializedObject>();


        public static bool HasCache { get; private set; }
        
        public static List<SaveObject> SaveObjects => saveObjects;
        public static List<string> SaveObjectAssetPaths => saveObjectPaths;
        public static Dictionary<SaveObject, SaveObjectEditor> EditorsLookup => editorsLookup;
        public static Dictionary<SaveObject, SerializedObject> SoLookup => soLookup;


        public static void RefreshCache()
        {
            RefreshSaveObjectsCache();
            RefreshSaveObjectAssetPathsCache();
            RefreshSaveObjectSerializedObjectsCache();
            RefreshSaveObjectEditorsCache();
            
            if (HasCache) return;
            HasCache = true;
        }


        private static void RefreshSaveObjectsCache()
        {
            saveObjects = new List<SaveObject>();
            saveObjects = UtilEditor.SaveData.Data.Where(t => t != null).ToList();
            
            if (!saveObjects.HasNullEntries()) return;
            
            saveObjects = (List<SaveObject>) saveObjects.RemoveMissing().RemoveDuplicates<SaveObject>();
            UtilEditor.SaveData.Data = saveObjects;
        }
        
        
        private static void RefreshSaveObjectAssetPathsCache()
        {
            saveObjectPaths = new List<string>();
            
            foreach (var saveObj in SaveObjects)
            {
                saveObjectPaths.Add(AssetDatabase.GetAssetPath(saveObj));
            }
        }

        
        private static void RefreshSaveObjectSerializedObjectsCache()
        {
            soLookup = new Dictionary<SaveObject, SerializedObject>();
            
            foreach (var saveObj in SaveObjects)
            {
                soLookup.Add(saveObj, new SerializedObject(saveObj));
            }
        }
        
        
        private static void RefreshSaveObjectEditorsCache()
        {
            editorsLookup = new Dictionary<SaveObject, SaveObjectEditor>();
            
            foreach (var saveObj in soLookup)
            {
                editorsLookup.Add(saveObj.Key, (SaveObjectEditor) UnityEditor.Editor.CreateEditor(saveObj.Value.targetObject));
            }
        }

        public int InitializeOrder => -100;


        public void OnEditorInitialized()
        {
            RefreshCache();
        }
    }
}