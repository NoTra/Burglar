using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace burglar
{
    public class SettingsManager : MonoBehaviour
    {
        [SerializeField] private Toggle _toggleFullscreen;
        [SerializeField] private TMP_Dropdown _resolutionDropdown;
        
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
                var resolutions = Screen.resolutions;
                Screen.SetResolution(resolutions[resolutionIndex].width, resolutions[resolutionIndex].height, Screen.fullScreen);
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
                var resolutions = Screen.resolutions;
                Screen.SetResolution(resolutions[value].width, resolutions[value].height, Screen.fullScreen);
                
                // Save the value in player prefs
                PlayerPrefs.SetInt("resolution", value);
            });
        }

        private void InitResolutionDropdownOptions()
        {
            Debug.Log("InitResolutionDropdownOptions");
            _resolutionDropdown.ClearOptions();
            
            var resolutions = Screen.resolutions;
            var options = new System.Collections.Generic.List<string>();
            var currentResolution = Screen.currentResolution;
            
            foreach (var resolution in resolutions)
            {
                options.Add(resolution.width + "x" + resolution.height);
                
                if (resolution.width == currentResolution.width && resolution.height == currentResolution.height)
                {
                    _resolutionDropdown.value = resolutions.Length - 1;
                }
            }
            
            _resolutionDropdown.AddOptions(options);
        }
    }
}
