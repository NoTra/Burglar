using TMPro;
using UnityEngine;
using UnityEngine.UI;
using burglar.managers;
using burglar.items;

namespace burglar.UI
{
    public class ShopItem : MonoBehaviour
    {
        private Item _item;
        [SerializeField] private TextMeshProUGUI _title;
        [SerializeField] private TextMeshProUGUI _price;
        [SerializeField] private TextMeshProUGUI _description;
        [SerializeField] private Image _icon;
        [SerializeField] private Button _buyButton;
        private TextMeshProUGUI _buyButtonText;

        [SerializeField] private Color disableColor;
        [SerializeField] private Color disableBorderColor;

        [SerializeField] private GameObject _notAvailable;

        private void Awake() {
            _buyButtonText = _buyButton.GetComponentInChildren<TextMeshProUGUI>();
        }

        private void Start()
        {
            if (GameManager.Instance.items.Contains(_item))
            {
                _buyButton.interactable = false;
                _buyButtonText.text = "Owned";
            }
            else
            {
                _buyButton.onClick.AddListener(BuyItem);
            }
            
            _notAvailable.SetActive(!_item.isPurchasable);
            
            if (!_item.isPurchasable)
            {
                _title.color = disableColor;
                _price.color = disableColor;
                _description.color = disableColor;
                GetComponent<Image>().color = disableBorderColor;
            }
        }

        public void SetItem(Item item)
        {
            _item = item;
            _title.text = item.name;
            _description.text = item.description;
            _price.text = item.price.ToString();
            _icon.sprite = item.icon;
        }

        private void BuyItem()
        {
            if (GameManager.Instance.credit < _item.price) return;
            
            GameManager.Instance.credit -= _item.price;
            GameManager.Instance.items.Add(_item);
            
            // Update UI to reflect the purchase
            UIManager.Instance.UpdateItems();
            
            
            _buyButton.interactable = false;
            _buyButtonText.text = "Owned";

            EventManager.OnCreditChanged();
        }
    }
}
