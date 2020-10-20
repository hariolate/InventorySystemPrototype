using System;
using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem.Default
{
    public class SlotsActivator : MonoBehaviour
    {
        private ISlotPanel[] Slots
        {
            get
            {
                var slots = new List<ISlotPanel>();
                for (var i = 0; i < transform.childCount; i++)
                {
                    var child = transform.GetChild(i);
                    var panel = child.GetComponent<ISlotPanel>();
                    if (panel != null)
                    {
                        slots.Add(panel);
                    }
                }

                return slots.ToArray();
            }
        }

        private void OnEnable()
        {
            Debug.Log(Slots);
            foreach (var slot in Slots)
            {
                slot.GameObject.SetActive(true);
            }
        }

        private void OnDisable()
        {
            foreach (var slot in Slots)
            {
                slot.GameObject.SetActive(false);
            }
        }
    }
}