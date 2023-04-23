/*
 * Copyright (c) 2018-Present Carter Games
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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CarterGames.Assets.SaveManager.Encryption;
using CarterGames.Assets.SaveManager.Serializiation;
using CarterGames.Assets.SaveManager.Utility;
using CarterGames.Common;
using UnityEngine;

namespace CarterGames.Assets.SaveManager
{
    public static class SaveManager
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        // The actual save data asset holding the data in the project.
        private static SaveData SaveData => AssetAccessor.GetAsset<SettingsAssetRuntime>().SaveData;

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Properties
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        #region Properties
        
        
        /// <summary>
        /// Gets if the manager is initialized.
        /// </summary>
        public static bool IsInitialized { get; private set; }

        
        /// <summary>
        /// Gets if the save manager is currently saving the game.
        /// </summary>
        public static bool IsSaving { get; private set; }

        
        /// <summary>
        /// Gets if the save file exists at the current save path.
        /// </summary>
        private static bool HasSaveFile => File.Exists(SavePath);


        /// <summary>
        /// Gets the save path for the current save setup.
        /// </summary>
        private static string SavePath => AssetAccessor.GetAsset<SettingsAssetRuntime>().SavePath;


        /// <summary>
        /// Gets the save path for the current save setup.
        /// </summary>
        private static bool Prettify => AssetAccessor.GetAsset<SettingsAssetRuntime>().Prettify;


        /// <summary>
        /// Gets the current encryption setting setup.
        /// </summary>
        private static EncryptionOption EncryptionSetting => AssetAccessor.GetAsset<SettingsAssetRuntime>().Encryption;
        
        
        /// <summary>
        /// A lookup of all the save objects in the save data. 
        /// </summary>
        private static Dictionary<string, SaveObject> SaveDataLookup { get; set; }


        /// <summary>
        /// A lookup of all the loaded data.
        /// </summary>
        private static SerializableDictionary<string, SerializableDictionary<string, string>> LoadedDataLookup { get; set; } = new SerializableDictionary<string, SerializableDictionary<string, string>>();



        private static ILoadHandler loadHandler;
        
        #endregion

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Events
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        #region Events

        
        /// <summary>
        /// Raises when the game is saved.
        /// </summary>
        public static readonly Evt Saved = new Evt();
        
        
        /// <summary>
        /// Raises when the game is loaded.
        /// </summary>
        public static readonly Evt Loaded = new Evt();

        
        #endregion
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Initialization Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        #region Initialization Methods

        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        private static void RuntimeInitializeOnLoad() 
        {
            IsInitialized = false;
            Initialize();
        }


        /// <summary>
        /// Initializes the save manager for use.
        /// </summary>
        private static void Initialize()
        {
            if (IsInitialized) return;

#if UNITY_WEBGL && !UNITY_EDITOR
            loadHandler = new WebSaveHandler();
#else
            loadHandler = new StandardSaveHandler();
#endif        
            
            TryGenerateSaveLookup();

            if (SaveData != null)
            {
                if (AssetAccessor.GetAsset<SettingsAssetRuntime>().AutoLoad)
                {
                    LoadedDataLookup = LoadFromFile();
                }
                else
                {
                    LoadedDataLookup = new SerializableDictionary<string, SerializableDictionary<string, string>>();
                }
            }
            else
            {
                LoadedDataLookup = new SerializableDictionary<string, SerializableDictionary<string, string>>();
            }


            foreach (var saveValue in SaveDataLookup.ToList())
            {
                saveValue.Value.Load();
            }
            

            IsInitialized = true;
            Loaded.Raise();
        }


        /// <summary>
        /// Registers the save object to the save manager.
        /// </summary>
        /// <param name="saveObject">The object to register.</param>
        public static void RegisterObject(SaveObject saveObject)
        {
            SetSaveObjectData(saveObject);
        }

        
        #endregion

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   File Management Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        #region File Management Methods
        
        
        /// <summary>
        /// Saves the current data to the file.
        /// </summary>
        /// <param name="data">The data to save.</param>
        private static void SaveToFile(SerializableDictionary<string, SerializableDictionary<string, string>> data)
        {
            // Updates the encryption setting before saving the game.
#if UNITY_EDITOR
            if (UtilRuntime.HasPref(PrefSetting.Encryption, PrefUseOption.EditorCurrent))
            {
                if (UtilRuntime.GetPref(PrefSetting.Encryption, PrefUseOption.EditorCurrent) !=
                    UtilRuntime.GetPref(PrefSetting.Encryption, PrefUseOption.Editor))
                {
                    UtilRuntime.SetPref(PrefSetting.Encryption, PrefUseOption.EditorCurrent,
                        UtilRuntime.GetPref(PrefSetting.Encryption, PrefUseOption.Editor));
                }
            }
            else
            {
                UtilRuntime.SetPref(PrefSetting.Encryption, PrefUseOption.EditorCurrent, (int) EncryptionSetting);
            }
#else
            if (UtilRuntime.HasPref(PrefSetting.Encryption, PrefUseOption.Current))
            {
                if (UtilRuntime.GetPref(PrefSetting.Encryption, PrefUseOption.Current) !=
                    UtilRuntime.GetPref(PrefSetting.Encryption, PrefUseOption.Build))
                {
                    UtilRuntime.SetPref(PrefSetting.Encryption, PrefUseOption.Current,
                        UtilRuntime.GetPref(PrefSetting.Encryption, PrefUseOption.Build));
                }
            }
            else
            {
                UtilRuntime.SetPref(PrefSetting.Encryption, PrefUseOption.Current, (int) EncryptionSetting);
            }
#endif
            
            
            var jsonString = JsonUtility.ToJson(data, Prettify);
            
            
            if (EncryptionSetting == EncryptionOption.Disabled)
            {
                try
                {
                    using (var stream = new FileStream(SavePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write))
                    {
                        using (var writer = new StreamWriter(stream))
                        {
                            stream.SetLength(0);
                            writer.Write(jsonString);
                            writer.Close();
                        }
                        
                        stream.Close();
                    }
                }
                catch (Exception e)
                {
                    SmLog.Error($"Failed to write to {SavePath} without encryption with the exception: {e}");
                }
            }
            else
            {
                try
                {
                    EncryptionHandler.EncryptData(jsonString);
                }
                catch (Exception e)
                {
                    SmLog.Error($"Failed to write to {SavePath} with the exception: {e}");
                }
            }
            
#if UNITY_WEBGL && !UNITY_EDITOR
            Application.ExternalEval("_JS_FileSystem_Sync();");
#endif
        }

        
        /// <summary>Load Generic Data.
        /// Load file as Object from custom Path.
        /// </summary>
        /// <returns>The data loaded from the file.</returns>
        private static SerializableDictionary<string, SerializableDictionary<string, string>> LoadFromFile()
        {
            return loadHandler.LoadFromFile(SavePath);
        }

        

        public static void ProcessLoadedData(SerializableDictionary<string, SerializableDictionary<string, string>> jsonData)
        {
            var keys = jsonData.Keys.ToList();
            
            if (keys.Count <= 0) return;
            
            foreach (var saveObject in SaveData.Data)
            {
                if (keys.Contains(saveObject.SaveKey))
                {
                    if (jsonData[saveObject.SaveKey] == null)
                    {
                        continue;
                    }
                    
                    foreach (var pair in jsonData[saveObject.SaveKey])
                    {
                        var t = saveObject.GetSaveValue()[pair.Key].GetType();
                        saveObject.SetValue(pair.Key, ((SaveValueBase) JsonUtility.FromJson(jsonData[saveObject.SaveKey][pair.Key], t)).ValueObject);
                    }
                }
            }
        }
        
        
        /// <summary>
        /// Deletes the current save file from the current save path.
        /// </summary>
#if UNITY_EDITOR
        [UnityEditor.MenuItem("Tools/Carter Games/Save Manager/Delete Save Data", priority = 32)]
#endif
        private static void DeleteSaveFile()
        {
            if (!HasSaveFile) return;
            File.Delete(SavePath);
            LoadedDataLookup?.Clear();
            SaveDataLookup?.Clear();
        }
        

        #endregion

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Save Handling Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        #region Save Handling Methods

        
        /// <summary>
        /// Saves the game when called.
        /// </summary>
        public static void Save(bool callListeners = true)
        {
            IsSaving = true;
            
            if (callListeners)
            {
                CallSaveListeners(true);
            }

            TryGenerateSaveLookup();
            
            foreach (var saveValue in SaveDataLookup.Values.ToList())
            {
                RegisterObject(saveValue);
            }

            SaveToFile(SaveData.SerializableData);
            IsSaving = false;

            if (callListeners)
            {
                CallSaveListeners(false);
            }

            Saved.Raise();
        }


        
        /// <summary>
        /// Loads the game when called.
        /// </summary>
        public static void Load(SerializableDictionary<string, SerializableDictionary<string, string>> data = null, bool callListeners = true)
        {
            if (callListeners)
            {
                CallSaveListeners(true);
            }
            
            var loadedData = data ?? LoadFromFile();
            
            if (SaveData != null)
            {
                LoadedDataLookup = loadedData;
            }
            else
            {
                LoadedDataLookup = new SerializableDictionary<string, SerializableDictionary<string, string>>();
            }


            TryGenerateSaveLookup();
            

            foreach (var saveValue in SaveDataLookup.ToList())
            {
                saveValue.Value.Load();
            }
            
            if (callListeners)
            {
                CallSaveListeners(false);
            }
            
            Loaded.Raise();
        }

        
        
        /// <summary>
        /// Clears the save data when called.
        /// </summary>
        public static void Clear()
        {
            TryGenerateSaveLookup(true);
            
            
            if (SaveDataLookup != null)
            {
                if (SaveDataLookup.Count > 0)
                {
                    foreach (var saveValue in SaveDataLookup.Values.ToList())
                    {
                        saveValue.ResetObjectSaveValues();
                    }
                }
            }
            
            SaveToFile(SaveData.SerializableData);
        }
        

        #endregion

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Get/TryGet Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        #region Get/TryGet Methods (Save Objects / Save Values)

        
        /// <summary>
        /// Gets the first save object with the matching type.
        /// </summary>
        /// <typeparam name="T">The type to get.</typeparam>
        /// <returns>A save object of the type entered.</returns>
        public static T GetSaveObject<T>() where T : SaveObject
        {
            foreach (var saveObj in SaveDataLookup.Values)
            {
                if (saveObj.GetType() != typeof(T)) continue;
                return (T) saveObj;
            }

            return null;
        }
        
        
        /// <summary>
        /// Gets the save object with the matching key and type.
        /// </summary>
        /// <param name="saveObjectKey">The key to find.</param>
        /// <typeparam name="T">The type to get.</typeparam>
        /// <returns>A save object of the type entered.</returns>
        public static T GetSaveObject<T>(string saveObjectKey) where T : SaveObject
        {
            if (SaveDataLookup.ContainsKey(saveObjectKey))
            {
                return (T) SaveDataLookup[saveObjectKey];
            }

            return null;
        }


        /// <summary>
        /// Tries to get the first save object of the matching type.
        /// </summary>
        /// <param name="saveObject">The object found or null.</param>
        /// <typeparam name="T">The type to get.</typeparam>
        /// <returns>If it was successful or not.</returns>
        public static bool TryGetSaveObject<T>(out T saveObject) where T : SaveObject
        {
            saveObject = GetSaveObject<T>();
            return saveObject != null;
        }
        
        
        /// <summary>
        /// Tries to get the save object with the matching key.
        /// </summary>
        /// <param name="saveObjectKey">The key to find.</param>
        /// <param name="saveObject">The object found or null.</param>
        /// <typeparam name="T">The type to get.</typeparam>
        /// <returns>If it was successful or not.</returns>
        public static bool TryGetSaveObject<T>(string saveObjectKey, out T saveObject) where T : SaveObject
        {
            saveObject = GetSaveObject<T>(saveObjectKey);
            return saveObject != null;
        }

        
        /// <summary>
        /// Get the save object of the entered save key.
        /// </summary>
        /// <param name="saveKey">The key to look for.</param>
        /// <returns>The object found or null.</returns>
        private static SerializableDictionary<string, string> GetSaveValuesLookup(string saveKey)
        {
            if (LoadedDataLookup is { } && LoadedDataLookup.Count > 0)
            {
                if (LoadedDataLookup.ContainsKey(saveKey))
                {
                    return LoadedDataLookup[saveKey];
                }
            }


            if (!SaveDataLookup.ContainsKey(saveKey)) return null;
            
            var d = SaveDataLookup[saveKey].GetSaveValue();
            var converted = new SerializableDictionary<string, string>();

            foreach (var pair in d)
            {
                converted.Add(pair.Key, JsonUtility.ToJson(pair.Value, Prettify));
            }
                
            return converted;
        }
        

        /// <summary>
        /// Tries to get the save object of the entered save key.
        /// </summary>
        /// <param name="saveKey">The key to look for.</param>
        /// <param name="data">The data found or null.</param>
        /// <returns>If it was successful or not.</returns>
        public static bool TryGetSaveValuesLookup(string saveKey, out SerializableDictionary<string, string> data)
        {
            data = GetSaveValuesLookup(saveKey);
            return data != null;
        }
        

        #endregion
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Utility Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Resets a value in the save if it exists.
        /// </summary>
        /// <param name="objectKey">The object key.</param>
        /// <param name="valueKey">The value key.</param>
        /// <returns>If it was successful.</returns>
        public static bool TryResetElementFromSave(string objectKey, string valueKey)
        {
            if (!IsInitialized)
            {
                Initialize();
            }


            if (!LoadedDataLookup.ContainsKey(objectKey)) return false;
            if (!LoadedDataLookup[objectKey].ContainsKey(valueKey)) return false;

            
            LoadedDataLookup[objectKey].Remove(valueKey);
            SaveToFile(SaveData.SerializableData);
            return true;
        }

        
        /// <summary>
        /// Updates the save object. 
        /// </summary>
        /// <param name="saveObject">The object o update in the save.</param>
        public static void UpdateAndSaveObject(SaveObject saveObject)
        {
            if (!IsInitialized)
            {
                Initialize();
            }
            
            
            if (!TryGetSaveValuesLookup(saveObject.SaveKey, out var data)) return;
            
            
            var d = saveObject.GetSaveValue();
            var converted = new SerializableDictionary<string, string>();

            foreach (var pair in d)
            {
                converted.Add(pair.Key, JsonUtility.ToJson(pair.Value, Prettify));
            }

            data = converted;
            
            
            if (LoadedDataLookup is { } && LoadedDataLookup.Count > 0)
            {
                LoadedDataLookup[saveObject.SaveKey] = data;
            }
            else
            {
                LoadedDataLookup = new SerializableDictionary<string, SerializableDictionary<string, string>>()
                {
                    { saveObject.SaveKey, data }
                };
            }
        }

        
        
        private static void TryGenerateSaveLookup(bool ignoreCheck = false)
        {
            if (!ignoreCheck)
            {
                if (SaveDataLookup != null) return;
            }
            
            SaveDataLookup = new Dictionary<string, SaveObject>();
                
            foreach (var saveValue in SaveData.Data)
            {
                if (SaveDataLookup.ContainsKey(saveValue.SaveKey)) 
                    SaveDataLookup[saveValue.SaveKey] = saveValue;
                else
                    SaveDataLookup.Add(saveValue.SaveKey, saveValue);
            }
        }


        private static void SetSaveObjectData(SaveObject saveObject)
        {
            TryGenerateSaveLookup();
            
            if (SaveDataLookup.ContainsKey(saveObject.SaveKey))
            {
                SaveDataLookup[saveObject.SaveKey] = saveObject;
            }
            else
            {
                SaveDataLookup.Add(saveObject.SaveKey, saveObject);
            }
        }
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Listener Call Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        #region Listener Call Methods

        
        /// <summary>
        /// Calls for all the ISaveListener implementations to be run if any are found.
        /// </summary>
        /// <param name="saveCalled">Was it a called call?</param>
        private static void CallSaveListeners(bool saveCalled)
        {
            if (!Application.isPlaying) return;

            var listeners = Ref.GetComponentsFromAllScenes<ISaveListener>();

            if (listeners.Count <= 0) return;

            foreach (var listener in listeners)
            {
                if (saveCalled)
                {
                    listener.OnGameSaveCalled();
                }
                else
                {
                    listener.OnGameSaveCompleted();
                }
            }
        }
        
        
        /// <summary>
        /// Calls for all the ILoadListener implementations to be run if any are found.
        /// </summary>
        /// <param name="loadCalled">Was it a called call?</param>
        private static void CallLoadListeners(bool loadCalled)
        {
            if (!Application.isPlaying) return;

            var listeners = Ref.GetComponentsFromAllScenes<ILoadListener>();

            if (listeners.Count <= 0) return;
            
            foreach (var listener in listeners)
            {
                if (loadCalled)
                {
                    listener.OnGameLoadCalled();
                }
                else
                {
                    listener.OnGameLoadCompleted();
                }
            }
        }
        
        
        #endregion
    }
}