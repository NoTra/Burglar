using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

namespace burglar
{
    public class SliderValue : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        private TextMeshProUGUI _valueText;
        [SerializeField] private AudioMixerGroup _audioMixerGroup;
        private float _diffBetweenMaxAndMin;

        private void Awake()
        {
            _valueText = GetComponent<TextMeshProUGUI>();
            
            _diffBetweenMaxAndMin = slider.maxValue - slider.minValue;

            if (PlayerPrefs.HasKey(_audioMixerGroup.name + "Volume"))
            {
                slider.value = PlayerPrefs.GetFloat(_audioMixerGroup.name + "Volume", 0f);                
            }
            else
            {
                slider.value = _audioMixerGroup.audioMixer.GetFloat(_audioMixerGroup.name + "Volume", out var value) ? value : 0f;
            }
            
            
            _valueText.text = Mathf.Ceil((((slider.value + _diffBetweenMaxAndMin) / _diffBetweenMaxAndMin) * 100)) + "%";
        }

        // Start is called before the first frame update
        private void Start()
        {
            slider.onValueChanged.AddListener((float value) =>
            {
                Debug.Log("Value changed to " + value);
                
                // Convert value to percentage
                var percentValue = ((value + _diffBetweenMaxAndMin) / _diffBetweenMaxAndMin) * 100;
                
                Debug.Log("Percent value: " + percentValue);
                
                _valueText.text = Mathf.Ceil(percentValue) + "%";
                
                var audioMixerName = _audioMixerGroup.name;
                
                // Check if the variable audioMixerName + "Volume" exists in the audio mixer
                if (!_audioMixerGroup.audioMixer.FindMatchingGroups(audioMixerName + "Volume").Length.Equals(0))
                {
                    Debug.LogError("AudioMixer group " + audioMixerName + "Volume not found");
                    return;
                }
                
                // Put the value on the audio mixer group volume
                _audioMixerGroup.audioMixer.SetFloat(audioMixerName + "Volume", value);
                
                // Save the value in player prefs
                PlayerPrefs.SetFloat(audioMixerName + "Volume", value);
            });            
        }
    }
}
