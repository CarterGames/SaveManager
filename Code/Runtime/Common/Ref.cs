using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CarterGames.Common
{
    /// <summary>
    /// Provides a solution to get references from the active scenes.
    /// </summary>
    public sealed class Ref : MonoBehaviour
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        private static Ref instance;

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Unity Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        private void Awake()
        {
            if (instance != null) Destroy(this.gameObject);
            instance = this;
        }
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        /// <summary>
        /// Handles the initialization of the class as soon as the game is loaded.
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        private static void Initialise()
        {
            if (instance != null) return;

            var obj = new GameObject("Save Manager (Reference Helper)");
            DontDestroyOnLoad(obj);
            obj.AddComponent<Ref>();
        }
        
        
        /// <summary>
        /// Gets any and all of the type requested from any active scene...
        /// </summary>
        /// <typeparam name="T">The type to get</typeparam>
        /// <returns>List of any instances of the type found in the scene</returns>
        public static List<T> GetComponentsFromAllScenes<T>()
        {
            var objects = new List<GameObject>();
            var scenes = new List<Scene>();
            var validObjectsFromScene = new List<T>();

            for (var i = 0; i < SceneManager.sceneCount; i++)
                scenes.Add(SceneManager.GetSceneAt(i));

            foreach (var s in scenes)
                objects.AddRange(s.GetRootGameObjects());

            foreach (var go in objects)
                validObjectsFromScene.AddRange(go.GetComponentsInChildren<T>(true));
            
            if (instance != null)
            {
                foreach (var obj in instance.gameObject.scene.GetRootGameObjects())
                    validObjectsFromScene.AddRange(obj.GetComponentsInChildren<T>());
            }

            return validObjectsFromScene;
        }
    }
}