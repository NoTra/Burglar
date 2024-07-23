using UnityEngine;
using UnityEngine.UI;
using burglar.items;

namespace burglar.UI
{
    public class UIItemElement : MonoBehaviour
    {
        private Image _border;
        public bool isSelected = false;
        public Item item;
        [SerializeField] private Image _iconElement;

        private void Awake()
        {
            _border = GetComponent<Image>();
        }

        // Start is called before the first frame update
        void Start()
        {
            _iconElement.sprite = item.icon;
        }

        public void Select()
        {
            isSelected = true;
            _border.color = Color.white;
        }

        public void Deselect()
        {
            isSelected = false;
            _border.color = new Color(255, 255, 255, 0);
        }
    }
}
