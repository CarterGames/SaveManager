using CarterGames.Assets.SaveManager.Slots;
using UnityEngine;

namespace CarterGames.Assets.SaveManager.Demo
{
    public class ExampleSlotMenuManager : MonoBehaviour
    {
        private void OnEnable()
        {
            SaveSlotManager.SlotLoadedEvt.Add(OnSlotLoadedSuccessfully);
            SaveSlotManager.SlotLoadFailedEvt.Add(OnSlotLoadFailed);
        }


        private void OnDisable()
        {
            SaveSlotManager.SlotLoadedEvt.Remove(OnSlotLoadedSuccessfully);
            SaveSlotManager.SlotLoadFailedEvt.Remove(OnSlotLoadFailed);
        }


        private void OnSlotLoadedSuccessfully(int loadedSlotIndex)
        {
            // Here you'd run your logic to load the game from the current slot data.
            // You may want to add a dialog that shows the game loaded successfully as well.
            Debug.Log($"Slot {loadedSlotIndex} loaded, game would now move to load & change scene etc.");
        }


        private void OnSlotLoadFailed(int loadedSlotIndex)
        {
            // Here you'd show a load failed dialog.
            // Unlikely to ever occur, but in place in-case of an issue with the slot you are trying to load not being loaded.
            Debug.Log($"Slot {loadedSlotIndex} failed to load, game should show a failed dialog here.");
        }
    }
}