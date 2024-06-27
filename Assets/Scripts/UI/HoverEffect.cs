using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace burglar.UI
{
    public class HoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] TextMeshProUGUI _text;
        [SerializeField] TMP_FontAsset _hoverFont;
        private Color _defaultColor;

        private void Awake()
        {
            _defaultColor = _text.color;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _text.color = new Color(164f / 255f, 113f / 255f, 38f / 255f);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _text.color = _defaultColor;
        }
    }
}
