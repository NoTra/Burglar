using UnityEngine;
using burglar.UI;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

namespace burglar
{
    public class TabElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public GameObject targetPanel;
        public bool isSelected = false;

        private Button _button;
        private HoverEffect _hoverEffect;
        private TextMeshProUGUI _text;
        private Image _bgImage;
        
        private Color _defaultColor;
        [SerializeField] private Color _hoverColor;
        
        [SerializeField] private Color _defaultBgColor;
        [SerializeField] private Color _hoverBgColor;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _text = GetComponentInChildren<TextMeshProUGUI>();
            _bgImage = GetComponent<Image>();
            
            _defaultColor = _text.color;
            _defaultBgColor = _button.colors.normalColor;
            _hoverBgColor = _button.colors.highlightedColor;
        }
        
        private void Start()
        {
            
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (isSelected) return;
            
            _text.color = _hoverColor;
            _bgImage.color = _hoverBgColor;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (isSelected) return;
            
            _text.color = _defaultColor;
            _bgImage.color = _defaultBgColor;
        }

        public void ColorsUpdate()
        {
            // if (_defaultColor == new Color(0, 0, 0, 0))
            // {
            //     _defaultColor = new Color(255, 255, 255, 1);
            // }
            if (!isSelected)
            {
                _text.color = _defaultColor;
                _bgImage.color = _defaultBgColor;
            }
            else
            {
                _text.color = _hoverColor;
                _bgImage.color = _hoverBgColor;
            }
        }
    }
    
    
}
