using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

/*
 * 
 *  Save Manager
 *							  
 *	Save Manager
 *      The main script for managing the saving and loading of a game.
 *			
 *  Written by:
 *      Jonathan Carter
 *
 *  Published By:
 *      Carter Games
 *      E: hello@carter.games
 *      W: https://www.carter.games
 *		
 *  Version: 1.1.0
 *	Last Updated: 24/07/2021 (d/m/y)							
 * 
 */

namespace CarterGames.Assets.SaveManager
{
    /// <summary>
    /// Static Class | The Save Manager class, used the save and load the game. This script can be called from anywhere in your code and can't be attached to a GameObject.
    /// </summary>
    public static class SaveManager
    {
        private static readonly string SavePath = Application.persistentDataPath + "/savefile.sf";

        
        
        /// <summary>
        /// Saves the game, using the SaveData class.
        /// </summary>
        /// <param name="data">The SaveData to save.</param>
        public static void SaveGame(SaveData data)
        {
            // Erased the old save file, done to avoid problems loading as the class changing will cause an error is not done.
            if (File.Exists(SavePath))
                File.Delete(SavePath);

            var _formatter = new BinaryFormatter();
            var _stream = new FileStream(SavePath, FileMode.OpenOrCreate);

            _formatter.Serialize(_stream, data);
            _stream.Close();
        }


        
        /// <summary>
        /// Loads the game and returns an instance of the SaveData class
        /// </summary>
        /// <returns>An instance of the SaveData class with the loaded values</returns>
        public static SaveData LoadGame()
        {
            if (File.Exists(SavePath))
            {
                var _formatter = new BinaryFormatter();
                var _stream = new FileStream(SavePath, FileMode.Open);

                var _data = _formatter.Deserialize(_stream) as SaveData;

                _stream.Close();

                return _data;
            }

            Debug.LogError("Save Manager | CG : Error Code 1 : Save file not found!");
            return null;
        }


        
        /// <summary>
        /// Resets the save file to be a blank file with no values saved in it.
        /// </summary>
        public static void ResetSaveFile()
        {
            // Erased the old save file, done to avoid problems loading as the class changing will cause an error is not done.
            if (File.Exists(SavePath))
                File.Delete(SavePath);

            var _formatter = new BinaryFormatter();
            var _stream = new FileStream(SavePath, FileMode.OpenOrCreate);

            _formatter.Serialize(_stream, new SaveData());
            _stream.Close();
        }
        
        
        
        
        /// <summary>
        /// Delete the save file and doesn't replace it with any new data...
        /// </summary>
        public static void DeleteSaveFile()
        {
            if (File.Exists(SavePath))
                File.Delete(SavePath);
        }
        

        

        /// <summary>
        /// Checks to see if there is a save file already in the directory.
        /// </summary>
        /// <returns>Bool</returns>
        public static bool HasSaveFile()
        {
            return File.Exists(SavePath);
        }
    }
}