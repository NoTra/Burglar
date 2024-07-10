using burglar.managers;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace burglar.UI
{
    public class HoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] TextMeshProUGUI _text;
        [SerializeField] TMP_FontAsset _hoverFont;
        [SerializeField] private bool triggerSoundOnHover = true;
        [SerializeField] private bool triggerSoundOnClick = true;
        private Color _defaultColor;
        
        private void Awake()
        {
            if (_text == null) _text = GetComponent<TextMeshProUGUI>();
            
            _defaultColor = _text.color;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (triggerSoundOnHover)
            {
                AudioManager.Instance.PlaySFX(AudioManager.Instance.soundHover);                
            }
            
            _text.color = new Color(164f / 255f, 113f / 255f, 38f / 255f);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _text.color = _defaultColor;
        }
        
        public void OnClick()
        {
            if (triggerSoundOnClick)
            {
                AudioManager.Instance.PlaySFX(AudioManager.Instance.soundClick);                
            }
        }
    }
}
