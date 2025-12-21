/*
 * Save Manager
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

using System.IO;

namespace CarterGames.Assets.SaveManager
{
    /// <summary>
    /// Handles storing data in a local file.
    /// </summary>
    public class DataLocationLocalFile : IDataLocation
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Gets if the location has data currently stored in it.
        /// </summary>
        /// <param name="path">The path to store the data at.</param>
        /// <returns>If there is data in the location.</returns>
        public bool HasData(string path)
        {
            return File.Exists(path);
        }
        

        /// <summary>
        /// Saves data to the location when called.
        /// </summary>
        /// <param name="path">The path to store the data at.</param>
        /// <param name="data">The data to store.</param>
        public void SaveToLocation(string path, string data)
        {
            if (!HasData(path))
            {
                CreateToDirectory(path);
                CreateSaveFile(path);
            }
                    
            using var stream = new FileStream(path, FileMode.Open, FileAccess.Write, FileShare.Write);
                    
            using (var writer = new StreamWriter(stream))
            {
                stream.SetLength(0);
                writer.Write(data);
                writer.Close();
            }
                        
            stream.Close();
        }
        

        /// <summary>
        /// Loads the data from the location when called.
        /// </summary>
        /// <param name="path">The path to load the data at.</param>
        /// <returns>The data loaded from the location.</returns>
        public string LoadFromLocation(string path)
        {
            if (!HasData(path))
            {
#if UNITY_WEBGL && !UNITY_EDITOR
                CreateWebSaveFile(path);                
#else
                CreateToDirectory(path);
                CreateSaveFile(path);
#endif
                
                return string.Empty;
            }

            string jsonData;

            using var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            
            using (var reader = new StreamReader(stream))
            {
                jsonData = reader.ReadToEnd();
                reader.Close();
            }

            stream.Close();

            return jsonData;
        }
        
        
        /// <summary>
        /// Creates a save file that works on web builds.
        /// </summary>
        /// <param name="webPath">The path for the web build to save to.</param>
        private static void CreateWebSaveFile(string webPath)
        {
            var split = webPath.Split('/');
            var pathBuilt = string.Empty;
                
            foreach (var s in split)
            {
                if (s.Equals("idbfs"))
                {
                    pathBuilt = s;
                    continue;
                }
                    
                if (s.Equals("save.sf2")) continue;

                if (Directory.Exists(pathBuilt + "/" + s))
                {
                    pathBuilt += "/" + s;
                    continue;
                }

                Directory.CreateDirectory(pathBuilt + "/" + s);
                pathBuilt += "/" + s;
            }

            CreateSaveFile(webPath);
        }
        
        
        /// <summary>
        /// Creates to a particular path if it doesn't exist yet.
        /// </summary>
        /// <param name="path">The path to create to.</param>
        private static void CreateToDirectory(string path)
        {
            var currentPath = string.Empty;
            var split = path.Split('/');

            for (var i = 0; i < path.Split('/').Length; i++)
            {
                var element = path.Split('/')[i];
                currentPath += element + "/";

                if (i.Equals(split.Length - 1))continue;
                if (Directory.Exists(currentPath))continue;
                
                Directory.CreateDirectory(currentPath);
            }
        }


        /// <summary>
        /// Creates the save file.
        /// </summary>
        /// <param name="path">The path to save to.</param>
        private static void CreateSaveFile(string path)
        {
            using var stream = new FileStream(path, FileMode.Create);
            
            stream.Flush();
            stream.Close();
        }
    }
}