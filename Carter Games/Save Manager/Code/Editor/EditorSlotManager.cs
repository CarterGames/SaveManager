using System.Collections.Generic;
using CarterGames.Assets.SaveManager.Slots;

namespace CarterGames.Assets.SaveManager.Editor
{
    public class EditorSlotManager
    {
        public static int TotalSlots => SaveSlotManager.TotalSlotsInUse;
        public static IReadOnlyDictionary<int, SaveSlot> AllSlotsData => SaveSlotManager.AllSlots;

        
        public static void AddNewSlot()
        {
            if (SaveSlotManager.TryCreateSlot(out var newSlot))
            {
                EditorSaveObjectController.AddEditorsForSaveSlot(newSlot);
            }
        }

        
        public static void DeleteSlot(SaveSlot saveSlot)
        {
            SaveSlotManager.DeleteSlot(saveSlot.SlotIndex);
        }
    }
}