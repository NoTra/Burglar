using TMPro;
using UnityEngine;
using burglar.managers;

namespace burglar.UI
{
    public class ShopCredit : MonoBehaviour
    {
        private TextMeshProUGUI text;

        private void Awake()
        {
            text = GetComponent<TextMeshProUGUI>();
        }

        void Start()
        {
            UpdateCredits();
        }

        private void OnEnable()
        {
            EventManager.CreditChanged += UpdateCredits;
        }

        private void OnDisable()
        {
            EventManager.CreditChanged -= UpdateCredits;
        }

        public void UpdateCredits()
        {
            text.text = "Credit : " + GameManager.Instance.credit.ToString();
        }
    }
}
