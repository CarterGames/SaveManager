using System.Globalization;
using UnityEngine;

namespace CarterGames.Assets.SaveManager
{
    public static class SaveManagerPrefs
    {
        private static string Internal_GetKey(string key) => $"{SaveManagerConstants.PrefFormat}_{key}";
        
        
        public static int GetIntKey(string key) => int.Parse(Internal_GetKey<int>(key));
        public static bool GetBoolKey(string key) => bool.Parse(Internal_GetKey<bool>(key));
        public static float GetFloatKey(string key) => float.Parse(Internal_GetKey<float>(key));
        public static string GetStringKey(string key) => Internal_GetKey<string>(key);
        
        
        private static string Internal_GetKey<T>(string key)
        {
            var prefKey = Internal_GetKey(key);
            
            if (PlayerPrefs.HasKey(prefKey))
            {
                return PlayerPrefs.GetString(prefKey);
            }

            PlayerPrefs.SetString(prefKey, default(T)?.ToString());
            return default(T)?.ToString();
        }
        
        
        public static void SetKey(string key, int value) => Internal_SetKey<int>(key, value.ToString());
        public static void SetKey(string key, bool value) => Internal_SetKey<bool>(key, value.ToString());
        public static void SetKey(string key, float value) => Internal_SetKey<float>(key, value.ToString(CultureInfo.InvariantCulture));
        public static void SetKey(string key, string value) => Internal_SetKey<string>(key, value);
        
        
        private static void Internal_SetKey<T>(string key, string value)
        {
            PlayerPrefs.SetString(Internal_GetKey(key), value);
        }
    }
}