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
        [SerializeField] private Text name;
        [SerializeField] private Text health;
        [SerializeField] private Text position;
        [SerializeField] private Text shields;


        private void OnEnable()
        {
            saveObject = SaveManager.GetSaveObject<ExampleSaveObject>();
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
            
            saveObject.Save();
            SaveManager.Save();
        }


        public void LoadExampleData()
        {
            saveObject.Load();

            name.text = saveObject.playerName.Value;
            health.text = saveObject.playerHealth.Value.ToString();
            position.text = saveObject.playerPosition.Value.ToString();
            shields.text = saveObject.playerShield.Value.ToString();
        }


        public void ClearExampleData()
        {
            saveObject.ResetObjectSaveValues();
            SaveManager.Save();
        }
    }
}