using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

namespace burglar
{
    public class UIItem : MonoBehaviour
    {
        [SerializeField] private GameObject itemElementPrefab;
        private List<GameObject> _elements = new List<GameObject>();

        // Start is called before the first frame update
        void Start()
        {
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
            EventManager.SelectItem += (item) => OnSelectItem(item);
        }

        private void OnSelectItem(Item item)
        {
            // Deselect all items
            foreach (var element in _elements)
            {
                var itemElement = element.GetComponent<UIItemElement>();
                itemElement.Deselect();
            }

            // Select the item
            foreach (var element in _elements)
            {
                var itemElement = element.GetComponent<UIItemElement>();
                if (itemElement.item == item)
                {
                    itemElement.Select();
                }
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
