using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using burglar.managers;
using burglar.environment;

namespace burglar.UI
{
    public class CombinationButton : MonoBehaviour, IPointerClickHandler// , IPointerEnterHandler, IPointerExitHandler
    {
        public Image _background;

        [SerializeField] private Vector2 _coordinates;
        [SerializeField] private Safe _safe;
        public bool isSelected;
        public bool isClickable = true;

        private Color _previousColor;

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
            _previousColor = _background.color;
            _background.color = UIManager.Instance._hoverColor;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _background.color = _previousColor;
        }
    }
}
