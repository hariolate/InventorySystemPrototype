using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace InventorySystem
{
    [CreateAssetMenu(fileName = "NewItemDatabase", menuName = "InventorySystem/Item Database", order = 0)]
    public class ItemDatabase : ScriptableObject
    {
        public List<Item> items;

        public Item GetItemByTitle(string title)
        {
            return items.FirstOrDefault(item => item.title == title);
        }

        public Dictionary<string, Item> ItemDictionary
        {
            get
            {
                var dic = new Dictionary<string, Item>();
                foreach (var item in items)
                {
                    dic[item.title] = item;
                }
                return dic;
            }
        }
    }
    
}