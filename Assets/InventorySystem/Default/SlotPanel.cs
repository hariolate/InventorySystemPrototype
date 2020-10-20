using System;
using UnityEngine;
using UnityEngine.UI;

namespace InventorySystem.Default
{
    public class SlotPanel : MonoBehaviour, ISlotPanel
    {
        public Inventory Inventory { get; set; }
        public int ID { get; set; }

        private Item Item => Inventory.GetItem(ID);
        private int Amount => Inventory.GetAmount(ID);
        private bool Empty => Inventory.IsSlotEmpty(ID);

        public GameObject iconImageGameObject;
        public GameObject amountTextGameObject;
        private Image Icon => iconImageGameObject.GetComponent<Image>();
        private Text AmountText => amountTextGameObject.GetComponent<Text>();
        
        public Sprite defaultIcon;
        
        public void OnItemChanged()
        {
            if(Item!=null){
                Icon.sprite = Item.icon;
                AmountText.text = Amount==0?"":Amount.ToString();
                return;
            }

            Icon.sprite = defaultIcon;
            AmountText.text = "";
        }

        public void Init()
        {
            OnItemChanged();
        }

        public GameObject GameObject => gameObject;

        private void OnEnable()
        {
            if (Inventory is null) return;
            OnItemChanged();
        }
    }
}