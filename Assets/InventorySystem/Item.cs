using UnityEngine;

namespace InventorySystem
{
    [CreateAssetMenu(fileName = "NewItem", menuName = "InventorySystem/Item", order = 0)]
    public class Item : ScriptableObject
    {
        public string title;
        public string description;
        public Sprite icon;
        public bool stackable;
    }
}