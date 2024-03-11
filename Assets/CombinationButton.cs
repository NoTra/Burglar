using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace burglar
{
    public class CombinationButton : MonoBehaviour, IPointerClickHandler// , IPointerEnterHandler, IPointerExitHandler
    {
        public Image _background;

        [SerializeField] private Vector2 _coordinates;
        [SerializeField] private Safe _safe;
        public bool _isSelected;
        public bool _isClickable = true;

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

        // Start is called before the first frame update
        void Start()
        {
            _isSelected = false;
            _isClickable = true;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("Clicked on " + _coordinates);
            if (_isClickable)
            {
                _isSelected = !_isSelected;
                _safe.AddToCombination(_coordinates);

                Debug.Log("CombinationLength : " + _safe.GetCombinationLength());
                Debug.Log("Try length : " + _safe.GetSelectedCombinationLength());

                // The length is the same as the combination length ? 
                if (_safe.GetSelectedCombinationLength() == _safe.GetCombinationLength())
                {
                    // Check if the combination is correct
                    if (_safe.CheckCombination())
                    {
                        EventManager.OnSuccessSafeCrack(_safe);
                    }
                    else
                    {
                        EventManager.OnFailSafeCrack();
                    }

                }
            }
        }

        /*public void OnPointerEnter(PointerEventData eventData)
        {
            _previousColor = _background.color;
            _background.color = UIManager.Instance._hoverColor;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _background.color = _previousColor;
        }*/
    }
}
