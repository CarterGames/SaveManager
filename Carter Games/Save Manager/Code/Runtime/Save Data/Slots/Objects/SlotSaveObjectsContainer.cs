using System.Collections.Generic;
using UnityEngine;

namespace CarterGames.Assets.SaveManager.Slots
{
    public class SlotSaveObjectsContainer
    {
        public IEnumerable<SaveObject> SlotSaveObjects { get; private set; }
        
        
        public SlotSaveObjectsContainer()
        {
            var list = new List<SaveObject>();

            var saveObjectsTypes = SaveObjectController.SlotSaveObjectTypes;

            foreach (var soType in saveObjectsTypes)
            {
                list.Add((SaveObject)ScriptableObject.CreateInstance(soType));
            }
            

            SlotSaveObjects = list;
        }
    }
}