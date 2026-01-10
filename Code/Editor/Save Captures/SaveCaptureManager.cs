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

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CarterGames.Shared.SaveManager;
using CarterGames.Shared.SaveManager.Editor;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;

namespace CarterGames.Assets.SaveManager.Editor
{
    public static class SaveCaptureManager
    {
        private const string CapturesPath = "Assets/Plugins/Carter Games/Save Manager/Captures";
        
        private static IEnumerable<SaveCaptureDefine> Captures { get; set; }


        public static bool TryGetAllCaptures(out IEnumerable<SaveCaptureDefine> captures)
        {
            Captures = GetCapturesFromDirectory();
            captures = Captures;
            return captures.Any();
        }


        private static IEnumerable<SaveCaptureDefine> GetCapturesFromDirectory()
        {
            if (!Directory.Exists(CapturesPath)) return Array.Empty<SaveCaptureDefine>();
            
            var assets = Directory.GetFiles(CapturesPath).Where(t => !t.Contains(".meta"));
            var files = new List<SaveCaptureDefine>();
            
            foreach (var entry in assets)
            {
                try
                {
                    files.Add(new SaveCaptureDefine(AssetDatabase.LoadAssetAtPath<TextAsset>(entry)));
                }
#pragma warning disable 0168
                catch (Exception e)
#pragma warning restore 0168
                {
                    SmDebugLogger.LogWarning(SaveManagerErrorCode.SaveCaptureSaveFailed.GetErrorMessageFormat(entry, e.Message, e.StackTrace));
                    continue;
                }
            }

            return files;
        }


        public static void CaptureCurrentEditorSave(string captureName)
        {
            SaveManager.SaveGame();
            
            var profileSavePath = $"{CapturesPath}/{captureName}.json";
            var json = SmAssetAccessor.GetAsset<DataAssetSettings>().Location.LoadDataFromLocation();
            
            FileEditorUtil.CreateToDirectory(profileSavePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            File.WriteAllText(profileSavePath, json);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        
        
        public static void CaptureFromBackup(JToken backup)
        {
            var profileSavePath = $"{CapturesPath}/backup_capture_{DateTime.Now.ToOADate().ToString(CultureInfo.InvariantCulture)}.json";
            var json = backup.ToString();
            
            FileEditorUtil.CreateToDirectory(profileSavePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            File.WriteAllText(profileSavePath, json);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }


        public static void LoadCapture(TextAsset saveFile)
        {
            SmAssetAccessor.GetAsset<DataAssetSettings>().Location.SaveDataToLocation(saveFile.text);
            SaveManager.LoadGame();
        }
    }
}