using System.Collections.Generic;
using UnityEngine;
using burglar.UI;
using burglar.items;

namespace burglar.managers
{
    public class ShopManager : MonoBehaviour
    {
        [SerializeField] private GameObject _itemGridContainer;
        [SerializeField] private GameObject _shopItemPrefab;
        [SerializeField] private List<Item> _availableItems;
        
        private void Start()
        {
            // Add all items to the shop
            foreach (var item in _availableItems)
            {
                if (item.isPurchasable == false) continue;

                var shopItem = Instantiate(_shopItemPrefab, _itemGridContainer.transform);
                shopItem.GetComponent<ShopItem>().SetItem(item);
            }
        }

        public void SaveAndExitButton()
        {
            // Trigger save event
            EventManager.OnSave();

            // Load main menu
            LevelManager.Instance.LoadScene("main");
        }

        public void SaveAndContinueButton()
        {
            // Load next level
            LevelManager.Instance.LoadNextLevel();

            // Trigger save event
            EventManager.OnSave();
        }
    }
}
