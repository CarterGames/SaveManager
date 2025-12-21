using System.Collections.Generic;
using CarterGames.Assets.SaveManager.Slots;
using UnityEngine;

namespace CarterGames.Assets.SaveManager.Demo
{
    public class ExampleSlotMenuDisplayManager : MonoBehaviour
    {
        [SerializeField] private Transform slotDisplayParent;
        [SerializeField] private GameObject slotDisplayPrefab;


        private Dictionary<int, ExampleSlotDisplay> displaysLookup;
        

        private void OnEnable()
        {
            displaysLookup = new Dictionary<int, ExampleSlotDisplay>();
            
            if (!SaveManager.IsLoading)
            {
                SpawnSlotObjects();
            }
            else
            {
                SaveManager.GameLoadedEvt.Add(SpawnSlotObjects);
            }
            
            SaveSlotManager.SlotCreatedEvt.Add(OnSlotCreated);
        }


        private void OnDisable()
        {
            SaveManager.GameLoadedEvt.Remove(SpawnSlotObjects);
            SaveSlotManager.SlotCreatedEvt.Remove(OnSlotCreated);
        }


        private void SpawnSlotObjects()
        {
            SaveManager.GameLoadedEvt.Remove(SpawnSlotObjects);

            var slotIndexesSpawned = new List<int>();
            
            foreach (var entry in SaveSlotManager.AllSlots)
            {
                var instance = Instantiate(slotDisplayPrefab, slotDisplayParent);
                var instanceDisplayScript = instance.GetComponentInChildren<ExampleSlotDisplay>();
                
                instanceDisplayScript.AssignSlot(entry.Value);
                slotIndexesSpawned.Add(entry.Key);
                displaysLookup.Add(entry.Key, instanceDisplayScript);
            }
            
            // Extra slots
            if (SaveSlotManager.TotalSlotsRestricted)
            {
                // Spawn all remaining slot indexes in the right place.
                // So if limited to x slots it'll show the rest as empty.
                for (var i = 0; i < SaveSlotManager.RestrictedSlotsTotal; i++)
                {
                    // Skip if already spawned.
                    if (slotIndexesSpawned.Contains(i)) continue;
                    
                    var instance = Instantiate(slotDisplayPrefab, slotDisplayParent);
                    var instanceDisplayScript = instance.GetComponentInChildren<ExampleSlotDisplay>();
                    
                    instanceDisplayScript.UpdateDisplay();
                    instance.transform.SetSiblingIndex(i);
                    displaysLookup.Add(i, instanceDisplayScript);
                }
                
            }
            else
            {
                // Spawn an extra slot to allow new slots to be added.
                var instance = Instantiate(slotDisplayPrefab, slotDisplayParent);
                var instanceDisplayScript = instance.GetComponentInChildren<ExampleSlotDisplay>();
                    
                instanceDisplayScript.UpdateDisplay();
                instance.transform.SetAsLastSibling();
                displaysLookup.Add(instance.transform.GetSiblingIndex(), instanceDisplayScript);
            }
        }


        private void OnSlotCreated(SaveSlot newSlot)
        {
            if (displaysLookup.ContainsKey(newSlot.SlotIndex))
            {
                displaysLookup[newSlot.SlotIndex].AssignSlot(newSlot);
            }

            if (SaveSlotManager.TotalSlotsRestricted) return;
            
            // Spawn an extra slot to allow new slots to be added.
            var instance = Instantiate(slotDisplayPrefab, slotDisplayParent);
            var instanceDisplayScript = instance.GetComponentInChildren<ExampleSlotDisplay>();
                    
            instanceDisplayScript.UpdateDisplay();
            instance.transform.SetAsLastSibling();
            displaysLookup.Add(instance.transform.GetSiblingIndex(), instanceDisplayScript);
        }
    }
}