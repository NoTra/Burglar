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
            // if button selected, don't change color
            if (EventSystem.current.currentSelectedGameObject == gameObject)
            {
                Debug.Log("Button selected");
            }
            else
            {
                Debug.Log("Button not selected so we change color");
                _text.color = _defaultColor;
            }

            
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
