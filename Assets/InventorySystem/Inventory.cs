using System;
using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem
{
    public class Inventory : MonoBehaviour
    {
        public ItemDatabase database;
        public Item placeHolderItem = null;
        public GameObject inventoryPanel;
        public GameObject slotPrefab;

        private IInventoryPanel InventoryPanel
        {
            get
            {
                if (inventoryPanel is null)
                {
                    Debug.LogError("InventoryPanel required");
                    return null;
                }

                var res = inventoryPanel.GetComponent<IInventoryPanel>();
                if (!(res is null)) return res;
                Debug.LogError("InventoryPanel should implement IInventoryPanel");
                return null;

            }
        }

        private ISlotPanel[] _slots;
        private Item[] StoredItems { set; get; }
        private int[] StoredItemAmounts { set; get; }

        public int slotCount;

        private void Start()
        {
            InventoryPanel.Inventory = this;

            _slots = new ISlotPanel[slotCount];
            StoredItems = new Item[slotCount];
            StoredItemAmounts = new int[slotCount];

            for (var i = 0; i < slotCount; i++)
            {
                var slotGameObject = Instantiate(slotPrefab);
                var slotTransform = slotGameObject.transform;
                slotTransform.SetParent(inventoryPanel.transform);
                var slot = slotGameObject.GetComponent<ISlotPanel>();

                if (slot is null)
                {
                    Debug.LogError($"Given slot prefab {slotPrefab} doesn't implement interface {typeof(ISlotPanel)}.");
                    return;
                }

                slot.Inventory = this;
                slot.ID = i;

                _slots[i] = slot;
                StoredItems[i] = placeHolderItem;
                StoredItemAmounts[i] = 0;

                slotGameObject.SetActive(false);
            }

            InventoryPanel.Init();

            // TODO: - Load from PlayerPrefs
        }

        private void ResetSlot(int index)
        {
            StoredItems[index] = placeHolderItem;
            StoredItemAmounts[index] = 0;
            _slots[index].OnItemChanged();
        }

        private const int InvalidStoredItemIndex = -1;

        public int AddItem(string title)
        {
            var itemToAdd = database.GetItemByTitle(title);
            if (itemToAdd is null)
            {
                Debug.LogWarning($"Requested item with title \"{title}\" not found in the database");
                return InvalidStoredItemIndex;
            }

            var existedStoredItemIndex = GetIndexOfItemWithTitle(title);
            if (itemToAdd.stackable && existedStoredItemIndex != InvalidStoredItemIndex)
            {
                StoredItemAmounts[existedStoredItemIndex]++;
                _slots[existedStoredItemIndex].OnItemChanged();
                return existedStoredItemIndex;
            }

            var availableSlotIndex = GetIndexOfAvailableSlot();
            if (availableSlotIndex != InvalidStoredItemIndex)
            {
                StoredItems[availableSlotIndex] = itemToAdd;
                StoredItemAmounts[availableSlotIndex] = 1;
                _slots[availableSlotIndex].OnItemChanged();
                return availableSlotIndex;
            }

            // TODO: - handle slot overflow
            Debug.LogError("Slot overflow!");
            return InvalidStoredItemIndex;
        }

        public void UseItem(int index, int amount = 1)
        {
            var currentAmount = StoredItemAmounts[index];
            if (amount > currentAmount)
            {
                Debug.LogError("Do not have enough item to use");
                return;
            }

            if (amount == currentAmount)
            {
                ResetSlot(index);
                return;
            }

            StoredItemAmounts[index] -= amount;
        }

        public void RemoveItem(int index) => ResetSlot(index);

        public void RemoveAllItems()
        {
            for (var i = 0; i < slotCount; i++)
            {
                ResetSlot(i);
            }
        }

        public void ClearItems()
        {
            for (var i = 0; i < slotCount; i++)
            {
                if (StoredItemAmounts[i] <= 0)
                {
                    ResetSlot(i);
                }
            }
        }

        private int GetIndexOfAvailableSlot()
        {
            for (var i = 0; i < StoredItems.Length; i++)
            {
                if (StoredItems[i] == placeHolderItem)
                {
                    return i;
                }
            }

            return InvalidStoredItemIndex;
        }

        private int GetIndexOfItemWithTitle(string title)
        {
            for (var i = 0; i < StoredItems.Length; i++)
            {
                var item = StoredItems[i];
                if (item.title == title)
                {
                    return i;
                }
            }

            return InvalidStoredItemIndex;
        }

        private void OnDisable() => inventoryPanel.SetActive(false);

        private void OnEnable() => inventoryPanel.SetActive(true);

        public Item GetItem(int index) => StoredItems[index];

        public int GetAmount(int index) => StoredItemAmounts[index];

        public bool IsSlotEmpty(int index) => StoredItems[index] == placeHolderItem;

        public void SwapSlots(int index1, int index2)
        {
            var tmp = _slots[index1];
            _slots[index1] = _slots[index2];
            _slots[index2] = tmp;
        }

        public ISlotPanel[] GetSlots()
        {
            return _slots;
        }
    }
}