﻿/*
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
using System.IO;
using CarterGames.Assets.SaveManager.Encryption;
using CarterGames.Assets.SaveManager.Serializiation;
using CarterGames.Assets.SaveManager.Utility;
using UnityEngine;

namespace CarterGames.Assets.SaveManager
{
    /// <summary>
    /// The web save loader, used for loading on web platforms. 
    /// </summary>
    public class WebSaveHandler : ILoadHandler
    {
        /* —————————————————————————————————————————————————————————————————————————————————————————————————————————————
        |   ILoadHandler Implementation
        ————————————————————————————————————————————————————————————————————————————————————————————————————————————— */

        /// <summary>
        /// Processes the game data to load it into the game.
        /// </summary>
        /// <param name="savePath">The save path of the save file.</param>
        /// <returns>The save data loaded into the nested dictionary setup.</returns>
        public SerializableDictionary<string, SerializableDictionary<string, string>> LoadFromFile(string savePath)
        {
            if (!File.Exists(savePath))
            {
                CreateFile(savePath);
            }
            
            
            // Load file config to load save data from...
#if UNITY_EDITOR
            var lastEncryptionOption = (EncryptionOption) UtilRuntime.GetPref(PrefSetting.Encryption, PrefUseOption.EditorCurrent);
#else
            var lastEncryptionOption = (EncryptionOption) UtilRuntime.GetPref(PrefSetting.Encryption, PrefUseOption.Current);
#endif
            
            
            if (lastEncryptionOption != EncryptionOption.Disabled)
            {
                try
                {
                    var jsonData = JsonUtility.FromJson<SerializableDictionary<string, SerializableDictionary<string, string>>> (EncryptionHandler.Decrypt(lastEncryptionOption));

                    var dic = new SerializableDictionary<string, SerializableDictionary<string,string>>();

                    foreach (var kpair in jsonData)
                    {
                        var innerDic = new SerializableDictionary<string, string>();
                        
                        if (kpair.Value != null)
                        {
                            foreach (var innerPair in kpair.Value)
                            {
                                innerDic.Add(innerPair.Key, innerPair.Value);
                            }
                        }

                        dic.Add(kpair.Key, innerDic);
                    }
                    
                    SaveManager.ProcessLoadedData(jsonData);

                    return dic;
                }
                catch (Exception e)
                {
                    SmLog.Error($"Failed to read to {savePath} with the exception: {e}");
                    return new SerializableDictionary<string, SerializableDictionary<string, string>>();
                }
            }
            else
            {
                try
                {
                    SerializableDictionary<string, SerializableDictionary<string, string>> jsonData;

                    using (var stream = new FileStream(AssetAccessor.GetAsset<SettingsAssetRuntime>().SavePath,
                               FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            jsonData = JsonUtility
                                .FromJson<SerializableDictionary<string, SerializableDictionary<string, string>>>(
                                    reader.ReadToEnd());
                            reader.Close();
                        }

                        stream.Close();
                    }


                    var dic = new SerializableDictionary<string, SerializableDictionary<string, string>>();

                    foreach (var kpair in jsonData)
                    {
                        var innerDic = new SerializableDictionary<string, string>();

                        foreach (var innerPair in kpair.Value)
                        {
                            innerDic.Add(innerPair.Key, innerPair.Value);
                        }

                        dic.Add(kpair.Key, innerDic);
                    }

                    SaveManager.ProcessLoadedData(jsonData);
                    
                    return dic;
                }
                catch (Exception e)
                {
                    SmLog.Error($"Failed to read to {savePath} with the exception: {e}");
                    return new SerializableDictionary<string, SerializableDictionary<string, string>>();
                }
            }
        }

        /* —————————————————————————————————————————————————————————————————————————————————————————————————————————————
        |   Methods
        ————————————————————————————————————————————————————————————————————————————————————————————————————————————— */
        
        /// <summary>
        /// Creates the save file if it doesn't exist yet.
        /// </summary>
        /// <param name="path">The path to create the file in.</param>
        private void CreateFile(string path)
        {
            SmLog.Normal("File does not exist to load at the current save path.");
                
            var split = path.Split('/');
            var pathBuilt = string.Empty;
                
            foreach (var s in split)
            {
                if (s.Equals("idbfs"))
                {
                    pathBuilt = s;
                    continue;
                }
                    
                if (s.Equals("save.sf")) continue;

                if (Directory.Exists(pathBuilt + "/" + s))
                {
                    pathBuilt += "/" + s;
                    continue;
                }

                Directory.CreateDirectory(pathBuilt + "/" + s);
                pathBuilt += "/" + s;
            }

            
            using (var stream = new FileStream(path, FileMode.Create))
            {
                stream.Flush();
                stream.Close();
            }
        }
    }
}