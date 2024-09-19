using System.Collections.Generic;
using UnityEngine;
using burglar.managers;
using burglar.items;

namespace burglar.UI
{
    public class UIItem : MonoBehaviour
    {
        [SerializeField] private GameObject itemElementPrefab;
        private List<GameObject> _elements = new List<GameObject>();

        // Start is called before the first frame update
        private void Start()
        {
            InitItems();
        }

        public void InitItems()
        {
            // Clear all children
            foreach (Transform child in transform)
            {
                _elements.Remove(child.gameObject);
                Destroy(child.gameObject);
            }
            
            // Add items possessed by the player
            var i = 0;
            foreach (var item in GameManager.Instance.items)
            {
                var element = Instantiate(itemElementPrefab, transform);
                var itemElement = element.GetComponent<UIItemElement>();
                itemElement.item = item;
                _elements.Add(element);
                if (i == 0)
                {
                    itemElement.Select();
                }
                else
                {
                    itemElement.Deselect();
                }
                
                i++;
            }
        }

        private void OnEnable()
        {
            EventManager.SelectItem += OnSelectItem;
        }
        
        private void OnDisable()
        {
            EventManager.SelectItem -= OnSelectItem;
        }

        private void OnSelectItem(Item item)
        {
            // Select the item and deselect the rest
            foreach (var element in _elements)
            {
                var itemElement = element.GetComponent<UIItemElement>();
                if (itemElement.item == item)
                {
                    itemElement.Select();
                }
                else
                {
                    itemElement.Deselect();
                }
            }
        }
    }
}
