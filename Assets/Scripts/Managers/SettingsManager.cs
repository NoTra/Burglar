using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

namespace burglar
{
    public class SettingsManager : MonoBehaviour
    {
        [SerializeField] private Toggle _toggleFullscreen;
        [SerializeField] private TMP_Dropdown _resolutionDropdown;

        private List<Resolution> _resolutions;
        
        public Button _backButton;

        private void Awake()
        {
            if (PlayerPrefs.HasKey("fullscreen"))
            {
                Screen.fullScreen = PlayerPrefs.GetInt("fullscreen") == 1;
            }
            
            if (PlayerPrefs.HasKey("resolution"))
            {
                var resolutionIndex = PlayerPrefs.GetInt("resolution");
                
                _resolutions = Screen.resolutions
                    .Select(res => new Resolution { width = res.width, height = res.height })
                    .Distinct()
                    .OrderByDescending(res => res.width * res.height)
                    .ToList();
                
                Screen.SetResolution(_resolutions[resolutionIndex].width, _resolutions[resolutionIndex].height, Screen.fullScreen);
            }
        }

        // Start is called before the first frame update
        private void Start()
        {
            _toggleFullscreen.isOn = Screen.fullScreen;
            
            _toggleFullscreen.onValueChanged.AddListener((bool value) =>
            {
                Screen.fullScreen = value;
                
                // Save the value in player prefs
                PlayerPrefs.SetInt("fullscreen", value ? 1 : 0);
            });
            
            InitResolutionDropdownOptions();
            
            _resolutionDropdown.onValueChanged.AddListener((int value) =>
            {
                Screen.SetResolution(_resolutions[value].width, _resolutions[value].height, Screen.fullScreen);
                
                // Save the value in player prefs
                PlayerPrefs.SetInt("resolution", value);
            });
        }

        private void InitResolutionDropdownOptions()
        {
            _resolutionDropdown.ClearOptions();

            var options = new System.Collections.Generic.List<string>();
            var currentResolution = Screen.currentResolution;
            
            var currentIndex = 0;
            for (var i = 0; i < _resolutions.Count; i++)
            {
                var resolution = _resolutions[i];
                options.Add(resolution.width + "x" + resolution.height);

                if (resolution.width == currentResolution.width && resolution.height == currentResolution.height)
                {
                    currentIndex = i;
                }
            }

            _resolutionDropdown.AddOptions(options);
            _resolutionDropdown.value = currentIndex;
            _resolutionDropdown.RefreshShownValue(); // Rafraîchir la valeur sélectionnée
        }
    }
}
