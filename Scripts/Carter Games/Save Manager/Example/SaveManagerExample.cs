using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

/*
 * 
 *  Save Manager
 *							  
 *	Save Manager Example
 *      This is not needed for the asset to work, it is just used in the example scene.
 *      Please do not edit this code as it will break the example scene provided with this asset
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

namespace CarterGames.Assets.SaveManager.Example
{
    /// <summary>
    /// EXAMPLE ONLY - DO NOT EDIT!
    /// (CLASS) - example class showing the saving and loading of data, used in the example scene.
    /// NOTE - this class only works if the SaveData has not being edited by you, once it is edited the scene will not function.
    /// </summary>
    public class SaveManagerExample : MonoBehaviour
    {
        // Input values
        [Header("Input Fields")]
        public Text playerName;
        public Text playerHealth;
        public Text playerPosition0;
        public Text playerPosition1;
        public Text playerPosition2;
        public Text playerShield;
        public Image playerSprite;

        // An instance of the custom class used in the example
        private ExampleClass exampleClass = new ExampleClass();

        // output values
        [Header("Output Text Components")]
        public Text displayPlayerName;
        public Text displayPlayerHealth;
        public Text displayPlayerPosition;
        public Text displayPlayerShield;
        public Image displayPlayerSprite;


        /// <summary>
        /// Saves the data by making a new save data and passing the values entered into the scene into it, you don't have to edit all the values each time you save, this example does.
        /// NOTE: int/float.phase() is used here as we are using input fields in the scene, you'd normally have the correct value type to hand, with UI Input fields we don't, so we have to convert them in this example.
        /// </summary>
        public void SaveGame()
        {
            // For you this would be ' SaveData ' not ' ExampleSaveData '
            var saveData = new ExampleSaveData();

            // Passing in values to be saved
            saveData.examplePlayerName = playerName.text;
            saveData.examplePlayerHealth = float.Parse(playerHealth.text);

            // Creating a SaveVector3 and setting the Vector3 up before saving it.
            var exampleVec = new Vector3(float.Parse(playerPosition0.text), float.Parse(playerPosition1.text), float.Parse(playerPosition2.text));
            saveData.examplePlayerPosition = exampleVec;

            saveData.examplePlayerSprite = playerSprite.sprite;

            exampleClass.shieldAmount = int.Parse(playerShield.text);
            saveData.exampleCustomClass = exampleClass;

            // you would just call ' SaveManager.SaveGame(saveData) ' for the sake of not requiring the project to be set up one way to show the asset working... 
            // ...there is an example save manager on this script which runs just like the normal one.
            ExampleSaveManager.SaveGame(saveData);
        }


        /// <summary>
        /// Loads the data and puts it into the display to show it has loaded the entered values
        /// </summary>
        public void LoadGame()
        {
            var loadData = ExampleSaveManager.LoadGame();

            displayPlayerName.text = loadData.examplePlayerName;
            displayPlayerHealth.text = loadData.examplePlayerHealth.ToString();
            displayPlayerPosition.text = loadData.examplePlayerPosition.ToString();
            displayPlayerShield.text = loadData.exampleCustomClass.shieldAmount.ToString();
            displayPlayerSprite.sprite = loadData.examplePlayerSprite;
        }
    }

    /// <summary>
    /// EXAMPLE ONLY - DO NOT EDIT!
    /// </summary>
    [System.Serializable]
    public class ExampleSaveData
    {
        public string examplePlayerName;
        public float examplePlayerHealth;
        public SaveVector3 examplePlayerPosition;
        public ExampleClass exampleCustomClass;
        public SaveSprite examplePlayerSprite;
    }

    
    /// <summary>
    /// EXAMPLE ONLY - DO NOT EDIT!
    /// </summary>
    public class ExampleSaveManager
    {
        /// <summary>
        /// EXAMPLE ONLY | Static | Saves the game, using the ExampleSaveData class.
        /// </summary>
        /// <param name="_data">The ExampleSaveData to save.</param>
        public static void SaveGame(ExampleSaveData _data)
        {
            var savePath = Application.persistentDataPath + "/savefile.example";

            // Erased the old save file, done to avoid problems loading as the class changing will cause an error is not done.
            if (File.Exists(savePath))
                File.Delete(savePath);

            var formatter = new BinaryFormatter();
            var stream = new FileStream(savePath, FileMode.OpenOrCreate);

            formatter.Serialize(stream, _data);
            stream.Close();
        }

        /// <summary>
        /// EXAMPLE ONLY | Static | Loads the game and returns an instance of the ExampleSaveData class
        /// </summary>
        /// <returns>An instance of the SaveData class with the loaded values</returns>
        public static ExampleSaveData LoadGame()
        {
            var savePath = Application.persistentDataPath + "/savefile.example";

            if (File.Exists(savePath))
            {
                var formatter = new BinaryFormatter();
                var stream = new FileStream(savePath, FileMode.Open);

                var _data = formatter.Deserialize(stream) as ExampleSaveData;

                stream.Close();

                return _data;
            }

            Debug.LogError("Save file not found!");
            return null;
        }
    }

    /// <summary>
    /// EXAMPLE ONLY - DO NOT EDIT!
    /// (CLASS) Example class used to show how the class value can be used in the asset
    /// NOTE: The class has to have the "Serializable" attribute to work.
    /// NOTE: This script is not needed for the asset package to function.
    /// </summary>
    [System.Serializable]
    public class ExampleClass
    {
        [SerializeField] public int shieldAmount = 5;
    }
}