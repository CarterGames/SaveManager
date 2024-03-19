/*
 * Copyright (c) 2024 Carter Games
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

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CarterGames.Common
{
    /// <summary>
    /// A helper class for getting references to other scripts.
    /// </summary>
    public static class Ref
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Gets the first of any and all of the type requested from the active scene.
        /// </summary>
        /// <typeparam name="T">The type to get</typeparam>
        /// <returns>First instance of the type found in the active scene</returns>
        public static T GetComponentFromScene<T>()
        {
            var allOfType = GetComponentsFromScene<T>();

            return allOfType.Count > 0 
                ? allOfType[0] 
                : default;
        }
        
        
        /// <summary>
        /// Gets the first of any and all of the type requested from all scenes...
        /// </summary>
        /// <typeparam name="T">The type to get</typeparam>
        /// <returns>First instance of the type found in all scenes</returns>
        public static T GetComponentFromAllScenes<T>()
        {
            var allOfType = GetComponentsFromAllScenes<T>();

            return allOfType.Count > 0 
                ? allOfType[0] 
                : default;
        }
        
        
        /// <summary>
        /// Gets any and all of the type requested from the active scene...
        /// </summary>
        /// <typeparam name="T">The type to get</typeparam>
        /// <returns>List of any instances of the type found in the scene</returns>
        public static List<T> GetComponentsFromScene<T>()
        {
            var objs = new List<GameObject>();
            var scene = SceneManager.GetActiveScene();
            var validObjectsFromScene = new List<T>();
            
            scene.GetRootGameObjects(objs);
            
            foreach (var go in objs)
            {
                validObjectsFromScene.AddRange(go.GetComponentsInChildren<T>(true));
            }

            return validObjectsFromScene;
        }
        
        
        /// <summary>
        /// Gets any and all of the type requested from all scenes...
        /// </summary>
        /// <typeparam name="T">The type to get</typeparam>
        /// <returns>List of any instances of the type found in all scenes</returns>
        public static List<T> GetComponentsFromAllScenes<T>()
        {
            var objs = new List<GameObject>();
            var scenes = new List<Scene>();
            var validObjectsFromScene = new List<T>();

            for (var i = 0; i < SceneManager.sceneCount; i++)
            {
                scenes.Add(SceneManager.GetSceneAt(i));
            }

            foreach (var scene in scenes)
            {
                objs.AddRange(scene.GetRootGameObjects());
            }

            foreach (var go in objs)
            {
                validObjectsFromScene.AddRange(go.GetComponentsInChildren<T>(true));
            }

            return validObjectsFromScene;
        }
    }
}