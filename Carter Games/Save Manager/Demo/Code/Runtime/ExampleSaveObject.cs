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

namespace CarterGames.Assets.SaveManager.Demo
{
    [CreateAssetMenu(fileName = "Example Save Object", menuName = "Carter Games/Save Manager/Demo/Example Save Object Asset", order = 20)]
    public class ExampleSaveObject : SaveObject
    {
        public SaveValue<string> playerName = new SaveValue<string>("examplePlayerName");
        public SaveValue<int> playerHealth = new SaveValue<int>("examplePlayerHealth");
        public SaveValue<Vector3> playerPosition = new SaveValue<Vector3>("examplePlayerPosition");
        public SaveValue<int> playerShield = new SaveValue<int>("examplePlayerShield");
    }
}