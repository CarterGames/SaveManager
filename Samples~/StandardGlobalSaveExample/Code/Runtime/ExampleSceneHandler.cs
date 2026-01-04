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

using UnityEngine; 
using UnityEngine.UI;

namespace CarterGames.Assets.SaveManager.Demo
{
    public class ExampleSceneHandler : MonoBehaviour
    {
        private ExampleSaveObject saveObject;

        [Header("Save Input Fields")] 
        [SerializeField] private InputField nameInputField;
        [SerializeField] private InputField healthInputField;
        [SerializeField] private InputField positionXInputField;
        [SerializeField] private InputField positionYInputField;
        [SerializeField] private InputField positionZInputField;
        [SerializeField] private InputField shieldInputField;
        
        [Header("Load Text Display's")]
#pragma warning disable 
        [SerializeField] private Text name;
#pragma warning restore 
        [SerializeField] private Text health;
        [SerializeField] private Text position;
        [SerializeField] private Text shields;


        private void OnEnable()
        {
            saveObject = SaveManager.GetGlobalSaveObject<ExampleSaveObject>();
            LoadExampleData();
        }


        public void SaveExampleData()
        {
            saveObject.playerName.Value = nameInputField.text;
            
            if (healthInputField.text.Length > 0)
            {
                saveObject.playerHealth.Value = int.Parse(healthInputField.text);
            }

            if (positionXInputField.text.Length > 0)
            {
                saveObject.playerPosition.Value = new Vector3(float.Parse(positionXInputField.text),
                    float.Parse(positionYInputField.text), float.Parse(positionZInputField.text));
            }

            if (shieldInputField.text.Length > 0)
            {
                saveObject.playerShield.Value = int.Parse(shieldInputField.text);
            }
            
            SaveManager.SaveGame();
        }


        public void LoadExampleData()
        {
            SaveManager.LoadGame();

            name.text = saveObject.playerName.Value;
            health.text = saveObject.playerHealth.Value.ToString();
            position.text = saveObject.playerPosition.Value.ToString();
            shields.text = saveObject.playerShield.Value.ToString();
        }


        public void ClearExampleData()
        {
            saveObject.ResetObjectSaveValues();
            SaveManager.SaveGame();
        }
    }
}