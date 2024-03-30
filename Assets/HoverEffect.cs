using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TextCore.Text;

namespace burglar
{
    public class HoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] TextMeshProUGUI _text;
        TMP_FontAsset _defaultFont;
        [SerializeField] TMP_FontAsset _hoverFont;
        private Color _defaultColor;

        private void Awake()
        {
            _defaultFont = _text.font;
            _defaultColor = _text.color;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            // _text.font = _hoverFont;
            // #A47126
            _text.color = new Color(164f / 255f, 113f / 255f, 38f / 255f);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            // _text.font = _defaultFont;
            _text.color = _defaultColor;
        }
    }
}
