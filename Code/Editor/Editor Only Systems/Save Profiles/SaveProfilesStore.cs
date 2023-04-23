using System;
using System.Collections.Generic;
using UnityEngine;

namespace CarterGames.Assets.SaveManager.Editor
{
    [Serializable]
    [CreateAssetMenu(fileName = "Save Profile Store", menuName = "Carter Games/Save Manager/Save Profile Store", order = 6)]
    public sealed class SaveProfilesStore : ScriptableObject
    { 
        [SerializeField] private List<TextAsset> profiles; 
        
        
        public List<TextAsset> Data
        {
            get => profiles;
            set => profiles = value;
        }


        public void AddProfile(TextAsset saveData)
        {
            profiles ??= new List<TextAsset>();
            profiles.Add(saveData);
        }

        
        public void RemoveProfile(TextAsset profile)
        {
            if (!profiles.Contains(profile)) return;
            profiles.Remove(profile);
        }
    }
}