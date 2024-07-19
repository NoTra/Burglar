using burglar.managers;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace burglar.UI
{
    public class HoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] TextMeshProUGUI _text;
        
        [Header("Sounds")]
        [SerializeField] private bool triggerSoundOnHover = true;
        [SerializeField] private bool triggerSoundOnClick = true;
        
        private Color _defaultColor;
        [FormerlySerializedAs("_hoverColor")] 
        public Color hoverColor = new(164f / 255f, 113f / 255f, 38f / 255f);
        
        private void Awake()
        {
            if (_text == null) _text = GetComponentInChildren<TextMeshProUGUI>();
            
            _defaultColor = _text.color;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (triggerSoundOnHover)
            {
                AudioManager.Instance.PlaySFX(AudioManager.Instance.soundHover);                
            }
            
            _text.color = hoverColor;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (EventSystem.current.currentSelectedGameObject == gameObject) return;
            
            _text.color = _defaultColor;
        }
        
        public void OnClick()
        {
            if (!triggerSoundOnClick) return;
            
            AudioManager.Instance.PlaySFX(AudioManager.Instance.soundClick);                
        }
    }
}
