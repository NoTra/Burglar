using System.Collections;
using System.Collections.Generic;
using burglar.UI;
using UnityEngine;
using UnityEngine.UI;

namespace burglar
{
    public class Tab : MonoBehaviour
    {
        public bool autoSelectFirstTab = true; 
        public Button firstTabButton;
        public GameObject buttonContainer;
        public GameObject panelContainer;
        
        private GameObject _currentSelectedPanel;
        private Button _currentSelectedButton;
        
        // Start is called before the first frame update
        private void Start()
        {
            // Loop over all buttons in buttonContainer and add a listener on click
            foreach (Transform button in buttonContainer.transform)
            {
                button.GetComponent<Button>().onClick.AddListener(() => OnTabSelected(button.gameObject));
            }
            
            if (autoSelectFirstTab)
            {
                // Simulate click on first tab button
                
                firstTabButton.onClick.Invoke();
            }
        }

        private void OnTabSelected(GameObject buttonGameObject)
        {
            // Loop over all panels in panelContainer and deactivate them
            foreach (Transform panel in panelContainer.transform)
            {
                panel.gameObject.SetActive(false);
            }
            
            // Loop over all buttons in buttonContainer and deactivate the selected state
            foreach (Transform button in buttonContainer.transform)
            {
                var tabElement = button.GetComponent<TabElement>();
                tabElement.isSelected = (button.gameObject == buttonGameObject);
                tabElement.ColorsUpdate();
            }
            
            // Activate the panel corresponding to the button clicked
            var selectedTabElement = buttonGameObject.GetComponent<TabElement>();
            selectedTabElement.targetPanel.SetActive(true);
        }
    }
}
