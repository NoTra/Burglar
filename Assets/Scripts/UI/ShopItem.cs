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
            if (GameManager.Instance.credit >= _item.price)
            {
                GameManager.Instance.credit -= _item.price;
                GameManager.Instance.items.Add(_item);
                _buyButton.interactable = false;
                _buyButtonText.text = "Owned";

                EventManager.OnCreditChanged();
            }
            else
            {
                Debug.Log("Not enough credits");
            }
            
        }
    }
}
