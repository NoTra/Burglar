using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using burglar.managers;
using burglar.environment;
using TMPro;

namespace burglar.UI
{
    public class CombinationButton : MonoBehaviour, IPointerClickHandler , IPointerEnterHandler, IPointerExitHandler
    {
        public Image _background;

        [SerializeField] private Vector2 _coordinates;
        [SerializeField] private Safe _safe;
        public bool isSelected;
        public bool isClickable = true;

        private Color _previousColor;
        private Color _previousBGColor;
        
        public TextMeshProUGUI _text;

        public Vector2 Coordinates
        {
            get => _coordinates;
            set => _coordinates = value;
        }

        public Safe Safe
        {
            get => _safe;
            set => _safe = value;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!isClickable) return;
            
            var audioManager = AudioManager.Instance;
            audioManager.PlaySFX(audioManager.soundClick);
            
            isSelected = !isSelected;
            _safe.AddToCombination(_coordinates);

            // The length is different from the combination length ?
            if (_safe.GetSelectedCombinationLength() != _safe.GetCombinationLength()) return;
            
            // Check if the combination is correct
            if (_safe.CheckCombination())
            {
                EventManager.OnSuccessSafeCrack(_safe);
            }
            else
            {
                EventManager.OnFailSafeCrack(_safe);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            var audioManager = AudioManager.Instance;
            audioManager.PlaySFX(audioManager.soundHover);
            
            if (!isClickable || isSelected) return;
            
            _previousColor = _text.color;
            _previousBGColor = _background.color;
            _background.color = UIManager.Instance._hoverBGColor;
            _text.color = UIManager.Instance._hoverColor;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!isClickable || isSelected) return;
            
            _background.color = _previousBGColor;
            _text.color = _previousColor;
        }
    }
}
