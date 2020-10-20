using System;
using UnityEngine;

namespace InventorySystem.Default
{
    public class InventoryPanel : MonoBehaviour, IInventoryPanel
    {
        public Inventory Inventory { get; set; }

        public GameObject fullPanel;
        public GameObject collapsedPanel;
        
        public bool isCollapsed = true;

        private ISlotPanel[] Slots => Inventory.GetSlots();

        private void SetSlotParents(GameObject parent)
        {
            foreach (var slot in Slots)
            {
                slot.GameObject.transform.SetParent(parent.transform);
            }
        }
        
        public void Collapse()
        {
            isCollapsed = true;
            fullPanel.SetActive(false);
            SetSlotParents(collapsedPanel);
            collapsedPanel.SetActive(true);
        }

        public void Expand()
        {
            isCollapsed = false;
            collapsedPanel.SetActive(false);
            SetSlotParents(fullPanel);
            fullPanel.SetActive(true);
        }

        public void Init()
        {
            if (isCollapsed)
            {
                Collapse();
            }
            else
            {
                Expand();
            }
        }

        private void OnEnable()
        {
            if (Inventory is null) return;
            Init();
        }

        private void OnDisable()
        {
            fullPanel.SetActive(false);
            collapsedPanel.SetActive(false);
        }
    }
}